using voting_data_access.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace voting_data_access.Data
{
    public class VotingDbContext : DbContext
    {
        public VotingDbContext(DbContextOptions<VotingDbContext> options) : base(options)
        {
        }

        public DbSet<VotingArticle> VotingArticle { get; set; }
        public DbSet<VotingSubArticle> VotingSubArticle { get; set; }
        public DbSet<VotingGroups> VotingGroups { get; set; }
        public DbSet<VotingRoles> VotingRoles { get; set; }
        public DbSet<VotingSession> VotingSession { get; set; }
        public DbSet<VotingUsers> VotingUsers { get; set; }
        public DbSet<Vote> Vote { get; set; }
        public DbSet<VoteSubmit> VoteSubmit { get; set; }
        public DbSet<VoteStatistics> VoteStatistics { get; set; }

        public async Task<List<VoteStatistics>> GetVoteStatisticsAsync()
        {
            return await VoteStatistics
                .FromSqlRaw("SELECT * FROM VoteStatistics")
                .ToListAsync();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VoteStatistics>().HasNoKey();
        }
    }
}
