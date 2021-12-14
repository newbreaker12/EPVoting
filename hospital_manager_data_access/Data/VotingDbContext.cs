using voting_data_access.Entities;
using Microsoft.EntityFrameworkCore;

namespace voting_data_access.Data
{
    public class VotingDbContext : DbContext
    {
        public VotingDbContext(DbContextOptions<VotingDbContext> options) : base(options)
        {
        }

        public DbSet<VotingArticle> VotingArticle { get; set; }
        public DbSet<VotingGroups> VotingGroups { get; set; }
        public DbSet<VotingRoles> VotingRoles { get; set; }
        public DbSet<VotingSession> VotingSession { get; set; }
        public DbSet<VotingUsers> VotingUsers { get; set; }
        public DbSet<Vote> Vote { get; set; }
        
    }
}
