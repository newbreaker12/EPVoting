using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using voting_bl.Service;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;

namespace voting_api.Controllers
{
    /// <summary>
    /// Contrôleur pour gérer les demandes liées aux rôles.
    /// </summary>
    [Produces("application/json")]
    [Route("roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly VotingRolesService _rolesService;
        private readonly VotingUsersService _usersService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="RolesController"/>.
        /// </summary>
        /// <param name="unitOfWork">L'unité de travail à utiliser par les services.</param>
        public RolesController(IUnitOfWork unitOfWork)
        {
            _rolesService = new VotingRolesService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork);
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
        /// Enregistre un nouveau rôle.
        /// </summary>
        /// <param name="roles">Le rôle à enregistrer.</param>
        /// <returns>Le rôle enregistré.</returns>
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ActionResult SaveRoles([FromBody] VotingRoles roles)
        {
            try
            {
                _rolesService.SaveRoles(roles);
                return Ok(new { data = "ok" });
            }
            catch (InvalidRoles e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        /// <summary>
        /// Obtient un rôle par son ID.
        /// </summary>
        /// <param name="id">L'ID du rôle à obtenir.</param>
        /// <returns>Le rôle demandé.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<VotingRoles> GetRole(long id)
        {
            var role = _rolesService.GetRoles(id);
            if (role == null)
            {
                return NotFound(new { data = "Role not found" });
            }
            return Ok(new { data = role });
        }

        /// <summary>
        /// Obtient tous les rôles.
        /// </summary>
        /// <returns>Une liste de tous les rôles.</returns>
        [HttpGet("all")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<IEnumerable<VotingRoles>> GetRoles()
        {
            var roles = _rolesService.GetRoles();
            return Ok(new { data = roles });
        }
    }
}
