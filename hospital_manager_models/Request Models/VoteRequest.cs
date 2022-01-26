namespace voting_models.Models
{
    public class VoteRequest
    {
        public long Id { get; set; }
        public string UserEmail { get; set; }
        public long SessionId { get; set; }
        public long ArticleId { get; set; }
    }
}
