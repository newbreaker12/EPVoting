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
    [Route("vote")]
    [ApiController]
    public class VoteController : Controller
    {
        private readonly VoteService _voteService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public VoteController(IUnitOfWork unitOfWork)
        {
            _voteService = new VoteService(unitOfWork);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<Vote> SaveVote(Vote vote)
        {
            try
            {
                _voteService.SaveVote(vote);
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
        public ActionResult<Vote> GetVote(long id)
        {
            return Ok(new
            {
                data = _voteService.GetVote(id)
            });
        }
        [HttpGet("all")]
        public ActionResult<IEnumerable<Vote>> GetVote()
        {
            return Ok(new
            {
                data = _voteService.GetVote()
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