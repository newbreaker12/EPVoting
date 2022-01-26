using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace voting_data_access.Repositories.Implementation
{
    public class VotingUsersRepository : Repository<VotingUsers>, IVotingUsersRepository
    {
        public VotingUsersRepository(VotingDbContext context) : base(context) { }

        public List<VotingUsers> GetUsers()
        {
            return Db.VotingUsers.Include(u => u.Roles).Include(u => u.Groups).ToList();
        }

        public bool AuthenticateUser(string email, string password)
        {
            var user = Db.VotingUsers.SingleOrDefault(u => u.Email == email && u.Password == password);
            return user != null;
        }
        public List<VotingRoles> getRoles(string username)
        {
            List<VotingRoles> result = new List<VotingRoles>();
            List<UserToRole> roleIds = Db.VotingUsers.Include(u => u.Roles).Single(u => u.Email == username).Roles.ToList();
            foreach (UserToRole ri in roleIds)
            {
                result.Add(Db.VotingRoles.Single(r => r.Id == ri.Id));
            }
            return result;
        }
    }
}