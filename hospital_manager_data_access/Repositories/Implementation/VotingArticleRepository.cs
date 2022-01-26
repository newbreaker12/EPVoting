using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_models.Models;

namespace voting_data_access.Repositories.Implementation
{
    public class VotingArticleRepository : Repository<VotingArticle>, IVotingArticleRepository
    {
        public VotingArticleRepository(VotingDbContext context) : base(context) { }

        public List<VotingArticleResponse> GetArticles()
        {
            DateTime now = DateTime.Now;
            List<VotingArticle> articles = Db.VotingArticle.ToList();
            List<VotingArticleResponse> result = new List<VotingArticleResponse>();
            foreach (VotingArticle article in articles)
            {
                    VotingGroups group = Db.VotingGroups.SingleOrDefault(s => s.Id == article.GroupsId);
                VotingSession session = Db.VotingSession.SingleOrDefault(s => s.ArticleId == article.Id);
                if (session != null)
                {
                    result.Add(new VotingArticleResponse
                    {
                        Id = article.Id,
                        GroupsId = article.GroupsId,
                        Name = article.Name,
                        Description = article.Description,
                        CreatedAt = article.CreatedAt,
                        Group = new VotingGroupsResponse()
                        {
                            Id = group.Id,
                            Name = group.Name,
                            CreatedAt = group.CreatedAt,
                            ReadableId = group.ReadableId

                        },
                        Session = new VotingSessionResponse()
                        {
                            Id = session.Id,
                            ArticleId = session.ArticleId,
                            Name = session.Name,
                            Description = session.Description,
                            From = session.From,
                            To = session.To,

                        }
                    });
                } else
                {
                    result.Add(new VotingArticleResponse
                    {
                        Id = article.Id,
                        GroupsId = article.GroupsId,
                        Name = article.Name,
                        Description = article.Description,
                        CreatedAt = article.CreatedAt,
                        Group = new VotingGroupsResponse()
                        {
                            Id = group.Id,
                            Name = group.Name,
                            CreatedAt = group.CreatedAt,
                            ReadableId = group.ReadableId

                        }
                    });
                }
            }
            return result;
        }

        public List<VotingArticleResponse> GetArticleForUser(string email)
        {
            DateTime now = DateTime.Now;
            List<VotingArticleResponse> result = new List<VotingArticleResponse>();
            List<long> groupIds = Db.VotingUsers.Include(u => u.Roles).Include(u => u.Groups).SingleOrDefault(u => u.Email == email).Groups?.Select(group => group.GroupId).ToList();

            if (groupIds != null)
            {
                return GetArticleForGroups(groupIds, email);
            }
            else
            {
                return null;
            }
        }

        public List<VotingArticleResponse> GetArticleForGroups(List<long> groupIds, string email)
        {
            DateTime now = DateTime.Now;
            List<VotingArticleResponse> result = new List<VotingArticleResponse>();
            foreach (long groupId in groupIds)
            {
                result.AddRange(GetArticleForGroup(groupId, email));
            }
            return result;
        }

        public List<VotingArticleResponse> GetArticleForGroup(long groupId, string email)
        {
            DateTime now = DateTime.Now;
            List<VotingArticleResponse> result = new List<VotingArticleResponse>();
            List<VotingArticle> articles = Db.VotingArticle.Where(u => u.GroupsId == groupId).ToList();
            foreach (VotingArticle article in articles)
            {
                VotingSession session = Db.VotingSession.SingleOrDefault(s => s.ArticleId == article.Id && s.From <= now && s.To >= now);
                if (session != null)
                {

                    VotingGroups group = Db.VotingGroups.SingleOrDefault(s => s.Id == article.GroupsId);
                    Vote vote = Db.Vote.SingleOrDefault(s => s.ArticleId == article.Id && s.SessionId == session.Id && s.UserEmail == email);
                        string StateVote = vote == null ? "DIDNT VOTE" : "VOTED";

                        result.Add(new VotingArticleResponse
                        {
                            Id = article.Id,
                            GroupsId = article.GroupsId,
                            Name = article.Name,
                            Description = article.Description,
                            CreatedAt = article.CreatedAt,
                            StatusVote = StateVote,
                            Session = new VotingSessionResponse()
                            {
                                Id = session.Id,
                                ArticleId = session.ArticleId,
                                Name = session.Name,
                                Description = session.Description,
                                From = session.From,
                                To = session.To,

                            },
                            Group = new VotingGroupsResponse()
                            {
                                Id = group.Id,
                                Name = group.Name,
                                CreatedAt = group.CreatedAt,
                                ReadableId = group.ReadableId

                            }
                        });
                }
            }
            return result;
        }
    }
}