using System;
using System.Collections.Generic;
using System.Text;

namespace voting_models.Response_Models
{
    public class StatisticsSearchResponse
    {
        public string SessionName { get; set; }
        public string SubArticleName { get; set; }
        public string ArticleName { get; set; }
        public string GroupName { get; set; }
        public int VoteCount { get; set; }
        public int VoteInFavourCount { get; set; }
        public int VoteNotInFavourCount { get; set; }
        public int VoteNeutralCount { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
