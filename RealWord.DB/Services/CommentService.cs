using AutoMapper;
using RealWord.DB;
using RealWord.DB.Entities;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.ResponseDtos;
using RealWord.DB.Repositories;
using RealWord.DB.Services;
using RealWordBE.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Services
{



    public class CommentService:BaseRepository, ICommentService
    {
        private readonly IArticleRebository _articleRepository;
        private readonly IProfileService _profileService;
        private readonly ICommentRebository _commentRebository;
        private readonly IUserRepository _userRebository;
        private readonly IMapper _mapper;
        public CommentService(ApplicationDbContext applicationDbContext ,IMapper mapper ,IArticleRebository articleRebository ,IProfileService profileService ,ICommentRebository commentRebository ,IUserRepository userRebository) : base(applicationDbContext)
        {
            _articleRepository = articleRebository;
            _profileService = profileService;
            _commentRebository = commentRebository;
            _userRebository = userRebository;
            _mapper = mapper;
        }
        public async Task<int> CreateComment(string currentUserName ,string slug ,CommentDto commentDto)
        {
            var comment = new Comment();
            var user = await _userRebository.GetUserByUsernameAsync(currentUserName);
            comment.User = user;
            comment.CommentMsg = commentDto.CommentMsg;
            var article = _articleRepository.GetArticleBySlug(slug);
            if( article == null ) return -1;
            comment.Article = article;
            await _commentRebository.AddComment(comment);

            await SaveChangesAsync();

            return comment.CommentId;


        }
        public async Task<CommentResponseDto> GetCommentResponse(int id ,string slug ,string CurrentUserName)
        {
            var comment = await _commentRebository.GetComment(id);
            await SaveChangesAsync();
            return await GenerateCommentRespnseDto(id ,slug ,CurrentUserName ,comment);
        }



        public async Task<IEnumerable<CommentResponseDto>> GetComments(string slug ,string CurrentUserName)
        {
            var comments = _commentRebository.GetComments(slug);
            if( comments == null ) return null;
            IList<CommentResponseDto> responses = new List<CommentResponseDto>();
            foreach( var comment in comments )
            {
                var response = await GenerateCommentRespnseDto(comment.CommentId ,slug ,CurrentUserName ,comment);
                responses.Add(response);
            }
            return responses;


        }
        private async Task<CommentResponseDto> GenerateCommentRespnseDto(int id ,string slug ,string CurrentUserName ,Comment comment)
        {
            var commentResponse = _mapper.Map<CommentResponseDto>(comment);
            commentResponse.UpdatedDate = await _commentRebository.GetUpdatedDateAsync(id);
            commentResponse.CreatedDate = await _commentRebository.GetCreatedDate(id);
            var author = _articleRepository.GetAuthorofArticle(slug);

            commentResponse.Author =
            await _profileService.GetProfileAsync(CurrentUserName ,author.UserName);
            return commentResponse;
        }

        public async Task RemoveComment(int id)
        {
            var comment = await _commentRebository.GetComment(id);
            if( comment == null ) return;
            _commentRebository.RemoveComment(comment);
            await SaveChangesAsync();

        }

        public async Task<bool> DoesUserMatchAuthorAsync(int commentId ,string userId)
        {
            var comment = await _commentRebository.GetComment(commentId);
            if( comment == null ) return false;
            if( userId == comment.User_id ) return true;

            return false;
        }

        public async Task<Comment> GetCommentAsync(int id)
        {
            return await _commentRebository.GetComment(id);

        }

    }
}