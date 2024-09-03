using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using voting_models.Models;

namespace voting_bl.Service
{
    public class VotingUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly byte[] _salt;

        public VotingUsersService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            var token = _configuration["AppSettings:Token"];
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is not configured in appsettings.json");
            }
            _salt = Encoding.ASCII.GetBytes(token);
        }

        public List<VotingUsers> GetUsersByGroup(long groupId)
        {
            List<VotingUsers> usersData = _unitOfWork.VotingUsers.GetByGroupId(groupId);
            return usersData;
        }
        public VotingUsersResponse GetUserByEmail(string email)
        {
            VotingUsersResponse usersData = _unitOfWork.VotingUsers.GetUserByEmail(email);
            return usersData;
        }
        public VotingUsers GetUserDataByEmail(string email)
        {
            return _unitOfWork.VotingUsers.GetUserDataByEmail(email);
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

        public void AddUsers(VotingUsers votingUsers)
        {
            VotingUsersResponse usersData = _unitOfWork.VotingUsers.GetUserByEmail(votingUsers.Email);
            if (usersData != null)
            {
                throw new InvalidUsersEmail("Email already taken");
            }

            votingUsers.Password = hashPassword(votingUsers.Password);

            _unitOfWork.VotingUsers.Add(votingUsers);
            _unitOfWork.Save();
        }
        public string hashPassword(string password)
        {
            using (var hmac = new HMACSHA512(_salt))
            {
                return Encoding.Unicode.GetString(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
        public string hashPincode(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
            }
        }

        public void SaveUsers(VotingUsers votingUsers)
        {
            _unitOfWork.VotingUsers.Update(votingUsers);
            _unitOfWork.Save();
        }

        public bool Authenticate(string email, string password)
        {
            return _unitOfWork.VotingUsers.AuthenticateUser(email, password);
        }
        public VotingRoles getRole(string username)
        {
            return _unitOfWork.VotingUsers.getRole(username);
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

        public VotingUsers DeleteUsers(long id)
        {
            VotingUsers user = _unitOfWork.VotingUsers.Get(id);
            _unitOfWork.VotingUsers.Remove(user);
            _unitOfWork.Save();
            return user;
        }

        public string updateAndGetPincode(VotingUsers user)
        {
            string pincode = generatePinCode();
            user.PinCode = hashPincode(pincode);
            _unitOfWork.VotingUsers.Update(user);
            _unitOfWork.Save();
            return pincode;
        }

        private string generatePinCode()
        {
            Random random = new Random();
            int randomNumber = random.Next(1000, 10000);
            return randomNumber.ToString();
        }
    }
}
