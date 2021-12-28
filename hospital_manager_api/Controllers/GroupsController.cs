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
    [Route("groups")]
    [ApiController]
    public class GroupsController : Controller
    {
        private readonly VotingGroupsService _groupsService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public GroupsController(IUnitOfWork unitOfWork)
        {
            _groupsService = new VotingGroupsService(unitOfWork);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingGroups> SaveGroups(VotingGroups groups)
        {
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

        [HttpGet("{id}")]
        public ActionResult<VotingGroups> GetGroups(long id)
        {
            return Ok(new
            {
                data = _groupsService.GetGroups(id)
            });
        }
        [HttpGet("all")]
        public ActionResult<IEnumerable<VotingGroups>> GetGroups()
        {
            return Ok(new
            {
                data = _groupsService.GetGroups()
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