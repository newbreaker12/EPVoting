using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace voting_data_access.Repositories.Implementation
{
    public class VotingSessionRepository : Repository<VotingSession>, IVotingSessionRepository
    {
        public VotingSessionRepository(VotingDbContext context) : base(context) { }

        public List<VotingSession> GetSession()
        {
            return Db.VotingSession.ToList();
        }

        public VotingSession GetActiveSessionByArticleId(long articleId)
        {
            DateTime now = DateTime.Now;
            return Db.VotingSession.SingleOrDefault(s => s.ArticleId == articleId && s.To > now);
        }
    }
}