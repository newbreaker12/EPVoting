using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
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
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly byte[] _salt;
        private readonly string accountSid;
        private readonly string smsAuthToken;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="UsersController"/>.
        /// </summary>
        /// <param name="unitOfWork">L'unité de travail à utiliser par les services.</param>
        /// <param name="configuration">La configuration de l'application.</param>
        public UsersController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _usersService = new VotingUsersService(unitOfWork, _configuration);
            _emailsService = new EmailsService(unitOfWork);
            _votingGroupsService = new VotingGroupsService(unitOfWork);
            _votingRolesService = new VotingRolesService(unitOfWork);
            _tokenHandler = new JwtSecurityTokenHandler();

            // Retrieve the salt (token) from the configuration
            var token = _configuration["AppSettings:Token"];
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is not configured in appsettings.json");
            }
            _salt = Encoding.ASCII.GetBytes(token);
            accountSid = _configuration["AppSettings:SMSAccountSid"];
            smsAuthToken = _configuration["AppSettings:SMSAuthToken"];
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
            VotingUsersResponse userDTO = _usersService.GetUserByEmail(request.Email);
            VotingUsers user = _usersService.GetUserDataByEmail(request.Email);

            if (user == null)
            {
                return Unauthorized("Wrong username and password");
            }

            if (!VerifyPasswordHash(request.Password, userDTO.Password))
            {
                return Unauthorized("Wrong password.");
            }

            string token = CreateToken(userDTO);
            user.AccessToken = token;
            user.TokenExpires = DateTime.UtcNow.AddMinutes(36000);
            user.TokenCreated = DateTime.UtcNow;
            _usersService.SaveUsers(user);
            return Ok(new { token });
        }

        private string CreateToken(VotingUsersResponse user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("email", user.Email),
                new Claim("firtName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("role", user.Role.Name),
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
                var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return computedHash.Equals(passwordHash);
            }
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur.
        /// </summary>
        /// <param name="user">L'utilisateur à enregistrer.</param>
        /// <returns>L'utilisateur enregistré.</returns>
        [HttpPost, Authorize(Roles = "ADMIN")]
        public ActionResult<VotingUsers> SaveUsers(VotingUsers user)
        {
            var userGroup = _votingGroupsService.GetGroups(user.GroupId);
            if (userGroup == null)
            {
                return BadRequest("Group doesn't exist");
            }
            try
            {
                var passwordUnhashed = user.Password;
                _usersService.AddUsers(user);
                GetPinCode(user.Email);
                SendSMS(user.Email, "Your password is " + passwordUnhashed);
                _emailsService.SendEmail(user.Email, "Account Created", "User has been created: " + user.Email);
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
            var user = _usersService.GetUsers(id);
            user.Password = null;
            return Ok(new
            {
                data = user
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
                var passwordUnhashed = user.Password;
                user.Password = CreatePasswordHash(user.Password); // Hash the password
                _usersService.EditUser(user.Id, user);
                SendSMS(user.Email, "Your password is " + passwordUnhashed);
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
            string email = GetClaim("email");
            return Ok(new
            {
                data = _usersService.GetUserByEmail(email)
            });
        }

        [HttpGet("pincode"), Authorize]
        public void GetPinCode()
        {
            string email = GetClaim("email");
            GetPinCode(email);
        }

        public void GetPinCode(string email)
        {
            VotingUsers user = _usersService.GetUserDataByEmail(email);

            string pincode = _usersService.updateAndGetPincode(user);


            TwilioClient.Init(accountSid, smsAuthToken);

            var call = MessageResource.Create(
                to: new PhoneNumber(user.PhoneNumber),
                from: new PhoneNumber("+15162104237"),
                body: "Your pincode is " + pincode
            );
        }

        [HttpGet("sendSMS"), Authorize(Roles = "ADMIN")]
        public void SendSMS(string email, string text)
        {
            VotingUsers user = _usersService.GetUserDataByEmail(email);

            string pincode = _usersService.updateAndGetPincode(user);


            TwilioClient.Init(accountSid, smsAuthToken);

            var call = MessageResource.Create(
                to: new PhoneNumber(user.PhoneNumber),
                from: new PhoneNumber("+15162104237"),
                body: "Your password is " + text
            );
        }

        /// <summary>
        /// Obtient tous les utilisateurs.
        /// </summary>
        /// <returns>Une liste de tous les utilisateurs.</returns>
        [HttpGet("all"), Authorize(Roles = "ADMIN")]
        public ActionResult<IEnumerable<VotingUsersResponse>> GetUsers()
        {
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
