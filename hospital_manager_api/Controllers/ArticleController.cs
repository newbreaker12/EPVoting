using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using voting_bl.Service;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;

namespace voting_api.Controllers
{
    /// <summary>
    /// Contrôleur pour gérer les demandes liées aux articles.
    /// </summary>
    [Produces("application/json")]
    [Route("article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly VotingArticleService _articleService;
        private readonly VotingUsersService _usersService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ArticleController"/>.
        /// </summary>
        /// <param name="unitOfWork">L'unité de travail à utiliser par les services.</param>
        public ArticleController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _articleService = new VotingArticleService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork, configuration);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Une méthode de ping simple pour vérifier si le contrôleur répond.
        /// </summary>
        /// <returns>Une réponse de chaîne "OK".</returns>
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("OK");
        }

        /// <summary>
        /// Enregistre un nouvel article.
        /// </summary>
        /// <param name="article">L'article à enregistrer.</param>
        /// <returns>L'article enregistré.</returns>
        [HttpPost]
        [Authorize(Roles = "ADMIN,PG")]
        public ActionResult SaveArticle([FromBody] VotingArticle article)
        {
            try
            {
                _articleService.SaveArticle(article);
                return Ok(new { data = "ok" });
            }
            catch (InvalidVote e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        /// <summary>
        /// Modifie un article existant.
        /// </summary>
        /// <param name="article">L'article à modifier.</param>
        /// <returns>L'article modifié.</returns>
        [HttpPut]
        [Authorize]
        public ActionResult EditArticle([FromBody] VotingArticle article)
        {
            var email = GetClaim("email");
            var userRole = _usersService.getRole(email);
            var existingArticle = _articleService.GetArticle(article.Id);

            if (existingArticle == null)
            {
                return NotFound(new { data = "Article not found" });
            }

            if (userRole.Name != "ADMIN" && userRole.Name != "PG" && userRole.Name != existingArticle.Group.Name)
            {
                return Unauthorized();
            }

            try
            {
                _articleService.EditArticle(article.Id, article.Description);
                return Ok(new { data = "ok" });
            }
            catch (InvalidVote e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        /// <summary>
        /// Obtient un article par son ID.
        /// </summary>
        /// <param name="id">L'ID de l'article à obtenir.</param>
        /// <returns>L'article demandé.</returns>
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<VotingArticleResponse> GetArticle(long id)
        {
            var userRole = GetClaim("role");
            var article = _articleService.GetArticle(id);

            if (article == null)
            {
                return NotFound(new { data = "Article not found" });
            }

            if (userRole != "ADMIN" && userRole != "PG" && userRole != article.Group.Name)
            {
                return Unauthorized();
            }

            return Ok(new { data = article });
        }

        /// <summary>
        /// Obtient tous les articles.
        /// </summary>
        /// <returns>Une liste de tous les articles.</returns>
        [HttpGet("all")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<IEnumerable<VotingArticleResponse>> GetArticles()
        {
            return Ok(new { data = _articleService.GetArticles() });
        }

        /// <summary>
        /// Obtient les articles pour un utilisateur spécifique.
        /// </summary>
        /// <returns>Une liste d'articles pour l'utilisateur.</returns>
        [HttpGet("user")]
        [Authorize]
        public ActionResult<List<VotingArticleResponse>> GetArticleForUser()
        {
            var email = GetClaim("email");
            try
            {
                return Ok(new { data = _articleService.GetArticlesForUser(email) });
            }
            catch (InvalidVote e)
            {
                return BadRequest(new { data = e.Message });
            }
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
