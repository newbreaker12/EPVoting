using System;
using System.Collections.Generic;
using System.Text;
using voting_bl.Service;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_models.Response_Models;

namespace voting_bl.Mapper
{
    public class StatisticsMapper
    {
        private readonly VotingArticleService _articleService;
        private readonly VotingSessionService votingSessionService;
        private readonly VotingSubArticleService _votingSubArticleService;
        private readonly VoteService voteService;

        public StatisticsMapper(IUnitOfWork unitOfWork)
        {
            _votingSubArticleService = new VotingSubArticleService(unitOfWork);
            votingSessionService = new VotingSessionService(unitOfWork);
            _articleService = new VotingArticleService(unitOfWork);
            voteService = new VoteService(unitOfWork);
        }

        public StatisticsSearchResponse Map(VotingSubArticle source)
        {
            VotingArticleResponse votingArticle = _articleService.GetArticleBySubArticleId(source.ArticleId);
            VotingSession session = votingSessionService.GetActiveSessionByArticleId(source.ArticleId);

            string articleName = votingArticle == null ? "" : votingArticle.Name;

            string sessionName = "";
            DateTime sessionFrom = new DateTime();
            DateTime sessionTo = new DateTime();
            if (session != null)
            {
                sessionName = session.Name;
                sessionFrom = session.From;
                sessionTo = session.To;
            }

            StatisticsSearchResponse destination = new StatisticsSearchResponse();
            // 0 = not in favour; 1 = neutral; 2 = in favour
            destination.VoteInFavourCount = voteService.GetVoteCountByTypeAndSubArticle(2, source.Id);
            destination.VoteNeutralCount = voteService.GetVoteCountByTypeAndSubArticle(1, source.Id);
            destination.VoteNotInFavourCount = voteService.GetVoteCountByTypeAndSubArticle(0, source.Id);
            destination.VoteCount = destination.VoteInFavourCount + destination.VoteNeutralCount + destination.VoteNotInFavourCount;

            if (votingArticle.Group != null) {
                destination.GroupName = votingArticle.Group.Name;
            }

            destination.SubArticleName = source.Name;
            destination.ArticleName = articleName;
            destination.SessionName = sessionName;
            destination.From = sessionFrom;
            destination.To = sessionTo;


            return destination;
        }
    }
}
