using voting_bl.Util;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using voting_models.Models;

namespace voting_bl.Service
{
    public class VotingArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VotingArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public VotingArticle GetArticle(long id)
        {
            VotingArticle articleData = _unitOfWork.VotingArticle.Get(id);
            return articleData;
        }

        public List<VotingArticleResponse> GetArticles()
        {
            List<VotingArticleResponse> articleData = _unitOfWork.VotingArticle.GetArticles();
            return articleData;
        }

        public void SaveArticle(VotingArticle votingArticle)
        {
            _unitOfWork.VotingArticle.Add(votingArticle);
            _unitOfWork.Save();
        }
        public List<VotingArticleResponse> GetArticlesForUser(string email)
        {
            List<VotingArticleResponse> articleData = _unitOfWork.VotingArticle.GetArticleForUser(email);
            return articleData;
        }
    }
}
