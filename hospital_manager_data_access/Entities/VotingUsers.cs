using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_data_access.Entities
{
    [Table("votingusers")]
    public class VotingUsers
    {
        [Column("id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("phonenumber")]
        public string PhoneNumber { get; set; }


        [Column("ismep")]
        public bool IsMEP { get; set; }

        [Column("roleid")]
        public long RoleId { get; set; }

        [Column("groupid")]
        public long GroupId { get; set; }

        [Column("disabled")]
        public bool Disabled { get; set; }
    }

}
