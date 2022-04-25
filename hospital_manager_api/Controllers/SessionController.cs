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
    [Route("session")]
    [ApiController]
    public class SessionController : Controller
    {
        private readonly VotingSessionService _sessionService;
        private readonly VotingUsersService _usersService;

        public SessionController(IUnitOfWork unitOfWork)
        {
            _sessionService = new VotingSessionService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork);
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingSession> SaveSession(VotingSession session)
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
                _sessionService.SaveSession(session);
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

        [HttpGet("{id}")]
        public ActionResult<VotingSession> GetSession(long id)
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
                data = _sessionService.GetSession(id)
            });
        }
        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingSession>> GetSession()
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
                data = _sessionService.GetSession()
            });
        }

        private string GetAuthorization()
        {
            return Request.Headers[HeaderNames.Authorization].ToString();
        }
    }
}