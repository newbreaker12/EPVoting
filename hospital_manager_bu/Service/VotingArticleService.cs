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
            VotingArticle articleData = _unitOfWork.votingArticle.Get(id);
            return articleData;
        }

        public List<VotingArticle> GetArticles()
        {
            List<VotingArticle> articleData = _unitOfWork.votingArticle.GetArticles();
            return articleData;
        }

        public void SaveArticle(VotingArticle votingArticle)
        {
            _unitOfWork.votingArticle.Add(votingArticle);
            _unitOfWork.Save();
        }
    }
}
