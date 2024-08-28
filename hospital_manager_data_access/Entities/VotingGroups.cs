using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace voting_data_access.Entities
{
    [Table(name: "votinggroups")]
    public class VotingGroups
    {
        [Column("id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("readableid")]
        public string ReadableId { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; }

        [Column("disabled")]
        public bool Disabled { get; set; }
    }
}
