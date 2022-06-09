using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Entities;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.RequestDtos.OuterDtos;
using RealWord.DB.Repositories;
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
        public ArticleController(IArticleRebository articleRepository ,IMapper mapper)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
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
            var AuthorId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            await _articleRepository.CreateArticle(article ,tags);
            var result = _articleRepository.SaveChangesAsync();
            return Ok();

        }
    }
}
