using voting_models.Models;
using System.Collections.Generic;

namespace voting_data_access.Entities
{
    public class VotingGroupsResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ReadableId { get; set; }
        public string CreatedAt { get; set; }

    }
}
