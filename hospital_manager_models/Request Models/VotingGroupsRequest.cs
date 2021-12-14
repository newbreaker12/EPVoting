using System.Collections.Generic;

namespace voting_models.Models
{
    public class VotingGroupsRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ReadableId { get; set; }
        public string CreatedAt { get; set; }
    }
}
