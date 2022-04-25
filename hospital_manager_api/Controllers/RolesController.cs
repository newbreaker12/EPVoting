using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using voting_bl.Service;
using voting_data_access.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Net.Http.Headers;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;

namespace voting_api.Controllers
{
    [Produces("application/json")]
    [Route("roles")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly VotingRolesService _rolesService;
        private readonly VotingUsersService _usersService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public RolesController(IUnitOfWork unitOfWork)
        {
            _rolesService = new VotingRolesService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingRoles> SaveRoles(VotingRoles roles)
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
                _rolesService.SaveRoles(roles);
                return Ok(new
                {
                    data = "ok"
                });
            }
            catch (InvalidRoles e)
            {
                return BadRequest(new
                {
                    data = e.Message
                });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<VotingRoles> GetRole(long id)
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
                data = _rolesService.GetRoles(id)
            });
        }
        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingRoles>> GetRoles()
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
                data = _rolesService.GetRoles()
            });
        }

        private string GetUsername()
        {
            var accessTokenString = Request.Headers[HeaderNames.Authorization].ToString();

            if (accessTokenString == null)
            {
                return "NONE";
            }

            try
            {
                return accessTokenString.Split(":")[0];
            }
            catch (ArgumentException)
            {
                return "NONE";
            }
        }
        private string GetAuthorization()
        {
            return Request.Headers[HeaderNames.Authorization].ToString();
        }
    }
}