using voting_data_access.Entities;
using System.Collections.Generic;
using voting_models.Models;

namespace voting_data_access.Repositories.Interfaces
{
    public interface IVotingUsersRepository : IRepository<VotingUsers>
    {
        List<VotingUsers> GetByGroupId(long groupId);
        VotingUsersResponse GetUserByEmail(string email);
        VotingUsers GetUserDataByEmail(string email);
        List<VotingUsersResponse> GetUsers();
        bool AuthenticateUser(string email, string password);
        VotingRoles getRole(string username);
    }

}
