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
        private readonly VotingArticleService _articleService;
        private readonly VotingSubArticleService _votingSubArticleService;
        private readonly VotingUsersService _usersService;

        public SubArticleController(IUnitOfWork unitOfWork)
        {
            _votingSubArticleService = new VotingSubArticleService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork);
            _articleService = new VotingArticleService(unitOfWork);
        }


        [HttpPost]
        public ActionResult<VotingSubArticle> SaveSubArticle(VotingSubArticle subArticle)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "ADMIN" && rs.Name != "PG")
            {
                return Unauthorized();
            }
            var ar = _articleService.GetArticle(subArticle.ArticleId);
            if (rs.Name != "PG" && rs.Name != ar.Group.Name && rs.Name != "ADMIN")
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
        public ActionResult<VotingSubArticle> UpdateSubArticle(VotingSubArticle subArticle)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "ADMIN" && rs.Name != "PG" && rs.Name != "ADMIN")
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
                data = _votingSubArticleService.GetSubAsByArticleId(id)
            });
        }
        [HttpGet("article/{id}")]
        public ActionResult<List<VotingSubArticleResponse>> GetSubAsByArticleIdForUser(long id)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "ADMIN" && rs.Name != "PG")
            {
                return Unauthorized();
            }
            var sar = _votingSubArticleService.GetSubArticleById(id);
            var ar = _articleService.GetArticle(sar.ArticleId);
            if (rs.Name != "PG" && rs.Name != ar.Group.Name)
            {
                return Unauthorized();
            }
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
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "ADMIN" && rs.Name != "PG")
            {
                return Unauthorized();
            }
            var sar = _votingSubArticleService.GetSubArticleById(id);
            var ar = _articleService.GetArticle(sar.ArticleId);
            if (rs.Name != "PG" && rs.Name != ar.Group.Name && rs.Name != "ADMIN")
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