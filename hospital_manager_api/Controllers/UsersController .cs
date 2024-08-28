using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using voting_bl.Service;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using voting_models.Models;

namespace voting_api.Controllers
{
    /// <summary>
    /// Contrôleur pour gérer les demandes liées aux utilisateurs.
    /// </summary>
    [Produces("application/json")]
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly VotingUsersService _usersService;
        private readonly EmailsService _emailsService;
        private readonly VotingGroupsService _votingGroupsService;
        private readonly VotingRolesService _votingRolesService;
        private readonly byte[] _salt;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="UsersController"/>.
        /// </summary>
        /// <param name="unitOfWork">L'unité de travail à utiliser par les services.</param>
        /// <param name="configuration">La configuration de l'application.</param>
        public UsersController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _usersService = new VotingUsersService(unitOfWork);
            _emailsService = new EmailsService(unitOfWork);
            _votingGroupsService = new VotingGroupsService(unitOfWork);
            _votingRolesService = new VotingRolesService(unitOfWork);

            // Retrieve the salt (token) from the configuration
            var token = _configuration["AppSettings:Token"];
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is not configured in appsettings.json");
            }
            _salt = Encoding.ASCII.GetBytes(token);
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
        /// Authentifie un utilisateur.
        /// </summary>
        /// <returns>Un jeton d'authentification si l'utilisateur est authentifié, sinon une réponse non autorisée.</returns>
        [HttpPost("login")]
        public ActionResult<string> Authenticate(VotingUsersRequest request)
        {
            VotingUsersResponse user = _usersService.GetUserBEmail(request.Email);

            if (!VerifyPasswordHash(request.Password, user.Password))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);
            return Ok(new { token });
        }

        private string CreateToken(VotingUsersResponse user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private string CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA512(_salt))
            {
                return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        private bool VerifyPasswordHash(string password, string passwordHash)
        {
            using (var hmac = new HMACSHA512(_salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(Convert.FromBase64String(passwordHash));
            }
        }

        private string GenerateRandomPinCode(int length = 5)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var data = new byte[length];
                rng.GetBytes(data);
                var builder = new StringBuilder(length);
                foreach (var b in data)
                {
                    builder.Append((b % 10).ToString());
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur.
        /// </summary>
        /// <param name="users">L'utilisateur à enregistrer.</param>
        /// <returns>L'utilisateur enregistré.</returns>
        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingUsers> SaveUsers(VotingUsers users)
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
            var userRole = _votingRolesService.GetRoles(users.RoleId);
            if (userRole == null)
            {
                return BadRequest("Role doesn't exist");
            }
            var userGroup = _votingGroupsService.GetGroups(users.GroupId);
            if (userGroup == null)
            {
                return BadRequest("Group doesn't exist");
            }
            var user = _usersService.GetUserBEmail(users.Email);
            if (user != null)
            {
                return BadRequest(new
                {
                    data = "This email is already assigned to another user"
                });
            }
            try
            {
                users.PinCode = GenerateRandomPinCode(); // Generate a random pin code
                users.Password = CreatePasswordHash(users.Password); // Hash the password
                _usersService.SaveUsers(users);
                _emailsService.SendEmail(users.Email, "Account Created", "User has been created: " + users.Email + "; " + users.Password + "; " + users.PinCode);
                return Ok(new
                {
                    data = "ok"
                });
            }
            catch (InvalidUsers e)
            {
                return BadRequest(new
                {
                    data = e.Message
                });
            }
        }

        /// <summary>
        /// Obtient un utilisateur par son ID.
        /// </summary>
        /// <param name="id">L'ID de l'utilisateur à obtenir.</param>
        /// <returns>L'utilisateur demandé.</returns>
        [HttpGet("{id}"), Authorize(Roles = "ADMIN")]
        public ActionResult<VotingUsers> GetUser(long id)
        {
            return Ok(new
            {
                data = _usersService.GetUsers(id)
            });
        }

        /// <summary>
        /// Supprime un utilisateur par son ID.
        /// </summary>
        /// <param name="id">L'ID de l'utilisateur à supprimer.</param>
        /// <returns>L'utilisateur supprimé.</returns>
        [HttpDelete("{id}"), Authorize(Roles = "ADMIN")]
        public ActionResult<VotingUsers> DeleteUser(long id)
        {
            return Ok(new
            {
                data = _usersService.DeleteUsers(id)
            });
        }

        /// <summary>
        /// Modifie un utilisateur existant.
        /// </summary>
        /// <param name="user">L'utilisateur à modifier.</param>
        /// <returns>L'utilisateur modifié.</returns>
        [HttpPut, Authorize(Roles = "ADMIN")]
        public ActionResult<VotingArticle> EditUser(VotingUsers user)
        {
            try
            {
                _usersService.EditUser(user.Id, user);
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
        /// Obtient un utilisateur par son adresse e-mail.
        /// </summary>
        /// <returns>L'utilisateur demandé.</returns>
        [HttpGet("email"), Authorize]
        public ActionResult<VotingUsersResponse> GetUserByEmail()
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }

            return Ok(new
            {
                data = _usersService.GetUserBEmail(up[0])
            });
        }

        /// <summary>
        /// Obtient tous les utilisateurs.
        /// </summary>
        /// <returns>Une liste de tous les utilisateurs.</returns>
        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingUsersResponse>> GetUsers()
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
                data = _usersService.GetUsers()
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
