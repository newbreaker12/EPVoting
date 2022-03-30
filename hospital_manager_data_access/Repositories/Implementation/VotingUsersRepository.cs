using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using voting_models.Models;

namespace voting_data_access.Repositories.Implementation
{
    public class VotingUsersRepository : Repository<VotingUsers>, IVotingUsersRepository
    {
        public VotingUsersRepository(VotingDbContext context) : base(context) { }

        public VotingUsersResponse GetUserByEmail(string email)
        {
            List<VotingRoles> vr = Db.VotingRoles.ToList();
            VotingUsers vu = Db.VotingUsers.Include(u => u.Roles).Include(u => u.Groups).SingleOrDefault(u => u.Email == email);
            return new VotingUsersResponse()
            {
                Id = vu.Id,
                Email = vu.Email,
                FirstName = vu.FirstName,
                LastName = vu.LastName,
                Password = vu.Password,
                IsMEP = vu.IsMEP,
                Roles = vr.Where(r => vu.Roles.SingleOrDefault(rr => rr.Id == r.Id) != null)?.Select(vrr => new VotingRolesResponse()
                {
                    Id = vrr.Id,
                    Name = vrr.Name,
                    Description = vrr.Description,
                }).ToList(),
                Groups = vu.Groups?.Select(vrr => new UserToGroupResponse()
                {
                    Id = vrr.Id,
                    GroupId = vrr.GroupId,
                }).ToList(),
            };
        }

        public List<VotingUsersResponse> GetUsers()
        {
            List<VotingRoles> vr = Db.VotingRoles.ToList();
            var users = Db.VotingUsers.Include(u => u.Roles).Include(u => u.Groups).ToList(); ;
            List <VotingUsersResponse> result = new List<VotingUsersResponse>();
            foreach (VotingUsers vu in users)
            {
                result.Add(new VotingUsersResponse()
                {
                    Id = vu.Id,
                    Email = vu.Email,
                    FirstName = vu.FirstName,
                    LastName = vu.LastName,
                    Password = vu.Password,
                    IsMEP = vu.IsMEP,
                    Roles = vr.Where(r => vu.Roles.SingleOrDefault(rr => rr.Id == r.Id) != null)?.Select(vrr => new VotingRolesResponse()
                    {
                        Id = vrr.Id,
                        Name = vrr.Name,
                        Description = vrr.Description,
                    }).ToList(),
                    Groups = vu.Groups?.Select(vrr => new UserToGroupResponse()
                    {
                        Id = vrr.Id,
                        GroupId = vrr.GroupId,
                    }).ToList(),

                });
            }
            return result;
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