using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace voting_data_access.Entities
{
    [Table(name: "vote")]
    public class Vote
    {

        [Column("id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("useremail")]
        public string UserEmail { get; set; }

        [Column("subarticleid")]
        public long SubArticleId { get; set; }


        [Column("type")]
        // 0 = not in favour; 1 = neutral; 2 = in favour
        public int Type { get; set; }

    }
}
