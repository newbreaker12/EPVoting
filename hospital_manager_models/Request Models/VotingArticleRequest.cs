using System;

namespace voting_models.Models
{
    public class VotingArticleRequest
    {
        public long Id { get; set; }
        public long GroupsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
