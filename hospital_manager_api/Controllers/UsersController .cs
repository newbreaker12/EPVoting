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

        public UsersController(IUnitOfWork unitOfWork)
        {
            _usersService = new VotingUsersService(unitOfWork);
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
            return _usersService.Authenticate(up[0], up[1]).ToString().ToUpper();
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

        private string GetAuthorization()
        {
            return Request.Headers[HeaderNames.Authorization].ToString();
        }
    }
}