using System.Collections.Generic;
using voting_data_access.Entities;

namespace voting_data_access.Repositories.Interfaces
{
    public interface IVoteSubmitRepository : IRepository<VoteSubmit>
    {
        VoteSubmit GetVoteSubmit(string email, long articleId);
    }
}
