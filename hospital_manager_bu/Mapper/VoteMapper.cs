using System;
using System.Collections.Generic;
using System.Text;
using voting_bl.Service;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_models.Response_Models;

namespace voting_bl.Mapper
{
    public class VoteMapper
    {
        private readonly VoteService voteService;
        private readonly VotingArticleService _articleService;
        private readonly VotingSubArticleService _votingSubArticleService;

        public VoteMapper(IUnitOfWork unitOfWork)
        {
            _votingSubArticleService = new VotingSubArticleService(unitOfWork);
            _articleService = new VotingArticleService(unitOfWork);
            voteService = new VoteService(unitOfWork);
        }

        public VoteSearchResponse Map(Vote source)
        {
            VotingSubArticle votingSubArticle = _votingSubArticleService.GetSubArticleById(source.SubArticleId);
            VotingArticleResponse votingArticle = _articleService.GetArticleBySubArticleId(votingSubArticle.ArticleId);

            string articleName = votingArticle == null ? "" : votingArticle.Name;
            string subArticleName = votingSubArticle == null ? "" : votingSubArticle.Name;

            VoteSearchResponse destination = new VoteSearchResponse();
            destination.Id = source.Id;
            destination.ArticleName = articleName;
            destination.SubArticleName = subArticleName;

            // 0 = not in favour; 1 = neutral; 2 = in favour
            switch (source.Type)
            {
                case 0:
                    destination.VoteType = "Not In Favour";
                    break;
                case 1:
                    destination.VoteType = "Neutral";
                    break;
                case 2:
                    destination.VoteType = "In Favour";
                    break;
            }
            return destination;
        }


    }
}
