﻿namespace voting_models.Models
{
    public class VoteResponse
    {
        public long Id { get; set; }
        public string UserEmail { get; set; }
        public long SubArticleId { get; set; }
        public int Type { get; set; }
    }
}
