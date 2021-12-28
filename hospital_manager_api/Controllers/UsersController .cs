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
    [Route("users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly VotingUsersService _usersService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public UsersController(IUnitOfWork unitOfWork)
        {
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
        public ActionResult<VotingUsers> Saveusers(VotingUsers users)
        {
            try
            {
                _usersService.SaveUsers(users);
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
        public ActionResult<VotingUsers> GetUsers(long id)
        {
            return Ok(new
            {
                data = _usersService.GetUsers(id)
            });
        }
        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingUsers>> GetUsers()
        {
            return Ok(new
            {
                data = _usersService.GetUsers()
            });
        }

        private string GetClaim(string name)
        {
            var accessTokenString = Request.Headers[HeaderNames.Authorization].ToString();

            if (accessTokenString == null || !accessTokenString.Contains("Bearer "))
            {
                return "NONE";
            }

            try
            {
                var accessToken = _tokenHandler.ReadToken(accessTokenString.Replace("Bearer ", "")) as JwtSecurityToken;
                return accessToken.Claims.Single(claim => claim.Type == name).Value;
            }
            catch (ArgumentException)
            {
                return "NONE";
            }
        }
    }
}