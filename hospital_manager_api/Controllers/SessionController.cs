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
    /// <summary>
    /// Contrôleur pour gérer les demandes liées aux sessions de vote.
    /// </summary>
    [Produces("application/json")]
    [Route("session")]
    [ApiController]
    public class SessionController : Controller
    {
        private readonly VotingSessionService _sessionService;
        private readonly EmailsService _emailService;
        private readonly VotingUsersService _usersService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="SessionController"/>.
        /// </summary>
        /// <param name="unitOfWork">L'unité de travail à utiliser par les services.</param>
        public SessionController(IUnitOfWork unitOfWork)
        {
            _sessionService = new VotingSessionService(unitOfWork);
            _emailService = new EmailsService(unitOfWork);
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
        /// Enregistre une nouvelle session de vote.
        /// </summary>
        /// <param name="session">La session de vote à enregistrer.</param>
        /// <returns>La session de vote enregistrée.</returns>
        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingSession> SaveSession(VotingSession session)
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
            try
            {
                if (_sessionService.GetActiveSessionByArticleId(session.ArticleId) != null)
                {
                    return BadRequest(new
                    {
                        data = "L'article a déjà une session"
                    });
                }
                _sessionService.SaveSession(session);
                _emailService.SendEmails(session.ArticleId);
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
        /// Obtient une session de vote par son ID.
        /// </summary>
        /// <param name="id">L'ID de la session de vote à obtenir.</param>
        /// <returns>La session de vote demandée.</returns>
        [HttpGet("{id}")]
        public ActionResult<VotingSession> GetSession(long id)
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
                data = _sessionService.GetSession(id)
            });
        }

        /// <summary>
        /// Obtient toutes les sessions de vote.
        /// </summary>
        /// <returns>Une liste de toutes les sessions de vote.</returns>
        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingSession>> GetSession()
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
                data = _sessionService.GetSession()
            });
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
