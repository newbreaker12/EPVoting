using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;

namespace voting_data_access.Repositories.Implementation
{
    public class VoteRepository : Repository<Vote>, IAppointmentRepository
    {
        public VoteRepository(VotingDbContext context) : base(context) { }
    }
}