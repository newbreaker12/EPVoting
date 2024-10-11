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
            return _unitOfWork.VotingGroups.Get(id);
        }

        public VotingGroups DeleteGroups(long id)
        {
            var groupsData = _unitOfWork.VotingGroups.Get(id);
            groupsData.Disabled = true;
            _unitOfWork.VotingGroups.Update(groupsData);
            _unitOfWork.Save();
            return groupsData;
        }

        public List<VotingGroups> GetGroups()
        {
            return _unitOfWork.VotingGroups.GetGroups();
        }

        public void SaveGroups(VotingGroups groupsData)
        {
            DateTime now = DateTime.Now;
            groupsData.CreatedAt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second).ToUniversalTime();
            _unitOfWork.VotingGroups.Add(groupsData);
            _unitOfWork.Save();
        }

        public VotingGroups UpdateGroups(VotingGroups groupUpdated)
        {
            var groupsData = _unitOfWork.VotingGroups.Get(groupUpdated.Id);
            groupsData.Name = groupUpdated.Name;
            groupsData.ReadableId = groupUpdated.ReadableId;
            groupsData.Disabled = groupUpdated.Disabled;
            _unitOfWork.VotingGroups.Update(groupsData);
            _unitOfWork.Save();
            return groupsData;
        }
    }
}
