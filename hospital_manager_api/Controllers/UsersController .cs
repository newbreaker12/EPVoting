﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using voting_bl.Service;
using voting_data_access.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Net.Http.Headers;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using voting_models.Models;

namespace voting_api.Controllers
{
    [Produces("application/json")]
    [Route("users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly VotingUsersService _usersService;
        private readonly EmailsService _emailsService;
        private readonly VotingGroupsService _votingGroupsService;
        private readonly VotingRolesService _votingRolesService;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _usersService = new VotingUsersService(unitOfWork);
            _emailsService = new EmailsService(unitOfWork);
            _votingGroupsService = new VotingGroupsService(unitOfWork);
            _votingRolesService = new VotingRolesService(unitOfWork);
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

       [HttpGet("login")]
       public ActionResult<string> Authenticate()
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if(_usersService.Authenticate(up[0], up[1]))
            {
                return Ok(new
                {
                    data = "ok"
                });
            } else
            {
                return Unauthorized(new
                {
                    data = "wrong username or password"
                });
            }
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingUsers> Saveusers(VotingUsers users)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "ADMIN")
            {
                return Unauthorized();
            }
            var userRole = _votingRolesService.GetRoles(users.RoleId);
            if (userRole == null)
            {
                return BadRequest("Role doesnt exist");
            }
            var userGroup = _votingGroupsService.GetGroups(users.GroupId);
            if (userGroup == null)
            {
                return BadRequest("Group doesnt exist");
            }
            var user = _usersService.GetUserBEmail(users.Email);
            if (user != null)
            {
                return BadRequest(new
                {
                    data = "This email is already assigned to another user"
                });
            }
            try
            {
                _usersService.SaveUsers(users);
                _emailsService.SendEmail(users.Email, "Account Created", "User has been created: "+users.Email + users.Password);
                return Ok(new
                {
                    data = "ok"
                });
            }
            catch (InvalidUsers e)
            {
                return BadRequest(new
                {
                    data = e.Message
                });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<VotingUsers> GetUser(long id)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "ADMIN")
            {
                return Unauthorized();
            }
            return Ok(new
            {
                data = _usersService.GetUsers(id)
            });
        }

        [HttpDelete("{id}")]
        public ActionResult<VotingUsers> DeleteUser(long id)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "ADMIN")
            {
                return Unauthorized();
            }
            var user = _usersService.GetUsers(id);
            var userRole = _usersService.getRole(user.Email);
            if (userRole.Name == "ADMIN")
            {
                return BadRequest(new
                {
                    data = "Can't delete ADMIN"
                });
            }
            return Ok(new
            {
                data = _usersService.DeleteUsers(id)
            });
        }

        [HttpPut]
        public ActionResult<VotingArticle> EditUser(VotingUsers user)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "ADMIN")
            {
                return Unauthorized();
            }
            try
            {
                _usersService.EditUser(user.Id, user);
                return Ok(new
                {
                    data = "ok"
                });
            }
            catch (InvalidVote e)
            {
                return BadRequest(new
                {
                    data = e.Message
                });
            }
        }

        [HttpGet("email")]
        public ActionResult<VotingUsersResponse> GetUserByEmail()
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }

            return Ok(new
            {
                data = _usersService.GetUserBEmail(up[0])
            });
        }
        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingUsersResponse>> GetUsers()
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "ADMIN")
            {
                return Unauthorized();
            }
            return Ok(new
            {
                data = _usersService.GetUsers()
            });
        }

        private string GetAuthorization()
        {
            return Request.Headers[HeaderNames.Authorization].ToString();
        }
    }
}