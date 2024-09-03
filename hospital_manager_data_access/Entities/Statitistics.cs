namespace voting_data_access.Entities
{
    public class VoteStatistics
    {
        public string GroupName { get; set; }
        public string ArticleName { get; set; }
        public string SubArticleName { get; set; }
        public int InFavorCount { get; set; }
        public int NotInFavorCount { get; set; }
        public int NeutralCount { get; set; }
    }
}
