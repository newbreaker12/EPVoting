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
    /// Contrôleur pour gérer les demandes liées aux groupes.
    /// </summary>
    [Produces("application/json")]
    [Route("groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly VotingGroupsService _groupsService;
        private readonly VotingUsersService _usersService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="GroupsController"/>.
        /// </summary>
        /// <param name="unitOfWork">L'unité de travail à utiliser par les services.</param>
        public GroupsController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _groupsService = new VotingGroupsService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork, _configuration);
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
        /// Enregistre un nouveau groupe.
        /// </summary>
        /// <param name="groups">Le groupe à enregistrer.</param>
        /// <returns>Le groupe enregistré.</returns>
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ActionResult SaveGroups(VotingGroups groups)
        {
            try
            {
                _groupsService.SaveGroups(groups);
                return Ok(new { data = "ok" });
            }
            catch (InvalidGroups e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        /// <summary>
        /// Obtient un groupe par son ID.
        /// </summary>
        /// <param name="id">L'ID du groupe à obtenir.</param>
        /// <returns>Le groupe demandé.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult GetGroup(long id)
        {
            var group = _groupsService.GetGroups(id);
            if (group == null)
            {
                return NotFound(new { data = "Group not found" });
            }
            return Ok(new { data = group });
        }

        /// <summary>
        /// Obtient tous les groupes.
        /// </summary>
        /// <returns>Une liste de tous les groupes.</returns>
        [HttpGet("all")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<IEnumerable<VotingGroups>> GetGroups()
        {
            var groups = _groupsService.GetGroups();
            return Ok(new { data = groups });
        }

        /// <summary>
        /// Modifie un groupe existant.
        /// </summary>
        /// <param name="groups">Le groupe à modifier.</param>
        /// <returns>Le groupe modifié.</returns>
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ActionResult UpdateGroup(VotingGroups groups)
        {
            try
            {
                var updatedGroup = _groupsService.UpdateGroups(groups);
                return Ok(new { data = updatedGroup });
            }
            catch (InvalidGroups e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        /// <summary>
        /// Supprime un groupe par son ID.
        /// </summary>
        /// <param name="id">L'ID du groupe à supprimer.</param>
        /// <returns>Le groupe supprimé.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult DeleteGroup(long id)
        {
            var users = _usersService.GetUsersByGroup(id);
            if (users.Count > 0)
            {
                return BadRequest(new { data = "Des utilisateurs sont assignés à ce groupe !" });
            }
            _groupsService.DeleteGroups(id);
            return Ok(new { data = "Group deleted successfully" });
        }
    }
}
