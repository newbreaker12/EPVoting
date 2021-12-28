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
    [Produces("application/json")]
    [Route("roles")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly VotingRolesService _rolesService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public RolesController(IUnitOfWork unitOfWork)
        {
            _rolesService = new VotingRolesService(unitOfWork);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingRoles> SaveRoles(VotingRoles roles)
        {
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

        [HttpGet("{id}")]
        public ActionResult<VotingRoles> GetRoles(long id)
        {
            return Ok(new
            {
                data = _rolesService.GetRoles(id)
            });
        }
        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingRoles>> GetRoles()
        {
            return Ok(new
            {
                data = _rolesService.GetRoles()
            });
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