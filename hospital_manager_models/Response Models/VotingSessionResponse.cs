using System;
using System.Collections.Generic;
using voting_data_access.Entities;

namespace voting_models.Models
{
    public class VotingSessionResponse
    {
        public long Id { get; set; }
        public long ArticleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
