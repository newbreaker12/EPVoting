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
    public class VotingSubArticleRepository : Repository<VotingSubArticle>, IVotingSubArticleRepository
    {
        public VotingSubArticleRepository(VotingDbContext context) : base(context) { }

        public List<VotingSubArticle> GetAll()
        {
            return Db.VotingSubArticle.ToList();
        }
        public List<VotingSubArticle> GetSubArticlesByArticleId(long articleId)
        {
            return Db.VotingSubArticle.Where(a => a.ArticleId == articleId).ToList();
        }
        public List<VotingSubArticleResponse> GetSubArticlesByArticleIdAndEmail(long articleId, string email)
        {

            List<VotingSubArticle> subArticles = Db.VotingSubArticle.Where(a => a.ArticleId == articleId).ToList();
            List<VotingSubArticleResponse> result = new List<VotingSubArticleResponse>();
            List<long> ids = subArticles?.Select(s => s.Id).ToList();
            List<Vote> votes = Db.Vote.Where(v => v.UserEmail == email && ids.Contains(v.SubArticleId)).ToList();

            foreach (VotingSubArticle s in subArticles) {
                Vote v = votes.SingleOrDefault(v => v.SubArticleId == s.Id && v.UserEmail == email);
                if (v != null)
                {
                    result.Add(new VotingSubArticleResponse()
                    {
                        Name = s.Name,
                        Id = s.Id,
                        ArticleId = s.ArticleId,
                        Description = s.Description,
                        CreatedAt = s.CreatedAt,
                        VoteType = v.Type
                    });
                } else
                {
                    result.Add(new VotingSubArticleResponse()
                    {
                        Name = s.Name,
                        Id = s.Id,
                        ArticleId = s.ArticleId,
                        Description = s.Description,
                        CreatedAt = s.CreatedAt,
                        VoteType = -1
                    });

                }

            }
            return result;
        }
    }
}