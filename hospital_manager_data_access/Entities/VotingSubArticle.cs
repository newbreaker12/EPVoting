using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace voting_data_access.Entities
{
    [Table(name: "votingsubarticle")]
    public class VotingSubArticle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("id")]
        public long Id { get; set; }

        [Column("articleid")]
        public long ArticleId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; }
    }
}
