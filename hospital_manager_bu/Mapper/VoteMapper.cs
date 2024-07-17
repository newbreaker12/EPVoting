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
            var votingSubArticle = _votingSubArticleService.GetSubArticleById(source.SubArticleId);
            var votingArticle = _articleService.GetArticleBySubArticleId(votingSubArticle.Id);

            var articleName = votingArticle == null ? "" : votingArticle.Description; // Assuming Description is the name
            var subArticleName = votingSubArticle == null ? "" : votingSubArticle.Name;

            var destination = new VoteSearchResponse
            {
                Id = source.Id,
                ArticleName = articleName,
                SubArticleName = subArticleName,
                VoteType = source.Type switch
                {
                    0 => "Not In Favour",
                    1 => "Neutral",
                    2 => "In Favour",
                    _ => "Unknown" // Default case to handle unexpected values
                }
            };

            return destination;
        }
    }
}
