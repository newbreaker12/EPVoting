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
        public List<UserToRole> Roles { get; set; }
        public List<UserToGroup> Groups { get; set; }
    }

    [Table(name: "UserToGroup")]
    public class UserToGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long GroupId { get; set; }

    }
    [Table(name: "UserToRole")]
    public class UserToRole
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long RoleId { get; set; }

    }
}
