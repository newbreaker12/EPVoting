using voting_data_access.Entities;
using Microsoft.EntityFrameworkCore;

namespace voting_data_access.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
        {
        }

        public DbSet<AppointmentData> AppointmentData { get; set; }
        public DbSet<ConsultationData> ConsultationData { get; set; }
        public DbSet<DoctorData> DoctorData { get; set; }
        public DbSet<HospitalData> HospitalData { get; set; }
        public DbSet<RoomData> RoomData { get; set; }
        public DbSet<SpecialityData> SpecialityData { get; set; }
        public DbSet<SpecialityToDoctorData> SpecialityToDoctorData { get; set; }
        public DbSet<SpecialityToRoomData> SpecialityToRoomData { get; set; }
    }
}
