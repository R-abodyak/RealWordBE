using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Repositories;

namespace RealWordBE.Controllers
{
    [Route("api")]
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
    }
}
