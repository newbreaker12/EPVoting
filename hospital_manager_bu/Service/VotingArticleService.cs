using voting_bl.Util;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using voting_models.Models;
using System;

namespace voting_bl.Service
{
    public class VotingArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VotingArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public VotingArticleResponse GetArticle(long id)
        {
            VotingArticleResponse articleData = _unitOfWork.VotingArticle.getArticle(id);
            return articleData;
        }

        public List<VotingArticleResponse> GetArticles()
        {
            List<VotingArticleResponse> articleData = _unitOfWork.VotingArticle.GetArticles();
            return articleData;
        }

        public void SaveArticle(VotingArticle votingArticle)
        {
            DateTime now = DateTime.Now;
            votingArticle.CreatedAt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            _unitOfWork.VotingArticle.Add(votingArticle);
            _unitOfWork.Save();
        }
        public void EditArticle(long id,  string description)
        {
            VotingArticle votingArticle = _unitOfWork.VotingArticle.Get(id);
            votingArticle.Description = description;
            _unitOfWork.VotingArticle.Update(votingArticle);
            _unitOfWork.Save();
        }
        public List<VotingArticleResponse> GetArticlesForUser(string email)
        {
            List<VotingArticleResponse> articleData = _unitOfWork.VotingArticle.GetArticleForUser(email);
            return articleData;
        }
    }
}
