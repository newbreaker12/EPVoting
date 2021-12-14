namespace voting_models.Models
{
    public class VotingArticleRequest
    {
        public long Id { get; set; }
        public long GroupsId { get; set; }
        public string Name { get; set; }
        public long Description { get; set; }
        public string CreatedAt { get; set; }

    }
}
