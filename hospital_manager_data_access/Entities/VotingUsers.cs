using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_data_access.Entities
{
    [Table("VotingUsers")]
    public class VotingUsers
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PinCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

        public bool IsMEP { get; set; }
        public long RoleId { get; set; }
        public long GroupId { get; set; }
        public bool Disabled { get; set; }
    }

}
