using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;

namespace voting_bl.Service
{
    public class StatisticsService
    {
        private readonly VotingDbContext _context;

        public StatisticsService(VotingDbContext context)
        {
            _context = context;
        }

        public async Task<List<VoteStatistics>> GetVoteStatisticsAsync()
        {
            return await _context.GetVoteStatisticsAsync();
        }
    }
}
