﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace voting_data_access.Entities
{
    [Table(name: "Vote")]
    public class Vote
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserEmail { get; set; }
        public long SubArticleId { get; set; }

        // 0 = not in favour; 1 = neutral; 2 = in favour
        public int Type { get; set; }

    }
}
