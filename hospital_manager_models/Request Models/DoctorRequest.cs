using System.Collections.Generic;

namespace voting_data_access.Entities
{
    public class DoctorRequest
    {
        public string Username { get; set; }

        public List<long> SpecialityIds { get; set; }

        public List<ConsultationRequest> Consultations { get; set; }


    }
}
