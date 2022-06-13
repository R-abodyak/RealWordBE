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
    public class ArticleCollectionsController:ControllerBase
    {

        private readonly IArticleService _articleService;
        private readonly IProfileService _profileService;
        private readonly IFollowerRepository _followerRepository;

        public ArticleCollectionsController(IArticleService articleService ,IProfileService profileService ,IFollowerRepository followerRepository)
        {

            _articleService = articleService;
            _profileService = profileService;
            _followerRepository = followerRepository;
        }



        [HttpGet]
        public async Task<ActionResult<ArticlesResponseOuterDto>> ListArticlesWithFilters
                ([FromQuery] string tag ,[FromQuery] string author ,[FromQuery] string favorited ,
                [FromQuery] int limit ,[FromQuery] int offset)
        {
            var CurrentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var CurrentUserName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            var articles = await _articleService.ListArticlesWithFilters(limit ,offset ,tag ,favorited ,author);
            if( articles == null ) return BadRequest(new Error() { ErrorMessage = "Invalid input" ,Status = "400" ,Tittle = "Bad Request" });
            List<ArticleResponseDto> articlResponseList = new List<ArticleResponseDto>();
            foreach( var article in articles )
            {
                var element = await _articleService.GetAricleResponseAsync(_profileService ,article.Slug ,CurrentUserId ,CurrentUserName);
                articlResponseList.Add(element);
            }
            var result = new ArticlesResponseOuterDto() { Articles = articlResponseList };
            return Ok(result);

        }
        [Authorize]
        [HttpGet("feed")]
        public async Task<ActionResult<ArticlesResponseOuterDto>> FeedArticlesAsync([FromQuery] int limit ,[FromQuery] int offset)
        {
            var CurrentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var CurrentUserName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

            var articles = _articleService.FeedArticles(_followerRepository ,CurrentUserId ,limit ,offset);
            if( articles == null ) return Ok("No followed users");
            List<ArticleResponseDto> articlResponseList = new List<ArticleResponseDto>();
            foreach( var article in articles )
            {
                var element = await _articleService.GetAricleResponseAsync(_profileService ,article.Slug ,CurrentUserId ,CurrentUserName);
                articlResponseList.Add(element);
            }
            var result = new ArticlesResponseOuterDto() { Articles = articlResponseList };
            return Ok(result);

        }

    }
}