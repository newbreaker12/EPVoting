using System;

namespace voting_models.Response_Models
{
    public class VoteSearchResponse
    {
        public long Id { get; set; }
        public string ArticleName { get; set; }
        public string SubArticleName  { get; set; }
        public string VoteType { get; set; }
    }
}
