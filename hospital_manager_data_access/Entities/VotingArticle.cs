using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace voting_data_access.Entities
{
    [Table(name: "votingarticle")]
    public class VotingArticle
    {
        [Column("id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("groupsid")]
        public long GroupsId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; }

        //[Column("ispublic")]
        //public bool IsPublic { get; set; }
    }
}
