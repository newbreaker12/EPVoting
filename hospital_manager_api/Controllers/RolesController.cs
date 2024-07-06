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
    /// Contrôleur pour gérer les demandes liées aux rôles.
    /// </summary>
    [Produces("application/json")]
    [Route("roles")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly VotingRolesService _rolesService;
        private readonly VotingUsersService _usersService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="RolesController"/>.
        /// </summary>
        /// <param name="unitOfWork">L'unité de travail à utiliser par les services.</param>
        public RolesController(IUnitOfWork unitOfWork)
        {
            _rolesService = new VotingRolesService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork);
            _tokenHandler = new JwtSecurityTokenHandler();
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
        /// Enregistre un nouveau rôle.
        /// </summary>
        /// <param name="roles">Le rôle à enregistrer.</param>
        /// <returns>Le rôle enregistré.</returns>
        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingRoles> SaveRoles(VotingRoles roles)
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
                _rolesService.SaveRoles(roles);
                return Ok(new
                {
                    data = "ok"
                });
            }
            catch (InvalidRoles e)
            {
                return BadRequest(new
                {
                    data = e.Message
                });
            }
        }

        /// <summary>
        /// Obtient un rôle par son ID.
        /// </summary>
        /// <param name="id">L'ID du rôle à obtenir.</param>
        /// <returns>Le rôle demandé.</returns>
        [HttpGet("{id}")]
        public ActionResult<VotingRoles> GetRole(long id)
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
                data = _rolesService.GetRoles(id)
            });
        }

        /// <summary>
        /// Obtient tous les rôles.
        /// </summary>
        /// <returns>Une liste de tous les rôles.</returns>
        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingRoles>> GetRoles()
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
                data = _rolesService.GetRoles()
            });
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
