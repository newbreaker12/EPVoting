namespace voting_models.Models
{
    public class VoteSubmitResponse
    {
        public long Id { get; set; }
        public string UserEmail { get; set; }
        public long ArticleId { get; set; }
    }
}
