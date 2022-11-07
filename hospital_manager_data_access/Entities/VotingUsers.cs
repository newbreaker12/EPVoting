using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace voting_data_access.Entities
{
    [Table(name: "VotingUsers")]
    public class VotingUsers
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsMEP { get; set; }
        public long RoleId { get; set; }
        public long GroupId { get; set; }
        public bool Disabled { get; set; }
    }

}
