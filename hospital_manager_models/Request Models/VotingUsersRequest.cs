using System.Collections.Generic;

namespace voting_models.Models
{
    public class VotingUsersRequest
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsMEP { get; set; }
        public long RoleId { get; set; }
        public long GroupId { get; set; }
    }
}