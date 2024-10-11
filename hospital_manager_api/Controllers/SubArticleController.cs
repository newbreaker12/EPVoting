using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using voting_bl.Mapper;
using voting_bl.Service;
using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using System.Linq;

namespace voting_api.Controllers
{
    /// <summary>
    /// Controller for managing requests related to sub-articles.
    /// </summary>
    [Produces("application/json")]
    [Route("subarticle")]
    [ApiController]
    public class SubArticleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly VotingArticleService _articleService;
        private readonly VotingSubArticleService _votingSubArticleService;
        private readonly VotingUsersService _usersService;
        private readonly VoteMapper _voteMapper;
        private readonly StatisticsService _statisticsService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubArticleController"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work to use for services.</param>
        /// <param name="votingDbContext">The voting database context.</param>
        /// /// <param name="configuration">The application configuration settings.</param>
        public SubArticleController(IUnitOfWork unitOfWork, VotingDbContext votingDbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _votingSubArticleService = new VotingSubArticleService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork, configuration);
            _articleService = new VotingArticleService(unitOfWork);
            _voteMapper = new VoteMapper(unitOfWork);
            _statisticsService = new StatisticsService(votingDbContext);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// A simple ping method to check if the controller is responsive.
        /// </summary>
        /// <returns>A string response "OK".</returns>
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("OK");
        }

        /// <summary>
        /// Gets the vote statistics.
        /// </summary>
        /// <returns>A list of vote statistics.</returns>
        [HttpGet("statistics")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<VoteStatistics>>> GetStatistics()
        {
            var results = await _statisticsService.GetVoteStatisticsAsync();
            return Ok(new { data = results });
        }

        /// <summary>
        /// Gets the vote statistics.
        /// </summary>
        /// <returns>A list of vote statistics.</returns>
        [HttpGet("statistics/users")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<UserStatistics>>> GetUserStatistics()
        {
            var results = await _statisticsService.GetVoteUserStatisticsAsync();
            return Ok(new { data = results });
        }

        /// <summary>
        /// Saves a new sub-article.
        /// </summary>
        /// <param name="subArticle">The sub-article to save.</param>
        /// <returns>The result of the save operation.</returns>
        [HttpPost]
        [Authorize(Roles = "ADMIN,PG")]
        public ActionResult SaveSubArticle([FromBody] VotingSubArticle subArticle)
        {
            var ar = _articleService.GetArticle(subArticle.ArticleId);
            try
            {
                _votingSubArticleService.SaveSubArticle(subArticle);
                return Ok(new { data = "ok" });
            }
            catch (InvalidVote e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        /// <summary>
        /// Updates an existing sub-article.
        /// </summary>
        /// <param name="subArticle">The sub-article to update.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPut]
        [Authorize(Roles = "ADMIN,PG")]
        public ActionResult UpdateSubArticle([FromBody] VotingSubArticle subArticle)
        {
            try
            {
                _votingSubArticleService.EditSubArticle(subArticle);
                return Ok(new { data = "ok" });
            }
            catch (InvalidVote e)
            {
                return BadRequest(new { data = e.Message });
            }
        }

        /// <summary>
        /// Gets sub-articles by article ID for admin.
        /// </summary>
        /// <param name="id">The article ID.</param>
        /// <returns>A list of sub-articles for the specified article.</returns>
        [HttpGet("admin/article/{id}")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<List<VotingSubArticle>> GetSubAsByArticleIdForAdmin(long id)
        {
            return Ok(new { data = _votingSubArticleService.GetSubAsByArticleId(id) });
        }

        /// <summary>
        /// Gets sub-articles by article ID for the user.
        /// </summary>
        /// <param name="id">The article ID.</param>
        /// <returns>A list of sub-articles for the specified article.</returns>
        [HttpGet("article/{id}")]
        [Authorize(Roles = "ADMIN,PG")]
        public ActionResult<List<VotingSubArticleResponse>> GetSubAsByArticleIdForUser(long id)
        {
            string email = GetClaim("email");
            var sar = _votingSubArticleService.GetSubArticleById(id);
            var ar = _articleService.GetArticle(sar.ArticleId);
            return Ok(new { data = _votingSubArticleService.GetSubAsByArticleIdAndEmail(id, email) });
        }

        /// <summary>
        /// Deletes a sub-article by its ID.
        /// </summary>
        /// <param name="id">The sub-article ID.</param>
        /// <returns>The result of the delete operation.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,PG")]
        public ActionResult Delete(long id)
        {
            var sar = _votingSubArticleService.GetSubArticleById(id);
            var ar = _articleService.GetArticle(sar.ArticleId);
            try
            {
                _votingSubArticleService.Delete(id);
                return Ok(new { data = "ok" });
            }
            catch (InvalidVote e)
            {
                return BadRequest(new { data = e.Message });
            }
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
