using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace voting_data_access.Entities
{
    [Table(name: "VotingGroups")]
    public class VotingGroups
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string ReadableId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Disabled { get; set; }
    }
}
