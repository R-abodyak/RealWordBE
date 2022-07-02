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
using RealWordBE.Authentication.Logout;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RealWordBE.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleFavoritsController:ControllerBase
    {
        private readonly ITokenManager _tokenManager;
        private readonly IProfileService _profileService;
        private readonly IArticleService _articleService;
        private readonly ILikeService _likeService;


        public ArticleFavoritsController(ITokenManager tokenManager ,IProfileService profileService ,IArticleService articleService ,ILikeService likeService)
        {
            _tokenManager = tokenManager;
            _profileService = profileService;
            _articleService = articleService;
            _likeService = likeService;
        }


        // [Authorize]
        [HttpPost("{slug}/favorite")]
        public async Task<IActionResult> CreateLike(string slug)
        {
            var token = _tokenManager.GetCurrentTokenAsync();
            if( token == string.Empty ) return Unauthorized();
            if( !_tokenManager.ValidateToken(token) ) return Unauthorized();
            var tokens = _tokenManager.ExtractClaims(token);

            var article = _articleService.GetArticle(slug);
            if( article == null ) return BadRequest(new Error()
            {
                Status = "400" ,
                Tittle = "Bad Request" ,
                ErrorMessage = "Invalid Slug "
            });
            var SrcId = tokens.Claims.First(claim => claim.Type == "uid").Value;
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
        //[Authorize]
        [HttpDelete("{slug}/favorite")]
        public async Task<IActionResult> DeleteLike(string slug)
        {
            var token = _tokenManager.GetCurrentTokenAsync();
            if( token == string.Empty ) return Unauthorized();
            if( !_tokenManager.ValidateToken(token) ) return Unauthorized();

            var tokens = _tokenManager.ExtractClaims(token);
            var article = _articleService.GetArticle(slug);
            if( article == null ) return BadRequest(new Error()
            {
                Status = "400" ,
                Tittle = "Bad Request" ,
                ErrorMessage = "Invalid Slug "
            });
            var SrcId = tokens.Claims.First(claim => claim.Type == "uid").Value;
            var SrcName = tokens.Claims.First(claim => claim.Type == "uid").Value;
            var isLiked = await _likeService.DeleteLikeAsync(slug ,SrcId);
            if( isLiked == Status.Duplicate )
                return BadRequest(new Error()
                {
                    Status = "400" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = $"Already UnFaviourte Article "
                });

            var articleResponseDto = await _articleService.GetAricleResponseAsync(_profileService ,article ,SrcId ,SrcName);
            if( articleResponseDto == null )
                return BadRequest(new Error()
                { ErrorMessage = "Invalid Slug" ,Status = "400" ,Tittle = "BadRequest" });


            var response = new ArticleResponseOuterDto() { Article = articleResponseDto };
            return Ok(response);




        }


    }
}