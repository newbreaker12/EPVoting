using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;

namespace voting_data_access.Repositories.Implementation
{
    public class ConsultationRepository : Repository<ConsultationData>, IConsultationRepository
    {
        public ConsultationRepository(HospitalDbContext context) : base(context) { }
    }
}