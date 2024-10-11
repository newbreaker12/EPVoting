using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using voting_bl.Service;
using voting_data_access.Entities;
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
    public class SessionController : ControllerBase
    {
        private readonly VotingSessionService _sessionService;
        private readonly EmailsService _emailService;
        private readonly VotingUsersService _usersService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="SessionController"/>.
        /// </summary>
        /// <param name="unitOfWork">L'unité de travail à utiliser par les services.</param>
        /// <param name="configuration"></param>
        public SessionController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _sessionService = new VotingSessionService(unitOfWork);
            _emailService = new EmailsService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork, configuration);
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
        /// Enregistre une nouvelle session de vote.
        /// </summary>
        /// <param name="session">La session de vote à enregistrer.</param>
        /// <returns>La session de vote enregistrée.</returns>
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ActionResult SaveSession(VotingSession session)
        {
            try
            {
                if (_sessionService.GetActiveSessionByArticleId(session.ArticleId) != null)
                {
                    return BadRequest(new { data = "L'article a déjà une session" });
                }
                _sessionService.SaveSession(session);
                _emailService.SendEmails(session.ArticleId);
                return Ok(new { data = "ok" });
            }
            catch (InvalidVote e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        /// <summary>
        /// Obtient une session de vote par son ID.
        /// </summary>
        /// <param name="id">L'ID de la session de vote à obtenir.</param>
        /// <returns>La session de vote demandée.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult GetSession(long id)
        {
            var session = _sessionService.GetSession(id);
            if (session == null)
            {
                return NotFound(new { data = "Session not found" });
            }
            return Ok(new { data = session });
        }

        /// <summary>
        /// Obtient toutes les sessions de vote.
        /// </summary>
        /// <returns>Une liste de toutes les sessions de vote.</returns>
        [HttpGet("all")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<IEnumerable<VotingSession>> GetSessions()
        {
            var sessions = _sessionService.GetSession();
            return Ok(new { data = sessions });
        }
    }
}
