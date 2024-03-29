﻿using voting_data_access.Entities;
using System.Collections.Generic;

namespace voting_data_access.Repositories.Interfaces
{
    public interface IVotingRolesRepository : IRepository<VotingRoles>
    {
        List<VotingRoles> GetRoles();
    }
}
