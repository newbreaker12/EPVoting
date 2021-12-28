using voting_bl.Util;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using voting_models.Models;
using System.Collections.Generic;
using System.Linq;


namespace voting_bl.Service
{
    public class VotingRolesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VotingRolesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public VotingRoles GetRoles(long id)
        {
            VotingRoles rolesData = _unitOfWork.VotingRoles.Get(id);
            return rolesData;
        }

        public List<VotingRoles> GetRoles()
        {
            List<VotingRoles> rolesData = _unitOfWork.VotingRoles.GetRoles();
            return rolesData;
        }

        public void SaveRoles(VotingRoles votingRoles)
        {
            _unitOfWork.VotingRoles.Add(votingRoles);
            _unitOfWork.Save();
        }
    }
}
