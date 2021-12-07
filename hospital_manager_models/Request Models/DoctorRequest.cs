using System.Collections.Generic;

namespace voting_models.Models
{
    public class DoctorRequest
    {
        public string Username { get; set; }

        public List<long> SpecialityIds { get; set; }

        public List<ConsultationRequest> Consultations { get; set; }


    }
}
