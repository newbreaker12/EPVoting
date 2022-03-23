namespace voting_models.Models
{
    public class VoteRequest
    {
        public long Id { get; set; }
        public string UserEmail { get; set; }
        public long SubArticleId { get; set; }
    }
}
