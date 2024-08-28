using voting_bl.Util;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using voting_models.Models;
using System;

namespace voting_bl.Service
{
    public class VotingSubArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VotingSubArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void Delete(long id)
        {
            _unitOfWork.VotingSubArticle.Remove(_unitOfWork.VotingSubArticle.Get(id));
            _unitOfWork.Save();
        }

        public void EditSubArticle(VotingSubArticle votingSubArticleRequest)
        {
            VotingSubArticle votingSubArticle = _unitOfWork.VotingSubArticle.Get(votingSubArticleRequest.Id);
            votingSubArticle.Name = votingSubArticleRequest.Name;
            votingSubArticle.Description = votingSubArticleRequest.Description;
            _unitOfWork.VotingSubArticle.Update(votingSubArticle);
            _unitOfWork.Save();
        }

        public void SaveSubArticle(VotingSubArticle votingSubArticle)
        {
            DateTime now = DateTime.Now;
            votingSubArticle.CreatedAt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second).ToUniversalTime();
            _unitOfWork.VotingSubArticle.Add(votingSubArticle);
            _unitOfWork.Save();
        }

        public VotingSubArticle GetSubArticleById(long id)
        {
            return _unitOfWork.VotingSubArticle.Get(id);
        }
        public List<VotingSubArticle> GetSubAsByArticleId(long id)
        {
            return _unitOfWork.VotingSubArticle.GetSubArticlesByArticleId(id);
        }
        public List<VotingSubArticle> GetAll()
        {
            return _unitOfWork.VotingSubArticle.GetAll();
        }

        public List<VotingSubArticleResponse> GetSubAsByArticleIdAndEmail(long id, string email)
        {
            return _unitOfWork.VotingSubArticle.GetSubArticlesByArticleIdAndEmail(id, email);
        }
    }
}
