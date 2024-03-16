using System.Collections.Generic;
using voting_data_access.Entities;

namespace voting_data_access.Repositories.Interfaces
{
    public interface IVotingSubArticleRepository : IRepository<VotingSubArticle>
    {
        List<VotingSubArticle> GetAll();
        List<VotingSubArticle> GetSubArticlesByArticleId(long articleId);

        List<VotingSubArticleResponse> GetSubArticlesByArticleIdAndEmail(long articleId, string email);
    }
}
