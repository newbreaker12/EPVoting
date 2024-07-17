using System.Collections.Generic;

namespace voting_models.Models
{
    public class VotingUsersResponse
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsMEP { get; set; }
        public VotingRolesResponse Role { get; set; }
        public VotingGroupsResponse Groups { get; set; }
    }


}