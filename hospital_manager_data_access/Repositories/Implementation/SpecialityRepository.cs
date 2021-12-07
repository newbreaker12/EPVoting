﻿using voting_data_access.Data;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace voting_data_access.Repositories.Implementation
{
    public class SpecialityRepository : Repository<SpecialityData>, ISpecialityRepository
    {
        public SpecialityRepository(HospitalDbContext context) : base(context) { }

        public List<SpecialityData> GetSpecialities(List<long> specialityIds)
        {
            return Db.SpecialityData.Where(speciality => specialityIds.Contains(speciality.Id)).ToList();
        }

        public SpecialityData GetSpecialityByName(string name)
        {
            return Db.SpecialityData.Where(speciality => speciality.Name == name).SingleOrDefault();
        }
    }
}