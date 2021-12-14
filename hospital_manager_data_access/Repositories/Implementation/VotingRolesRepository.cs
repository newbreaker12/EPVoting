using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace voting_data_access.Repositories.Implementation
{
    public class VotingRolesRepository : Repository<VotingRoles>, IVotingRolesRepository
    {
        public VotingRolesRepository(VotingDbContext context) : base(context) { }

    }
}