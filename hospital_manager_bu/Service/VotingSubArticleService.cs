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

        public void SaveSubArticle(VotingSubArticle votingSubArticle)
        {
            DateTime now = DateTime.Now;
            votingSubArticle.CreatedAt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            _unitOfWork.VotingSubArticle.Add(votingSubArticle);
            _unitOfWork.Save();
        }
    }
}
