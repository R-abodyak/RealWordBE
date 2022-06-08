using RealWord.DB.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealWord.DB.Repositories
{
    public interface IFollowerRepository
    {
        bool IsFollowing(string SrcId ,string DstId);
        //Task CreateFollow(string SrcId ,string DstId);
        Task<Folower> CreateFollow(string SrcId ,string DstId);
        void RemoveFollow(string SrcId ,string DstId);
        Task SaveChangesAsync();
    }
}
