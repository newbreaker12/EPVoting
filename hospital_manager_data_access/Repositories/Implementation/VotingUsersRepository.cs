using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using voting_models.Models;
using Microsoft.Data.SqlClient;
using System;

namespace voting_data_access.Repositories.Implementation
{
    public class VotingUsersRepository : Repository<VotingUsers>, IVotingUsersRepository
    {
        public VotingUsersRepository(VotingDbContext context) : base(context) { }

        public List<VotingUsers> GetByGroupId(long groupId)
        {
            return Db.VotingUsers.Where(u => u.GroupId == groupId).ToList();
        }

        public VotingUsers GetUserDataByEmail(string email)
        {
            return Db.VotingUsers.SingleOrDefault(u => u.Email == email);
        }
        public VotingUsersResponse GetUserByEmail(string email)
        {
            VotingUsers vu = Db.VotingUsers.SingleOrDefault(u => u.Email == email);
            if (vu == null)
            {
                return null;
            }
            VotingRoles vr = Db.VotingRoles.SingleOrDefault(r => r.Id == vu.RoleId);
            VotingGroups vg = Db.VotingGroups.SingleOrDefault(r => r.Id == vu.GroupId);
            VotingUsersResponse vur = new VotingUsersResponse()
            {
                Id = vu.Id,
                Email = vu.Email,
                FirstName = vu.FirstName,
                LastName = vu.LastName,
                Password = vu.Password,
                IsMEP = vu.IsMEP
            };
            if (vg != null)
            {

                vur.Groups = new VotingGroupsResponse()
                {
                    Id = vg.Id,
                    Name = vg.Name,
                    CreatedAt = vg.CreatedAt,
                    ReadableId = vg.ReadableId
                };
            }
            if (vr != null)
            {
                vur.Role = new VotingRolesResponse()
                {
                    Id = vr.Id,
                    Name = vr.Name,
                    Description = vr.Description,
                };
            }
            return vur;
        }

        public List<VotingUsersResponse> GetUsers()
        {
            var users = Db.VotingUsers.Where(i => i.Disabled == false).ToList();
            List<VotingUsersResponse> result = new List<VotingUsersResponse>();
            foreach (VotingUsers vu in users)
            {
                VotingRoles vr = Db.VotingRoles.SingleOrDefault(r => r.Id == vu.RoleId);
                VotingGroups vg = Db.VotingGroups.SingleOrDefault(r => r.Id == vu.GroupId);
                VotingUsersResponse vur = new VotingUsersResponse()
                {
                    Id = vu.Id,
                    Email = vu.Email,
                    FirstName = vu.FirstName,
                    LastName = vu.LastName,
                    Password = vu.Password,
                    IsMEP = vu.IsMEP
                };
                if (vg != null)
                {

                    vur.Groups = new VotingGroupsResponse()
                    {
                        Id = vg.Id,
                        Name = vg.Name,
                        CreatedAt = vg.CreatedAt,
                        ReadableId = vg.ReadableId
                    };
                }
                if (vr != null)
                {

                    vur.Role = new VotingRolesResponse()
                    {
                        Id = vr.Id,
                        Name = vr.Name,
                        Description = vr.Description,
                    };
                }
                result.Add(vur);
            }
            return result;
        }

        public bool AuthenticateUser(string email, string password)
        {
            try
            {
                // Log the input parameters for debugging
                Console.WriteLine($"AuthenticateUser called with Email: {email}, Password: {password}");

                // Ensure correct column names and use the combined Where method
                var user = Db.VotingUsers
                             .Where(i => !i.Disabled && i.Email == email && i.Password == password)
                             .SingleOrDefault();

                // Log the generated SQL query
                var sql = Db.VotingUsers.Where(i => !i.Disabled && i.Email == email && i.Password == password).ToQueryString();
                Console.WriteLine($"Generated SQL: {sql}");

                return user != null;
            }
            catch (SqlException ex)
            {
                // Log the exception details
                Console.WriteLine($"SQL Exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }
        public VotingRoles getRole(string email)
        {
            VotingUsers vu = Db.VotingUsers.SingleOrDefault(u => u.Email == email);
            if (vu == null)
            {
                return null;
            }
            return Db.VotingRoles.SingleOrDefault(r => r.Id == vu.RoleId);
        }
    }
}