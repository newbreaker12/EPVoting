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
    /// Contrôleur pour gérer les demandes liées aux groupes.
    /// </summary>
    [Produces("application/json")]
    [Route("groups")]
    [ApiController]
    public class GroupsController : Controller
    {
        private readonly VotingGroupsService _groupsService;
        private readonly VotingUsersService _usersService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="GroupsController"/>.
        /// </summary>
        /// <param name="unitOfWork">L'unité de travail à utiliser par les services.</param>
        public GroupsController(IUnitOfWork unitOfWork)
        {
            _groupsService = new VotingGroupsService(unitOfWork);
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
        /// Enregistre un nouveau groupe.
        /// </summary>
        /// <param name="groups">Le groupe à enregistrer.</param>
        /// <returns>Le groupe enregistré.</returns>
        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingGroups> SaveGroups(VotingGroups groups)
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
                _groupsService.SaveGroups(groups);
                return Ok(new
                {
                    data = "ok"
                });
            }
            catch (InvalidGroups e)
            {
                return BadRequest(new
                {
                    data = e.Message
                });
            }
        }

        /// <summary>
        /// Obtient un groupe par son ID.
        /// </summary>
        /// <param name="id">L'ID du groupe à obtenir.</param>
        /// <returns>Le groupe demandé.</returns>
        [HttpGet("{id}")]
        public ActionResult<VotingGroups> GetGroup(long id)
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
                data = _groupsService.GetGroups(id)
            });
        }

        /// <summary>
        /// Obtient tous les groupes.
        /// </summary>
        /// <returns>Une liste de tous les groupes.</returns>
        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingGroups>> GetGroups()
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
                data = _groupsService.GetGroups()
            });
        }

        /// <summary>
        /// Modifie un groupe existant.
        /// </summary>
        /// <param name="groups">Le groupe à modifier.</param>
        /// <returns>Le groupe modifié.</returns>
        [HttpPut]
        public ActionResult<VotingGroups> PutGroup(VotingGroups groups)
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
                data = _groupsService.UpdateGroups(groups)
            });
        }

        /// <summary>
        /// Supprime un groupe par son ID.
        /// </summary>
        /// <param name="id">L'ID du groupe à supprimer.</param>
        /// <returns>Le groupe supprimé.</returns>
        [HttpDelete("{id}")]
        public ActionResult<VotingGroups> DeleteGroup(long id)
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
            var users = _usersService.GetUsersByGroup(id);
            if (users.Count > 0)
                return BadRequest(new
                {
                    data = "Des utilisateurs sont assignés à ce groupe !"
                });
            return Ok(new
            {
                data = _groupsService.DeleteGroups(id)
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
