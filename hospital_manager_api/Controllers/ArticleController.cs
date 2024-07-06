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
    /// <summary>
    /// Contrôleur pour gérer les demandes liées aux articles.
    /// </summary>
    [Produces("application/json")]
    [Route("article")]
    [ApiController]
    public class ArticleController : Controller
    {
        private readonly VotingArticleService _articleService;
        private readonly VotingUsersService _usersService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ArticleController"/>.
        /// </summary>
        /// <param name="unitOfWork">L'unité de travail à utiliser par les services.</param>
        public ArticleController(IUnitOfWork unitOfWork)
        {
            _articleService = new VotingArticleService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork);
        }

        /// <summary>
        /// Une méthode de ping simple pour vérifier si le contrôleur répond.
        /// </summary>
        /// <returns>Une réponse de chaîne "OK".</returns>
        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

        /// <summary>
        /// Enregistre un nouvel article.
        /// </summary>
        /// <param name="article">L'article à enregistrer.</param>
        /// <returns>L'article enregistré.</returns>
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

        /// <summary>
        /// Modifie un article existant.
        /// </summary>
        /// <param name="article">L'article à modifier.</param>
        /// <returns>L'article modifié.</returns>
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

        /// <summary>
        /// Obtient un article par son ID.
        /// </summary>
        /// <param name="id">L'ID de l'article à obtenir.</param>
        /// <returns>L'article demandé.</returns>
        [HttpGet("{id}")]
        public ActionResult<VotingArticleResponse> GetArticle(long id)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
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

        /// <summary>
        /// Obtient tous les articles.
        /// </summary>
        /// <returns>Une liste de tous les articles.</returns>
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

        /// <summary>
        /// Obtient les articles pour un utilisateur spécifique.
        /// </summary>
        /// <returns>Une liste d'articles pour l'utilisateur.</returns>
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

        /// <summary>
        /// Obtient le nom d'utilisateur à partir de l'en-tête d'autorisation.
        /// </summary>
        /// <returns>Le nom d'utilisateur.</returns>
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

        /// <summary>
        /// Obtient l'en-tête d'autorisation.
        /// </summary>
        /// <returns>L'en-tête d'autorisation.</returns>
        private string GetAuthorization()
        {
            return Request.Headers[HeaderNames.Authorization].ToString();
        }
    }
}
