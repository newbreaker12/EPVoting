using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace voting_data_access.Repositories.Implementation
{
    public class VotingGroupsRepository : Repository<VotingGroups>, IVotingGroupsRepository
    {
        public VotingGroupsRepository(VotingDbContext context) : base(context) { }

        public List<VotingGroups> GetGroups()
        {
            return Db.VotingGroups.ToList();
        }
    }
}