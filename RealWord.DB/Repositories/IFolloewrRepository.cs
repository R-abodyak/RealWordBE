using System;
using System.Collections.Generic;
using System.Text;

namespace RealWord.DB.Repositories
{
    public interface IFollowerRepository
    {
        bool IsFollowing(string SrcId ,string DstId);
    }
}
