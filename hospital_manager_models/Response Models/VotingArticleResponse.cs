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
        public long Description { get; set; }
        public string CreatedAt { get; set; }
    }
}
