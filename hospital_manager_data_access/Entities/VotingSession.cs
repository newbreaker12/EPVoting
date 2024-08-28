using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_data_access.Entities
{
    [Table(name: "votingsession")]
    public class VotingSession
    {
        [Column("id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("articleid")]
        public long ArticleId { get; set; }


        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }

        [Column("fromdate")]
        public DateTime From { get; set; }

        [Column("todate")]
        public DateTime To { get; set; }

    }
}
