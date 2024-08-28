﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using voting_bl.Service;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using voting_models.Response_Models;
using System.Security.Claims;
using voting_bl.Mapper;

namespace voting_api.Controllers
{
    /// <summary>
    /// Contrôleur pour gérer les demandes liées aux votes.
    /// </summary>
    [Produces("application/json")]
    [Route("vote")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly VoteService _voteService;
        private readonly VotingArticleService _votingArticle;
        private readonly EmailsService _emailsService;
        private readonly VotingUsersService _usersService;
        private readonly VoteMapper _voteMapper;

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
            _voteMapper = new VoteMapper(unitOfWork);
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
        /// Enregistre un nouveau vote.
        /// </summary>
        /// <param name="vote">Le vote à enregistrer.</param>
        /// <returns>Le vote enregistré.</returns>
        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public ActionResult<Vote> SaveVote(Vote vote)
        {
            var email = User.Identity.Name;
            var userRole = _usersService.getRole(email);
            if (userRole.Name != "MEP")
            {
                return Unauthorized();
            }
            try
            {
                _voteService.SaveVote(vote);
                return Ok(new { data = "ok" });
            }
            catch (InvalidVote e)
            {
                return BadRequest(new { data = e.Message });
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
            var email = User.Identity.Name;
            var userRole = _usersService.getRole(email);
            if (userRole.Name != "MEP")
            {
                return Unauthorized();
            }

            if (_voteService.HasSubmittedVoteArticle(email, id))
            {
                return BadRequest(new { data = "ALREADY SUBMITTED VOTES" });
            }

            _voteService.SaveVote(new Vote() { SubArticleId = id, UserEmail = email, Type = type });
            return Ok(new { data = "VOTED" });
        }

        /// <summary>
        /// Soumet un vote pour un article.
        /// </summary>
        /// <param name="id">L'ID de l'article.</param>
        /// <returns>Le vote soumis.</returns>
        [HttpGet("article/{id}/vote/submit")]
        public ActionResult<Vote> VoteSubmit(long id)
        {
            var email = User.Identity.Name;
            var userRole = _usersService.getRole(email);
            if (userRole.Name != "MEP")
            {
                return Unauthorized();
            }

            if (_voteService.HasSubmittedVoteArticle(email, id))
            {
                return BadRequest(new { data = "ALREADY SUBMITTED VOTES" });
            }

            var votingArticle = _votingArticle.GetArticle(id);
            if (votingArticle == null)
            {
                return BadRequest(new { data = "Article not found" });
            }

            var user = _usersService.GetUserDataByEmail(email);
            byte[] file = PdfFileGenerator.GeneratePdf(votingArticle, user.PinCode, user.FirstName + " " + user.LastName);
            _emailsService.SendEmail(email, "Vote submitted: " + votingArticle.Description, "Please find attached", file);

            _voteService.SaveVoteSubmit(new VoteSubmit() { ArticleId = id, UserEmail = email });
            return Ok(new { data = "VOTED" });
        }

        /// <summary>
        /// Obtient un vote par son ID.
        /// </summary>
        /// <param name="id">L'ID du vote à obtenir.</param>
        /// <returns>Le vote demandé.</returns>
        [HttpGet("{id}")]
        public ActionResult<Vote> GetVote(long id)
        {
            var email = User.Identity.Name;
            var userRole = _usersService.getRole(email);
            if (userRole.Name != "ADMIN")
            {
                return Unauthorized();
            }

            return Ok(new { data = _voteService.GetVote(id) });
        }

        /// <summary>
        /// Obtient tous les votes.
        /// </summary>
        /// <returns>Une liste de tous les votes.</returns>
        [HttpGet("all")]
        public ActionResult<IEnumerable<VoteSearchResponse>> GetVotes()
        {
            var email = User.Identity.Name;
            var userRole = _usersService.getRole(email);
            if (userRole.Name != "ADMIN")
            {
                return Unauthorized();
            }

            var searchList = new List<VoteSearchResponse>();
            foreach (var source in _voteService.GetAllVotes())
            {
                searchList.Add(_voteMapper.Map(source));
            }

            return Ok(new { data = searchList });
        }
    }
}
