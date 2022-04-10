using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using System.Collections.Generic;
using voting_models.Models;
using System;

namespace voting_bl.Service
{
    public class VotingUsersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VotingUsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public VotingUsersResponse GetUserBEmail(string email)
        {
            VotingUsersResponse usersData = _unitOfWork.VotingUsers.GetUserByEmail(email);
            return usersData;
        }
        public VotingUsers GetUsers(long id)
        {
            VotingUsers usersData = _unitOfWork.VotingUsers.Get(id);
            return usersData;
        }

        public List<VotingUsersResponse> GetUsers()
        {
            List<VotingUsersResponse> usersData = _unitOfWork.VotingUsers.GetUsers();
            return usersData;
        }

        public void SaveUsers(VotingUsers votingUsers)
        {
            _unitOfWork.VotingUsers.Add(votingUsers);
            _unitOfWork.Save();
        }

        public bool Authenticate(string email, string password)
        {
           return _unitOfWork.VotingUsers.AuthenticateUser(email, password);
        }
        public VotingRoles getRole(string username)
        {
            return _unitOfWork.VotingUsers.getRole(username) ;
        }

        public void EditUser(long id, VotingUsers user)
        {
            VotingUsers votingUser = _unitOfWork.VotingUsers.Get(id);
            votingUser.FirstName = user.FirstName;
            votingUser.LastName = user.LastName;
            votingUser.GroupId = user.GroupId;
            votingUser.Password = user.Password;
            votingUser.RoleId = user.RoleId;
            votingUser.IsMEP = user.IsMEP;
            _unitOfWork.VotingUsers.Update(votingUser);
            _unitOfWork.Save();
        }
    }
}
