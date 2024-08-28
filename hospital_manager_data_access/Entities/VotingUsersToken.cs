using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace voting_data_access.Entities
{
    [Table(name: "votinguserstoken")]
    public class VotingUsersToken
    {

        [Column("id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("expirationdate")]
        public DateTime ExpirationDate { get; set; }
    }
}
