using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace voting_data_access.Entities
{
    [Table(name: "votesubmit")]
    public class VoteSubmit
    {

        [Column("id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("useremail")]
        public string UserEmail { get; set; }

        [Column("articleid")]
        public long ArticleId { get; set; }

    }
}
