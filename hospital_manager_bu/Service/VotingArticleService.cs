using voting_bl.Util;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_models.Models;
using System;
using System.Collections.Generic;

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
            return _unitOfWork.VotingArticle.getArticle(id);
        }

        public VotingArticleResponse GetArticleBySubArticleId(long subArticleId)
        {
            var subArticle = _unitOfWork.VotingSubArticle.Get(subArticleId);
            if (subArticle != null)
            {
                var article = _unitOfWork.VotingArticle.Get(subArticle.ArticleId);
                if (article != null)
                {
                    return new VotingArticleResponse
                    {
                        Id = article.Id,
                        Description = article.Description,
                        // Include other necessary properties
                        SubArticles = new List<VotingSubArticleResponse>() // Populate sub-articles if needed
                    };
                }
            }
            return null;
        }

        public List<VotingArticleResponse> GetArticles()
        {
            return _unitOfWork.VotingArticle.GetArticles();
        }

        public void SaveArticle(VotingArticle votingArticle)
        {
            votingArticle.CreatedAt = DateTime.Now;
            _unitOfWork.VotingArticle.Add(votingArticle);
            _unitOfWork.Save();
        }

        public void EditArticle(long id, string description)
        {
            var votingArticle = _unitOfWork.VotingArticle.Get(id);
            votingArticle.Description = description;
            _unitOfWork.VotingArticle.Update(votingArticle);
            _unitOfWork.Save();
        }

        public List<VotingArticleResponse> GetArticlesForUser(string email)
        {
            return _unitOfWork.VotingArticle.GetArticleForUser(email);
        }
    }
}
