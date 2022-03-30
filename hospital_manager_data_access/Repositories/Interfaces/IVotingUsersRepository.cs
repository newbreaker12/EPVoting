using voting_data_access.Entities;
using System.Collections.Generic;
using voting_models.Models;

namespace voting_data_access.Repositories.Interfaces
{
    public interface IVotingUsersRepository : IRepository<VotingUsers>
    {
        VotingUsersResponse GetUserByEmail(string email);
        List<VotingUsersResponse> GetUsers();
        bool AuthenticateUser(string email, string password);
        List<VotingRoles> getRoles(string username);
    }

}
