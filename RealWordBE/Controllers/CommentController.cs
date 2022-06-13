using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Models;
using RealWord.DB.Models.RequestDtos.OuterDtos;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Models.ResponseDtos.OuterResponseDto;
using RealWord.DB.Services;
using RealWordBE.Authentication.Logout;
using System.Linq;
using System.Threading.Tasks;

namespace RealWordBE.Controllers
{
    [Route("api/articles/{slug}/comments")]
    [ApiController]
    public class CommentController:ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ITokenManager _tokenManager;

        public CommentController(ICommentService commentService ,ITokenManager tokenManager)
        {

            _commentService = commentService;
            _tokenManager = tokenManager;
        }
        // [Authorize]
        [HttpPost]
        public async Task<ActionResult<CommentResponseOuterDto>> CreateComment(string slug ,CommentOuterDto commentOuterDto)
        {
            var commentDto = commentOuterDto.CommentDto;
            var token = _tokenManager.GetCurrentTokenAsync();
            if( token == string.Empty ) return Unauthorized();
            var tokens = _tokenManager.ExtractClaims(token);

            var CurrentUserName = tokens.Claims.First(claim => claim.Type == "username").Value;
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
            var token = _tokenManager.GetCurrentTokenAsync();
            string CurrentUserName = null;
            if( token != string.Empty )
            {
                var tokens = _tokenManager.ExtractClaims(token);
                CurrentUserName = tokens.Claims.First(claim => claim.Type == "username").Value;
            }

            var result = await _commentService.GetComments(slug ,CurrentUserName);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComments(string slug ,int id)
        {
            var token = _tokenManager.GetCurrentTokenAsync();
            if( token == string.Empty ) return Unauthorized();
            var tokens = _tokenManager.ExtractClaims(token);

            var CurrentUserName = tokens.Claims.First(claim => claim.Type == "username").Value;
            var currentUserId = tokens.Claims.First(claim => claim.Type == "username").Value;

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
