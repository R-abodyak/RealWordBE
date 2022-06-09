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
        public ArticleTagService _articleTagService { get; set; }

        public ArticleController(IArticleRebository articleRepository ,IMapper mapper ,ArticleTagService articleTagService)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
            _articleTagService = articleTagService;
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateArticle(ArticleOuterDto articleOuterDto)
        {
            var articleDto = articleOuterDto.ArticleDto;
            var article = _mapper.Map<Article>(articleDto);
            List<Tag> tags = new List<Tag>();
            foreach( string tagName in articleDto.Tags )
            {
                tags.Add(new Tag() { Name = tagName });

            }
            var UserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            article.UserId = UserId;

            await _articleTagService.CreateArticleWithTag(article ,tags);


            var FinalArticle = _articleRepository.GetArticleByTitle(article.Title);
            var result = _mapper.Map<ArticleResponseDto>(FinalArticle);
            return Ok(result);

        }
    }
}
