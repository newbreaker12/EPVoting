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
        private readonly VotingUsersService _usersService;

        public VoteController(IUnitOfWork unitOfWork)
        {
            _voteService = new VoteService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork);
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<Vote> SaveVote(Vote vote)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "MEP")
            {
                return Unauthorized();
            }
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
        [HttpGet("subarticle/{id}/vote/{type}")]
        public ActionResult<Vote> Vote(long id, int type)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "MEP")
            {
                return Unauthorized();
            }
            string email = GetUsername();

            if (_voteService.hasSubmittedVoteArticle(email, id))
            {
                return BadRequest(new
                {
                    data = "ALREADY SUBMITTED VOTES"
                });
            }

            _voteService.SaveVote(new Vote() { SubArticleId = id, UserEmail = email, Type = type});
            return Ok(new
            {
                data = "VOTED"
            });
        }
        [HttpGet("article/{id}/vote/submit")]
        public ActionResult<Vote> VoteSubmit(long id)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "MEP")
            {
                return Unauthorized();
            }
            string email = GetUsername();

            if(_voteService.hasSubmittedVoteArticle(email, id))
            {
                return BadRequest(new
                {
                    data = "ALREADY SUBMITTED VOTES"
                });
            }

            _voteService.SaveVoteSubmit(new VoteSubmit() { ArticleId = id, UserEmail = email, });
            return Ok(new
            {
                data = "VOTED"
            });
        }

        [HttpGet("{id}")]
        public ActionResult<Vote> GetVote(long id)
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
                data = _voteService.GetVote(id)
            });
        }
        [HttpGet("all")]
        public ActionResult<IEnumerable<Vote>> GetVote()
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
                data = _voteService.GetVote()
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