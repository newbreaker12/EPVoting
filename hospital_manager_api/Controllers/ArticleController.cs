using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using voting_bl.Service;
using voting_data_access.Entities;
using Microsoft.AspNetCore.Authorization;
using voting_models.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Net.Http.Headers;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;

namespace voting_api.Controllers
{
    [Produces("application/json")]
    [Route("article")]
    [ApiController]
    public class ArticleController : Controller
    {
        private readonly VotingArticleService _articleService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public ArticleController(IUnitOfWork unitOfWork)
        {
            _articleService = new VotingArticleService(unitOfWork);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingArticle> SaveArticle(VotingArticle article)
        {
            try
            {
                _articleService.SaveArticle(article);
                return Ok(new
                {
                    data = "ok"
                });
            }
            catch (InvalidArticle e)
            {
                return BadRequest(new
                {
                    data = e.Message
                });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<VotingArticle> GetArticle(long id)
        {
            return Ok(new
            {
                data = _articleService.GetArticle(id)
            });
        }
        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingArticle>> GetArticles()
        {
            return Ok(new
            {
                data = _articleService.GetArticles()
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