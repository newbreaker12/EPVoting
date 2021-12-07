﻿using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace voting_data_access.Repositories.Implementation
{
    public class HospitalRepository : Repository<HospitalData>, IHospitalRepository
    {
        public HospitalRepository(HospitalDbContext context) : base(context) { }

        public HospitalData GetHospital(long id)
        {
            return Db.HospitalData.Include(hospital => hospital.Address).Include(hospital => hospital.OpeningHours).Single(hospital => hospital.Id == id);
        }
        public List<HospitalData> GetHospitals()
        {
            return Db.HospitalData.Include(hospital => hospital.Address).Include(hospital => hospital.OpeningHours).ToList();
        }

    }
}