using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using voting_bl.Service;
using voting_data_access.Entities;
using Microsoft.Net.Http.Headers;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using voting_models.Response_Models;
using voting_bl.Mapper;
using voting_data_access.Data;

namespace voting_api.Controllers
{
    /// <summary>
    /// Contrôleur pour gérer les demandes liées aux sous-articles.
    /// </summary>
    [Produces("application/json")]
    [Route("subarticle")]
    [ApiController]
    public class SubArticleController : Controller
    {
        private readonly VotingArticleService _articleService;
        private readonly VotingSubArticleService _votingSubArticleService;
        private readonly VotingUsersService _usersService;
        private readonly VoteMapper _voteMapper;
        private readonly StatisticsService statisticsService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="SubArticleController"/>.
        /// </summary>
        /// <param name="unitOfWork">L'unité de travail à utiliser par les services.</param>
        public SubArticleController(IUnitOfWork unitOfWork, VotingDbContext votingDbContext)
        {
            _votingSubArticleService = new VotingSubArticleService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork);
            _articleService = new VotingArticleService(unitOfWork);
            _voteMapper = new VoteMapper(unitOfWork);
            statisticsService = new StatisticsService(votingDbContext);
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
        /// Obtient les statistiques des sous-articles.
        /// </summary>
        /// <returns>Une liste des réponses de recherche de statistiques.</returns>
        [HttpGet("statistics")]
        public ActionResult<IEnumerable<VoteStatistics>> GetStatistics()
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
            var results = statisticsService.GetVoteStatisticsAsync().Result;
            return Ok(new
            {
                data = results
            }) ;
        }

        /// <summary>
        /// Enregistre un nouveau sous-article.
        /// </summary>
        /// <param name="subArticle">Le sous-article à enregistrer.</param>
        /// <returns>Le sous-article enregistré.</returns>
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

        /// <summary>
        /// Met à jour un sous-article existant.
        /// </summary>
        /// <param name="subArticle">Le sous-article à mettre à jour.</param>
        /// <returns>Le sous-article mis à jour.</returns>
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

        /// <summary>
        /// Obtient tous les sous-articles pour un administrateur par ID d'article.
        /// </summary>
        /// <param name="id">L'ID de l'article.</param>
        /// <returns>Une liste de sous-articles.</returns>
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

        /// <summary>
        /// Obtient tous les sous-articles pour un utilisateur par ID d'article.
        /// </summary>
        /// <param name="id">L'ID de l'article.</param>
        /// <returns>Une liste de réponses de sous-articles.</returns>
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
            var sar = _votingSubArticleService.GetSubArticleById(id);
            var ar = _articleService.GetArticle(sar.ArticleId);
            if (rs.Name != "ADMIN" && rs.Name != "PG" && rs.Name != ar.Group.Name)
            {
                return Unauthorized();
            }
            string email = GetUsername();
            return Ok(new
            {
                data = _votingSubArticleService.GetSubAsByArticleIdAndEmail(id, email)
            });
        }

        /// <summary>
        /// Supprime un sous-article par son ID.
        /// </summary>
        /// <param name="id">L'ID du sous-article à supprimer.</param>
        /// <returns>Une réponse de confirmation de suppression.</returns>
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
            try
            {
                _votingSubArticleService.Delete(id);
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
