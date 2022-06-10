using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Entities;
using RealWord.DB.Models;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.RequestDtos.OuterDtos;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Repositories;
using RealWord.DB.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RealWordBE.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleController:ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ArticleService _articleService;
        private readonly ProfileService _profileService;

        public ArticleController(IMapper mapper ,ArticleService articleService ,ProfileService profileService)
        {

            _mapper = mapper;
            _articleService = articleService;
            _profileService = profileService;
        }

        [HttpGet("{slug}" ,Name = "GetArticle")]
        public async Task<ActionResult<ArticleResponseDto>> GetArticleAsync(string slug)
        {
            var CurrentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            var articleResponseDto = _articleService.GetAricleResponse(slug ,CurrentUserId);
            if( articleResponseDto == null ) return BadRequest(new Error()
            { ErrorMessage = "Invalid Slug" ,Status = "400" ,Tittle = "BadRequest" });
            var profile = new ProfileResponseDto();
            profile.UserName = articleResponseDto.Author.UserName;

            //author of article 
            var SrcId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            profile = await _profileService.GetProfileAsync(SrcId ,profile.UserName);

            if( profile == null )
            {
                return BadRequest(
                      new Error()
                      {
                          Status = "404" ,
                          Tittle = "Bad Request" ,
                          ErrorMessage = "Invalid User Name "
                      });
            }
            articleResponseDto.Author = profile;
            return Ok(articleResponseDto);

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ArticleResponseDto>> CreateArticle(ArticleOuterDto articleOuterDto)
        {
            var articleDto = articleOuterDto.ArticleDto;
            var article = _mapper.Map<Article>(articleDto);
            article.Slug = article.Title.Replace(" " ,"_");
            List<Tag> tags = new List<Tag>();
            foreach( string tagName in articleDto.Tags )
            {
                tags.Add(new Tag() { Name = tagName });

            }
            var UserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            article.UserId = UserId;

            await _articleService.CreateArticleWithTag(article ,tags);

            return RedirectToRoute("GetArticle" ,new { slug = article.Slug });




        }
        [Authorize]
        [HttpPut("{slug}")]
        public async Task<IActionResult> UpdateArticleAsync(ArticleForUpdateOuterDto articleOuterDto ,string slug)
        {
            var articlForUpdate = articleOuterDto.articleForUpdateDto;

            await _articleService.UpdateArticle(slug ,articlForUpdate);
            return RedirectToRoute("GetArticle" ,new { slug = slug });

        }
    }
}
