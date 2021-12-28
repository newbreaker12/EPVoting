using System;
using System.Collections.Generic;

namespace voting_models.Models
{
    public class VotingGroupsRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ReadableId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
