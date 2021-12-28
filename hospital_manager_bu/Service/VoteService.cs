using voting_bl.Util;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using voting_models.Models;
using System.Collections.Generic;

namespace voting_bl.Service
{
    public class VoteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VoteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Vote GetVote(long id)
        {
            Vote voteData = _unitOfWork.Vote.Get(id);
            return voteData;
        }

        public List<Vote> GetVote()
        {
            List<Vote> voteData = _unitOfWork.Vote.GetVote();
            return voteData;
        }

        public void SaveVote(Vote votingVote)
        {
            _unitOfWork.Vote.Add(votingVote);
            _unitOfWork.Save();
        }

    }
}
