using voting_data_access.Entities;
using System.Collections.Generic;

namespace voting_data_access.Repositories.Interfaces
{
    public interface IHospitalRepository : IRepository<HospitalData>
    {
        HospitalData GetHospital(long id);

        List<HospitalData> GetHospitals();
    }
}
