using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace voting_data_access.Entities
{
    [Table(name: "VoteSubmit")]
    public class VoteSubmit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserEmail { get; set; }
        public long ArticleId { get; set; }

    }
}
