using System.Collections.Generic;
using System.Linq;
using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;

namespace voting_data_access.Repositories.Implementation
{
    public class VoteRepository : Repository<Vote>, IVoteRepository
    {
        public VoteRepository(VotingDbContext context) : base(context) { }

        public Vote GetVoteForUser(string email, long subArticleId)
        {
            return Db.Vote.SingleOrDefault(v => v.SubArticleId == subArticleId && v.UserEmail == email);
        }

        public List<Vote> GetVote()
        {
            return Db.Vote.ToList();
        }
    }
}