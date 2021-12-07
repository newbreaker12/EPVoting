using voting_data_access.Entities;
using System.Collections.Generic;

namespace voting_data_access.Repositories.Interfaces
{
    public interface ISpecialityRepository : IRepository<SpecialityData>
    {
        List<SpecialityData> GetSpecialities(List<long> specialityIds);

        SpecialityData GetSpecialityByName(string name);

    }
}
