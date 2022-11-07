using voting_bl.Util;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using voting_models.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace voting_bl.Service
{
    public class VotingGroupsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VotingGroupsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public VotingGroups GetGroups(long id)
        {
            VotingGroups groupsData = _unitOfWork.VotingGroups.Get(id);
            return groupsData;
        }
        public VotingGroups DeleteGroups(long id)
        {
            VotingGroups groupsData = _unitOfWork.VotingGroups.Get(id);
            groupsData.Disabled = true;
            _unitOfWork.VotingGroups.Update(groupsData);
            _unitOfWork.Save();
            return groupsData;
        }

        public List<VotingGroups> GetGroups()
        { 
            List<VotingGroups> groupsData = _unitOfWork.VotingGroups.GetGroups();
            return groupsData;
        }

        public void SaveGroups(VotingGroups groupsData)
        {
            _unitOfWork.VotingGroups.Add(groupsData);
            _unitOfWork.Save();
        }

        public VotingGroups UpdateGroups(VotingGroups groupUpdated)
        {
            VotingGroups groupsData = _unitOfWork.VotingGroups.Get(groupUpdated.Id);
            groupsData.Name = groupUpdated.Name;
            groupsData.ReadableId = groupUpdated.ReadableId;
            groupsData.Disabled = groupUpdated.Disabled;
            _unitOfWork.VotingGroups.Update(groupsData);
            _unitOfWork.Save();
            return groupsData;
        }
    }
}
