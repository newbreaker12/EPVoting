using System.Collections.Generic;
using voting_data_access.Entities;

namespace voting_data_access.Repositories.Interfaces
{
    public interface IVotingArticleRepository : IRepository<VotingArticle>
    {
        VotingArticleResponse getArticle(long id);
        List<VotingArticleResponse> GetArticles();
        List<VotingArticleResponse> GetArticleForUser(string email);
    }
}
