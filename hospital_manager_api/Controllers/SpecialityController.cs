using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using voting_bl.Service;
using voting_data_access.Entities;
using Microsoft.AspNetCore.Authorization;
using voting_models.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Net.Http.Headers;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;

namespace voting_api.Controllers
{
    [Produces("application/json")]
    [Route("speciality")]
    [ApiController]
    public class SpecialityController : Controller
    {
        private readonly SpecialityService _specialityService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public SpecialityController(IUnitOfWork unitOfWork)
        {
            _specialityService = new SpecialityService(unitOfWork);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingRolesResponse> SaveSpeciality(VotingArticleRequest speciality)
        {
            try
            {
                var specialityResponse = _specialityService.SaveSpeciality(speciality);
                return Ok(new
                {
                    data = specialityResponse
                });
            }
            catch (InvalidSpeciality e)
            {
                return BadRequest(new
                {
                    data = e.Message
                });
            }
        }

        [HttpGet("{id}")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingRolesResponse> GetSpeciality(long id)
        {
            return Ok(new
            {
                data = _specialityService.GetSpeciality(id)
            });
        }

        [HttpGet("all")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<IEnumerable<VotingRolesResponse>> GetSpecialities()
        {
            return Ok(new
            {
                data = _specialityService.GetSpecialities()
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