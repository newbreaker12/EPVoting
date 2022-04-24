using System.Collections.Generic;
using System.Linq;
using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;

namespace voting_data_access.Repositories.Implementation
{
    public class VoteSubmitRepository : Repository<VoteSubmit>, IVoteSubmitRepository
    {
        public VoteSubmitRepository(VotingDbContext context) : base(context) { }

        public VoteSubmit GetVoteSubmit(string email, long articleId)
        {
            return Db.VoteSubmit.SingleOrDefault(s => s.UserEmail == email && s.ArticleId == articleId);
        }

    }
}