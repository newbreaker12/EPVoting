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
        public List<UserToRoleResponse> Roles { get; set; }
        public List<UserToGroupResponse> Groups { get; set; }
    }

    public class UserToGroupResponse
    {
        public long Id { get; set; }
        public long GroupId { get; set; }
    }

    public class UserToRoleResponse
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
    }
}
