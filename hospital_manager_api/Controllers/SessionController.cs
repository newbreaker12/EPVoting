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
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public SessionController(IUnitOfWork unitOfWork)
        {
            _sessionService = new VotingSessionService(unitOfWork);
            _tokenHandler = new JwtSecurityTokenHandler();
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
            return Ok(new
            {
                data = _sessionService.GetSession(id)
            });
        }
        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingSession>> GetSession()
        {
            return Ok(new
            {
                data = _sessionService.GetSession()
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