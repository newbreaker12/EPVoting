using voting_models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace voting_data_access.Entities
{
    public class VotingArticleResponse
    {
        public long Id { get; set; }
        public long GroupsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public VotingSessionResponse Session { get; set; }
        public List<VotingSessionResponse> Sessions { get; set; }
        public List<VotingSubArticleResponse> SubArticles { get; set; }
        public VotingGroupsResponse Group { get; set; }
        public VoteSubmitResponse VoteSubmitResponse { get; set; }
        public bool Submitted { get; set; }
    }
}
