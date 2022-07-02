using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Models.ResponseDtos.OuterResponseDto;
using RealWord.DB.Repositories;
using RealWord.DB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RealWordBE.Controllers
{
    [Route("api/tags")]

    [ApiController]
    public class TagController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public TagController(IMapper mapper ,ITagService tagService)
        {

            _mapper = mapper;
            _tagService = tagService;

        }
        [HttpGet()]

        public async Task<ActionResult<TagOuter>> GetTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            var tagsRespnse = _mapper.Map<List<TagResponseDto>>(tags);
            List<string> TagsName = new List<string>();
            foreach( var tag in tagsRespnse )
            {
                TagsName.Add(tag.Name);
            }
            TagOuter tagOuter = new TagOuter() { Tags = TagsName };
            return tagOuter;


        }
    }
}

