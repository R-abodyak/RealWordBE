using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Services
{
    public interface ILikeService
    {
        Task<Status> CreateLikeAsync(string slug ,string UserId);
        Task<Status> DeleteLikeAsync(string slug ,string UserId);
    }
}
