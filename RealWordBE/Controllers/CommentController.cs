using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Models;
using RealWord.DB.Models.RequestDtos.OuterDtos;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Services;
using System.Linq;
using System.Threading.Tasks;

namespace RealWordBE.Controllers
{
    [Route("api/articles/{slug}/comments")]
    [ApiController]
    public class CommentController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;
        private readonly IProfileService _profileService;

        public CommentController(IMapper mapper ,ICommentService commentService ,IProfileService profileService)
        {

            _mapper = mapper;
            _commentService = commentService;
            _profileService = profileService;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CommentResponseDto>> CreateComment(string slug ,CommentOuterDto commentOuterDto)
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


            return Ok(commentResponse);

        }
    }
}
