using System.Collections.Generic;

namespace voting_models.Models
{
    public class VotingSessionRequest
    {
        public long Id { get; set; }
        public long ArticleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }

    }
}
