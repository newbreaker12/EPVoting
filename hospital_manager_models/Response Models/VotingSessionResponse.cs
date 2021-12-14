using System.Collections.Generic;

namespace voting_models.Models
{
    public class VotingSessionResponse
    {
        public long Id { get; set; }
        public long GroupsId { get; set; }
        public string Name { get; set; }
        public long Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
