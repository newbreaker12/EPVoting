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
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using voting_models.Models;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace voting_api.Controllers
{
    [Produces("application/json")]
    [Route("doctor")]
    [ApiController]
    public class GroupsController : Controller
    {

    }
}