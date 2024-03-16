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


        public int GetVoteCountByTypeAndSubArticle(int type, long subArticleId)
        {
            return _unitOfWork.Vote.GetVoteCountByTypeAndSubArticle(type, subArticleId);
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

        public bool hasSubmittedVoteArticle(string email, long subArticleId)
        {
            VotingSubArticle votingSubArticle = _unitOfWork.VotingSubArticle.Get(subArticleId);
            return _unitOfWork.VoteSubmit.GetVoteSubmit(email, votingSubArticle.ArticleId) != null;
        }

        public void SaveVote(Vote vote)
        {
            Vote voteTemp = _unitOfWork.Vote.GetVoteForUser(vote.UserEmail, vote.SubArticleId);
            if (voteTemp == null)
            {
                _unitOfWork.Vote.Add(vote);
            } else
            {
                voteTemp.Type = vote.Type;
                _unitOfWork.Vote.Update(voteTemp);
                _unitOfWork.Save();
            }
            _unitOfWork.Save();
        }

        public void SaveVoteSubmit(VoteSubmit votingVote)
        {
            _unitOfWork.VoteSubmit.Add(votingVote);
            _unitOfWork.Save();
        }

    }
}
