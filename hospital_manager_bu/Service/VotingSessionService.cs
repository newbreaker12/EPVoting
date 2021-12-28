﻿using voting_bl.Util;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using voting_models.Models;
using System.Collections.Generic;
using System.Linq;

namespace voting_bl.Service
{
    public class VotingSessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VotingSessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public VotingSession GetSession(long id)
        {
            VotingSession sessionData = _unitOfWork.VotingSession.Get(id);
            return sessionData;
        }

        public List<VotingSession> GetSession()
        {
            List<VotingSession> sessionData = _unitOfWork.VotingSession.GetSession();
            return sessionData;
        }

        public void SaveSession(VotingSession votingSession)
        {
            _unitOfWork.VotingSession.Add(votingSession);
            _unitOfWork.Save();
        }

    }
}
