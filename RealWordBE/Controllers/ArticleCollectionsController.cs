using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class ArticleCollectionsController:ControllerBase
    {
        private readonly ITokenManager _tokenManager;
        private readonly IArticleService _articleService;
        private readonly IProfileService _profileService;
        private readonly IFollowerRepository _followerRepository;
        private readonly ILogger<ArticleCollectionsController> _logger;

        public ArticleCollectionsController(ILogger<ArticleCollectionsController> logger ,ITokenManager tokenManager ,IArticleService articleService ,IProfileService profileService ,IFollowerRepository followerRepository)
        {
            _tokenManager = tokenManager;
            _articleService = articleService;
            _profileService = profileService;
            _followerRepository = followerRepository;
            _logger = logger;

        }



        [HttpGet]
        public async Task<ActionResult<ArticlesResponseOuterDto>> ListArticlesWithFilters
                ([FromQuery] string tag ,[FromQuery] string author ,[FromQuery] string favorited ,
                [FromQuery] int limit ,[FromQuery] int offset)
        {
            var token = _tokenManager.GetCurrentTokenAsync();
            string CurrentUserId = null; string CurrentUserName = null;
            if( token != string.Empty )
            {
                var tokens = _tokenManager.ExtractClaims(token);
                CurrentUserId = tokens.Claims.First(claim => claim.Type == "uid").Value;
                CurrentUserName = tokens.Claims.First(claim => claim.Type == "username").Value;
            }
            var articles = await _articleService.ListArticlesWithFilters(limit ,offset ,tag ,favorited ,author);
            if( articles == null ) return BadRequest(new Error() { ErrorMessage = "Invalid input" ,Status = "400" ,Tittle = "Bad Request" });
            List<ArticleResponseDto> articlResponseList = new List<ArticleResponseDto>();
            foreach( var article in articles )
            {
                var element = await _articleService.GetAricleResponseAsync(_profileService ,article ,CurrentUserId ,CurrentUserName);

                articlResponseList.Add(element);
            }
            //if sorted by tag , tag should appear first
            if( tag != null )
                foreach( var elemnt in articlResponseList )
                {
                    elemnt.TagList.Remove(tag);
                    elemnt.TagList.Insert(0 ,tag);
                }

            int count = articlResponseList.Count;
            var result = new ArticlesResponseOuterDto() { Articles = articlResponseList ,ArticlesCount = count };
            return Ok(result);

        }
        [HttpGet("feed")]
        public async Task<ActionResult<ArticlesResponseOuterDto>> FeedArticlesAsync([FromQuery] int limit ,[FromQuery] int offset)
        {
            var token = _tokenManager.GetCurrentTokenAsync();
            if( token == string.Empty ) return Unauthorized();
            if( !_tokenManager.ValidateToken(token) ) return Unauthorized();

            var tokens = _tokenManager.ExtractClaims(token);
            var CurrentUserId = tokens.Claims.First(claim => claim.Type == "uid").Value;
            var CurrentUserName = tokens.Claims.First(claim => claim.Type == "username").Value;


            var articles = _articleService.FeedArticles(_followerRepository ,CurrentUserId ,limit ,offset);
            if( articles == null ) return Ok("No followed users");
            List<ArticleResponseDto> articlResponseList = new List<ArticleResponseDto>();
            foreach( var article in articles )
            {
                var element = await _articleService.GetAricleResponseAsync(_profileService ,article ,CurrentUserId ,CurrentUserName);
                articlResponseList.Add(element);
            }
            int count = articlResponseList.Count;
            var result = new ArticlesResponseOuterDto() { Articles = articlResponseList ,ArticlesCount = count };
            return Ok(result);

        }

    }
}