using System.Collections.Generic;
using voting_data_access.Entities;

namespace voting_data_access.Repositories.Interfaces
{
    public interface IVoteRepository : IRepository<Vote>
    {
        Vote GetVoteForUser(string email, long subArticleId);
        List<Vote> GetVote();
        public int GetVoteCountByTypeAndSubArticle(int type, long subArticleId);
    }
}
