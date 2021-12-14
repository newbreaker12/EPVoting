using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace voting_data_access.Entities
{
    [Table(name: "VotingSession")]
    public class VotingSession
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long GroupsId { get; set; }
        public string Name { get; set; }
        public long Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }

    }
}
