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
        
    }
}