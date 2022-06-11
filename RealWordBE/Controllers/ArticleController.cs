﻿using AutoMapper;
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
        private readonly IArticleService _articleService;
        private readonly IProfileService _profileService;

        public ArticleController(IMapper mapper ,IArticleService articleService ,IProfileService profileService)
        {

            _mapper = mapper;
            _articleService = articleService;
            _profileService = profileService;
        }

        [HttpGet("{slug}" ,Name = "GetArticle")]
        public async Task<ActionResult<ArticleResponseDto>> GetArticleAsync(string slug)
        {
            var validSlug = _articleService.IsValidSlug(slug);
            if( !validSlug )
                return BadRequest(new Error()
                {
                    Status = "400" ,
                    Tittle = "Bad Request" ,
                    ErrorMessage = "Invalid Slug "
                });

            var CurrentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var CurrentUserName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

            var articleResponseDto = _articleService.GetAricleResponse(slug ,CurrentUserId);
            if( articleResponseDto == null )
                return BadRequest(new Error()
                { ErrorMessage = "Invalid Slug" ,Status = "400" ,Tittle = "BadRequest" });
            var profile = new ProfileResponseDto();
            profile.UserName = articleResponseDto.Author.UserName;

            //author of article 

            profile = await _profileService.GetProfileAsync(CurrentUserName ,profile.UserName);

            if( profile == null )
            {
                return BadRequest(
                      new Error()
                      {
                          Status = "400" ,
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
            bool validSlug = _articleService.IsValidSlug(slug);
            if( !validSlug ) return BadRequest(new Error()
            {
                Status = "400" ,
                Tittle = "Bad Request" ,
                ErrorMessage = "Invalid Slug "
            });
            var articlForUpdate = articleOuterDto.articleForUpdateDto;
            var SrcId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            var permission = _articleService.HasPermissionToUpdate(slug ,SrcId);
            if( permission == false ) return StatusCode(403 ,"Permission Denied for updating Articles belong to other Authors");
            await _articleService.UpdateArticle(slug ,articlForUpdate);
            if( articlForUpdate.Title != null ) slug = articlForUpdate.Title.Replace(" " ,"_");

            return RedirectToRoute("GetArticle" ,new { slug = slug });

        }
    }
}