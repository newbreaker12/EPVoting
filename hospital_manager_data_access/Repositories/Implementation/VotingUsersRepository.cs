using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace voting_data_access.Repositories.Implementation
{
    public class VotingUsersRepository : Repository<VotingUsers>, IVotingUsersRepository
    {
        public VotingUsersRepository(VotingDbContext context) : base(context) { }
    }
}