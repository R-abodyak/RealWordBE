﻿using RealWord.DB.Entities;
using RealWord.DB.Models.RequestDtos;
using RealWord.DB.Models.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Services
{
    public interface ICommentService
    {
        Task<int> CreateComment(string currentUserId ,string slug ,CommentDto commentDto);
        Task<CommentResponseDto> GetCommentResponse(int commentId ,string slug ,string CurrentUserName);
    }
}
