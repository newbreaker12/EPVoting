using System;
using System.Collections.Generic;
using System.Linq;
using voting_data_access.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Net.Http.Headers;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using voting_bl.Service;

namespace voting_api.Controllers
{
    [Produces("application/json")]
    [Route("article")]
    [ApiController]
    public class ArticleController : Controller
    {
        private readonly VotingArticleService _articleService;
        private readonly VotingUsersService _usersService;

        public ArticleController(IUnitOfWork unitOfWork)
        {
            _articleService = new VotingArticleService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork);
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

        [HttpPost]
        public ActionResult<VotingArticle> SaveArticle(VotingArticle article)
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
            try
            {
                _articleService.SaveArticle(article);
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
        public ActionResult<VotingArticle> EditArticle(VotingArticle article)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            var ar = _articleService.GetArticle(article.Id);
            if (rs.Name != "ADMIN" && rs.Name != "PG" && rs.Name != ar.Group.Name)
            {
                return Unauthorized();
            }
            try
            {
                _articleService.EditArticle(article.Id, article.Description);
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
        public ActionResult<VotingArticleResponse> GetArticle(long id)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if(up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            var ar = _articleService.GetArticle(id);
            if (rs.Name != "ADMIN" && rs.Name != "PG" && rs.Name != ar.Group.Name)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                data = _articleService.GetArticle(id)
            });
        }


        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingArticle>> GetArticles()
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
                data = _articleService.GetArticles()
            });
        }



        [HttpGet("user")]
        public ActionResult<List<VotingArticleResponse>> GetArticleForUser()
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            try
            {
                return Ok(new
                {
                    data = _articleService.GetArticlesForUser(GetUsername())
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