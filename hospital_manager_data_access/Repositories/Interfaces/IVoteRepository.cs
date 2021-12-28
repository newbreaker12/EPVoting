﻿using System.Collections.Generic;
using voting_data_access.Entities;

namespace voting_data_access.Repositories.Interfaces
{
    public interface IVoteRepository : IRepository<Vote>
    {
        List<Vote> GetVote();
    }
}
