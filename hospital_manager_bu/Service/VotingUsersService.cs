using voting_bl.Util;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using voting_models.Models;
using System.Collections.Generic;

namespace voting_bl.Service
{
    public class VotingUsersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VotingUsersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public VotingUsers GetUsers(long id)
        {
            VotingUsers usersData = _unitOfWork.VotingUsers.Get(id);
            return usersData;
        }

        public List<VotingUsers> GetUsers()
        {
            List<VotingUsers> usersData = _unitOfWork.VotingUsers.GetUsers();
            return usersData;
        }

        public void SaveUsers(VotingUsers votingUsers)
        {
            _unitOfWork.VotingUsers.Add(votingUsers);
            _unitOfWork.Save();
        }

    }
}
