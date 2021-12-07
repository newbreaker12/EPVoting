using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;

namespace voting_data_access.Repositories.Implementation
{
    public class AppointmentRepository : Repository<AppointmentData>, IAppointmentRepository
    {
        public AppointmentRepository(HospitalDbContext context) : base(context) { }
    }
}