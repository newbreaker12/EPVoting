using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_data_access.Entities
{
    [Table(name: "votingroles")]
    public class VotingRoles
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }

}
