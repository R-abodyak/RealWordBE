using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Entities;
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
        private readonly IArticleRebository _articleRepository;
        private readonly IMapper _mapper;
        public ArticleService _articleService { get; set; }

        public ArticleController(IMapper mapper ,ArticleService articleService)
        {

            _mapper = mapper;
            _articleService = articleService;
        }

        [HttpGet("{slug}" ,Name = "GetArticle")]
        public IActionResult GetArticle(string slug)
        {
            var CurrentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var CurrentUsername = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            var articleResponseDto = _articleService.GetAricleResponse(slug ,CurrentUserId);
            var profile = new ProfileResponseDto();
            profile.UserName = CurrentUsername;
            CreatedAtRoute("Profile" ,new { username = CurrentUsername } ,profile);
            articleResponseDto.Author = profile;
            return Ok(articleResponseDto);


        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateArticle(ArticleOuterDto articleOuterDto)
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
            var articleResponseDto = _mapper.Map<ArticleResponseDto>(article);

            CreatedAtRoute("GetArticle" ,new { slug = articleResponseDto.Slug } ,articleResponseDto);
            return Ok(articleResponseDto);


        }
    }
}
