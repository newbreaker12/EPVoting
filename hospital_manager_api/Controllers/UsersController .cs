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

namespace voting_api.Controllers
{
    [Produces("application/json")]
    [Route("hospital")]
    [ApiController]
    public class HospitalController : Controller
    {
    }
}