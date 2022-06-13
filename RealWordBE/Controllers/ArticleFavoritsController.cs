using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Entities;
using RealWord.DB.Models;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.RequestDtos.OuterDtos;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Models.ResponseDtos.OuterResponseDto;
using RealWord.DB.Repositories;
using RealWord.DB.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RealWordBE.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleFavoritsController:ControllerBase
    {

        private readonly IArticleService _articleService;
        private readonly ILikeService _likeService;


        public ArticleFavoritsController(IArticleService articleService ,ILikeService likeService)
        {

            _articleService = articleService;
            _likeService = likeService;
        }


        [Authorize]
        [HttpPost("{slug}/favorite")]
        public async Task<IActionResult> CreateLike(string slug)
        {
            bool validSlug = _articleService.IsValidSlug(slug);
            if( !validSlug ) return BadRequest(new Error()
            {
                Status = "400" ,
                Tittle = "Bad Request" ,
                ErrorMessage = "Invalid Slug "
            });
            var SrcId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var isLiked = await _likeService.CreateLikeAsync(slug ,SrcId);
            if( isLiked == Status.Duplicate )
                return BadRequest(new Error()
                {
                    Status = "400" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = $"Already Faviourte Article "
                });
            return RedirectToRoute("GetArticle" ,new { slug = slug });



        }
        [Authorize]
        [HttpDelete("{slug}/favorite")]
        public async Task<IActionResult> DeleteLike(string slug)
        {
            bool validSlug = _articleService.IsValidSlug(slug);
            if( !validSlug ) return BadRequest(new Error()
            {
                Status = "400" ,
                Tittle = "Bad Request" ,
                ErrorMessage = "Invalid Slug "
            });
            var SrcId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var isLiked = await _likeService.DeleteLikeAsync(slug ,SrcId);
            if( isLiked == Status.Duplicate )
                return BadRequest(new Error()
                {
                    Status = "400" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = $"Already UnFaviourte Article "
                });
            return NoContent();



        }


    }
}