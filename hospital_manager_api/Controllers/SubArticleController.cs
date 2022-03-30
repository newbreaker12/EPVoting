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
    [Route("subarticle")]
    [ApiController]
    public class SubArticleController : Controller
    {
        private readonly VotingSubArticleService _votingSubArticleService;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly VotingUsersService _usersService;

        public SubArticleController(IUnitOfWork unitOfWork)
        {
            _votingSubArticleService = new VotingSubArticleService(unitOfWork);
            _tokenHandler = new JwtSecurityTokenHandler();
            _usersService = new VotingUsersService(unitOfWork);
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingSubArticle> SaveSubArticle(VotingSubArticle subArticle)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRoles(up[0]);
            if (rs.SingleOrDefault(r => r.Name == "ADMIN" || r.Name == "PG") == null)
            {
                return Unauthorized();
            }
            try
            {
                _votingSubArticleService.SaveSubArticle(subArticle);
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
        [HttpPut]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingSubArticle> UpdateSubArticle(VotingSubArticle subArticle)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRoles(up[0]);
            if (rs.SingleOrDefault(r => r.Name == "ADMIN" || r.Name == "PG") == null)
            {
                return Unauthorized();
            }
            try
            {
                _votingSubArticleService.EditSubArticle(subArticle);
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

        [HttpGet("admin/article/{id}")]
        public ActionResult<List<VotingSubArticle>> GetSubAsByArticleIdForAdmin(long id)
        {
            return Ok(new
            {
                data = _votingSubArticleService.GetSubAsByArticleId(id)
            });
        }
        [HttpGet("article/{id}")]
        public ActionResult<List<VotingSubArticleResponse>> GetSubAsByArticleIdForUser(long id)
        {
            string email = GetUsername();
            return Ok(new
            {
                data = _votingSubArticleService.GetSubAsByArticleIdAndEmail(id, email)
            });
        }
        [HttpDelete("{id}")]
        public ActionResult<string> Delete(long id)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRoles(up[0]);
            if (rs.SingleOrDefault(r => r.Name == "ADMIN" || r.Name == "PG") == null)
            {
                return Unauthorized();
            }
            try {
                _votingSubArticleService.Delete(id);
            return Ok(new
            {
                data = "ok"
            });
        } catch (InvalidVote e)
            {
                return BadRequest(new
                {
                    data = e.Message
                });
            }
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