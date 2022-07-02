using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Models.ResponseDtos.OuterResponseDto
{
    public class CommentsResponseOuterDto
    {
        public IEnumerable<CommentResponseDto> Comments { get; set; }
    }
}
