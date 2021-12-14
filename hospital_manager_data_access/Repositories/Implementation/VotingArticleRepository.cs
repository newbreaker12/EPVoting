using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;

namespace voting_data_access.Repositories.Implementation
{
    public class VotingArticleRepository : Repository<VotingArticle>, IVotingArticleRepository
    {
        public VotingArticleRepository(VotingDbContext context) : base(context) { }
    }
}