using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace voting_data_access.Repositories.Implementation
{
    public class DoctorRepository : Repository<DoctorData>, IDoctorRepository
    {
        public DoctorRepository(HospitalDbContext context) : base(context) { }

        public DoctorData GetDoctor(string username)
        {
            return Db.DoctorData.Include(doctor => doctor.Specialities).Include(doctor => doctor.Consultations).Single(doctor => doctor.Username == username);
        }
        public List<DoctorData> GetDoctors()
        {
            return Db.DoctorData.Include(doctor => doctor.Specialities).Include(doctor => doctor.Consultations).ToList();
        }
    }
}