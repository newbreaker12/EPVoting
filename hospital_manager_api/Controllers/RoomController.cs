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
    [Route("room")]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly RoomService _roomService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public RoomController(IUnitOfWork unitOfWork)
        {
            _roomService = new RoomService(unitOfWork);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "OK";
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<VotingUsersResponse> SaveRoom(VotingGroupsRequest room)
        {
            try
            {
                var roomResponse = _roomService.SaveRoom(room);
                return Ok(new
                {
                    data = roomResponse
                });
            }
            catch (InvalidRoom e)
            {
                return BadRequest(new
                {
                    data = e.Message
                });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<VotingUsersResponse> GetRoom(long id)
        {
            return Ok(new
            {
                data = _roomService.GetRoom(id)
            });
        }

        [HttpGet("hospital/{id}")]
        public ActionResult<IEnumerable<RoomData>> GetRoomsByHospitalId(long hospitalId)
        {
            return Ok(new
            {
                data = _roomService.GetRoomsByHospitalId(hospitalId)
            });
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<RoomData>> GetRooms()
        {
            return Ok(new
            {
                data = _roomService.GetRooms()
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