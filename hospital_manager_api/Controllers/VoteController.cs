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
using voting_models.Models;
using voting_models.Response_Models;
using voting_bl.Mapper;

namespace voting_api.Controllers
{
    /// <summary>
    /// Contrôleur pour gérer les demandes liées aux votes.
    /// </summary>
    [Produces("application/json")]
    [Route("vote")]
    [ApiController]
    public class VoteController : Controller
    {
        private readonly VoteService _voteService;
        private readonly VotingArticleService _votingArticle;
        private readonly EmailsService _emailsService;
        private readonly VotingUsersService _usersService;
        private readonly VoteMapper voteMapper;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="VoteController"/>.
        /// </summary>
        /// <param name="unitOfWork">L'unité de travail à utiliser par les services.</param>
        public VoteController(IUnitOfWork unitOfWork)
        {
            _voteService = new VoteService(unitOfWork);
            _usersService = new VotingUsersService(unitOfWork);
            _votingArticle = new VotingArticleService(unitOfWork);
            _emailsService = new EmailsService(unitOfWork);
            voteMapper = new VoteMapper(unitOfWork);
        }

        /// <summary>
        /// Enregistre un nouveau vote.
        /// </summary>
        /// <param name="vote">Le vote à enregistrer.</param>
        /// <returns>Le vote enregistré.</returns>
        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<Vote> SaveVote(Vote vote)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "MEP")
            {
                return Unauthorized();
            }
            try
            {
                _voteService.SaveVote(vote);
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
        /// Vote pour un sous-article.
        /// </summary>
        /// <param name="id">L'ID du sous-article.</param>
        /// <param name="type">Le type de vote.</param>
        /// <returns>Le vote enregistré.</returns>
        [HttpGet("subarticle/{id}/vote/{type}")]
        public ActionResult<Vote> Vote(long id, int type)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "MEP")
            {
                return Unauthorized();
            }
            string email = GetUsername();

            if (_voteService.hasSubmittedVoteArticle(email, id))
            {
                return BadRequest(new
                {
                    data = "ALREADY SUBMITTED VOTES"
                });
            }

            _voteService.SaveVote(new Vote() { SubArticleId = id, UserEmail = email, Type = type });
            return Ok(new
            {
                data = "VOTED"
            });
        }

        /// <summary>
        /// Soumet un vote pour un article.
        /// </summary>
        /// <param name="id">L'ID de l'article.</param>
        /// <returns>Le vote soumis.</returns>
        [HttpGet("article/{id}/vote/submit")]
        public ActionResult<Vote> VoteSubmit(long id)
        {
            string getAuthentication = GetAuthorization();
            var up = getAuthentication.Split(":");
            if (up.Length != 2 || _usersService.Authenticate(up[0], up[1]).ToString().ToUpper() != "TRUE")
            {
                return Unauthorized();
            }
            var rs = _usersService.getRole(up[0]);
            if (rs.Name != "MEP")
            {
                return Unauthorized();
            }
            string email = GetUsername();

            if (_voteService.hasSubmittedVoteArticle(email, id))
            {
                return BadRequest(new
                {
                    data = "ALREADY SUBMITTED VOTES"
                });
            }
            List<VotingArticleResponse> votingArticle = _votingArticle.GetArticlesForUser(GetUsername());

            if (votingArticle.Count < 1)
            {
                return BadRequest(new
                {
                    data = "No article found"
                });
            }
            byte[] file = PdfFileGenerator.GeneratePdf(votingArticle[0]);
            _emailsService.SendEmail(up[0], "Vote submitted: " + votingArticle[0].Description, "Please find attached", file);

            _voteService.SaveVoteSubmit(new VoteSubmit() { ArticleId = id, UserEmail = email, });
            return Ok(new
            {
                data = "VOTED"
            });
        }

        /// <summary>
        /// Obtient un vote par son ID.
        /// </summary>
        /// <param name="id">L'ID du vote à obtenir.</param>
        /// <returns>Le vote demandé.</returns>
        [HttpGet("{id}")]
        public ActionResult<Vote> GetVote(long id)
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
                data = _voteService.GetVote(id)
            });
        }

        /// <summary>
        /// Obtient tous les votes.
        /// </summary>
        /// <returns>Une liste de tous les votes.</returns>
        [HttpGet("all")]
        public ActionResult<IEnumerable<VoteSearchResponse>> GetVote()
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

            List<VoteSearchResponse> searchList = new List<VoteSearchResponse>();

            foreach (var source in _voteService.GetVote())
            {
                searchList.Add(voteMapper.Map(source));
            }

            return Ok(new
            {
                data = searchList
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
