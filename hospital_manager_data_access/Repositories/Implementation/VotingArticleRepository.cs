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

        public VotingArticleResponse getArticle(long id)
        {

            VotingArticleResponse result = new VotingArticleResponse();
            VotingArticle article = Db.VotingArticle.SingleOrDefault(s => s.Id == id);
            List<VotingSubArticle> subArticles = Db.VotingSubArticle.Where(s => s.ArticleId == article.Id).ToList();
            VotingSession session = Db.VotingSession.SingleOrDefault(s => s.ArticleId == article.Id);

            VotingGroups group = Db.VotingGroups.SingleOrDefault(s => s.Id == article.GroupsId);

            if (session != null)
            {
                result = new VotingArticleResponse
                {
                    Id = article.Id,
                    GroupsId = article.GroupsId,
                    Name = article.Name,
                    Description = article.Description,
                    CreatedAt = article.CreatedAt,
                    SubArticles = subArticles?.Select(subArticle => new VotingSubArticleResponse()
                    {
                        Id = subArticle.Id,
                        Name = subArticle.Name,
                        ArticleId = subArticle.ArticleId,
                        CreatedAt = subArticle.CreatedAt,
                        Description = subArticle.Description,
                    }).ToList(),
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
                };
            } else
            {
                result = new VotingArticleResponse
                {
                    Id = article.Id,
                    GroupsId = article.GroupsId,
                    Name = article.Name,
                    Description = article.Description,
                    CreatedAt = article.CreatedAt,
                    SubArticles = subArticles?.Select(subArticle => new VotingSubArticleResponse()
                    {
                        Id = subArticle.Id,
                        Name = subArticle.Name,
                        ArticleId = subArticle.ArticleId,
                        CreatedAt = subArticle.CreatedAt,
                        Description = subArticle.Description,
                    }).ToList(),
                    Group = new VotingGroupsResponse()
                    {
                        Id = group.Id,
                        Name = group.Name,
                        CreatedAt = group.CreatedAt,
                        ReadableId = group.ReadableId

                    }
                };

            }
            return result;

        }



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
                }
                else
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
            long groupId = Db.VotingUsers.SingleOrDefault(u => u.Email == email).GroupId;

            if (groupId != null)
            {
                return GetArticleForGroups(groupId, email);
            }
            else
            {
                return null;
            }
        }

        public List<VotingArticleResponse> GetArticleForGroups(long groupId, string email)
        {
            DateTime now = DateTime.Now;
           return GetArticleForGroup(groupId, email);
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
                    VoteSubmit voteSubmit = Db.VoteSubmit.SingleOrDefault(s => s.ArticleId == article.Id);
                    bool submitted = false;
                    VoteSubmitResponse voteSubmitResponse;
                    if (voteSubmit != null)
                    {
                        submitted = true;
                        voteSubmitResponse = new VoteSubmitResponse()
                        {
                            Id = voteSubmit.Id,
                            UserEmail = voteSubmit.UserEmail,
                            ArticleId = voteSubmit.ArticleId
                        };
                    } else
                    {
                        voteSubmitResponse = null;
                    }

                    VotingArticleResponse votingArticleResponse = new VotingArticleResponse
                    {
                        Id = article.Id,
                        GroupsId = article.GroupsId,
                        Name = article.Name,
                        Description = article.Description,
                        CreatedAt = article.CreatedAt,
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

                        },
                        VoteSubmitResponse = voteSubmitResponse,
                        Submitted = submitted
                    };
                    votingArticleResponse.SubArticles = new List<VotingSubArticleResponse>();

                    List<VotingSubArticle> subArticles = Db.VotingSubArticle.Where(s => s.ArticleId == article.Id).ToList();
                    foreach (VotingSubArticle subArticle in subArticles)
                    {

                        Vote vote = Db.Vote.SingleOrDefault(s => s.SubArticleId == subArticle.Id && s.UserEmail == email);
                        int StateVote = vote == null ? -1 : vote.Type;
                        VotingSubArticleResponse subArticleResponse = new VotingSubArticleResponse()
                        {
                            Id = subArticle.Id,
                            Name = subArticle.Name,
                            ArticleId = subArticle.ArticleId,
                            CreatedAt = subArticle.CreatedAt,
                            Description = subArticle.Description,
                            VoteType = StateVote

                        };
                        votingArticleResponse.SubArticles.Add(subArticleResponse);
                    }

                    result.Add(votingArticleResponse);
                }
            }
            return result;
        }
    }
}