using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Models;
using RealWord.DB.Models.RequestDtos.OuterDtos;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Models.ResponseDtos.OuterResponseDto;
using RealWord.DB.Services;
using System.Linq;
using System.Threading.Tasks;

namespace RealWordBE.Controllers
{
    [Route("api/articles/{slug}/comments")]
    [ApiController]
    public class CommentController:ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {

            _commentService = commentService;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CommentResponseOuterDto>> CreateComment(string slug ,CommentOuterDto commentOuterDto)
        {
            var commentDto = commentOuterDto.CommentDto;
            var CurrentUserName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            var commentId = await _commentService.CreateComment(CurrentUserName ,slug ,commentDto);
            if( commentId == -1 )
                return BadRequest(new Error()
                {
                    Status = "400" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = "Invalid Input  "
                });

            //get comment
            var commentResponse = await _commentService.GetCommentResponse(commentId ,slug ,CurrentUserName);
            var response = new CommentResponseOuterDto() { Comment = commentResponse };


            return Ok(response);

        }
        [HttpGet]
        public async Task<ActionResult<CommentsResponseOuterDto>> GetComments(string slug)
        {
            var currentUserName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

            var result = await _commentService.GetComments(slug ,currentUserName);
            if( result == null )
            {
                return BadRequest(new Error()
                {
                    Status = "400" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = "Invalid Slug "
                });
            }
            var response = new CommentsResponseOuterDto() { Comments = result };


            return Ok(response);

        }
        [Authorize]

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComments(string slug ,int id)
        {
            var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var currentUserName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            var result = await _commentService.GetCommentAsync(id);
            if( result == null )
                return BadRequest(new Error()
                {
                    Status = "400" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = "Invalid Comment Id  "
                });
            bool isAuthor = await _commentService.DoesUserMatchAuthorAsync(id ,currentUserId);
            if( isAuthor == false )
                return Forbid("permission denied ,users cant delete others comments");

            var comments = await _commentService.GetComments(slug ,currentUserName);
            if( comments == null )
                return BadRequest(new Error()
                {
                    Status = "400" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = "Invalid Slug or No comment Belong to slug"
                });
            var isCommentIdBelongToSlug = comments.Where(A => A.CommentId == id).Any();
            if( !isCommentIdBelongToSlug ) return BadRequest(new Error()
            {
                Status = "400" ,
                Tittle = "Bad Request" ,
                ErrorMessage = "Comment Not belong to Slug"
            });
            await _commentService.RemoveComment(id);
            return NoContent();
        }
    }
}
