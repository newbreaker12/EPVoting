namespace voting_models.Models
{
    public class VoteResponse
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public long SessionId { get; set; }
        public long UserEmail { get; set; }
        public string ArticleId { get; set; }
    }
}
