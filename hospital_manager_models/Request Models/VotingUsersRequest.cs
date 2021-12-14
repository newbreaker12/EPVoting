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
            public List<UserToRoleRequest> Roles { get; set; }
            public List<UserToGroupRequest> Groups { get; set; }
        }

        public class UserToGroupRequest
        {
            public long Id { get; set; }
            public long GroupId { get; set; }
        }

        public class UserToRoleRequest
        {
            public long Id { get; set; }
            public long RoleId { get; set; }
        }
 }
