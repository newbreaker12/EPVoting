using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_models.Models;
using System.Collections.Generic;
using System.Linq;

namespace voting_bl.Util
{
    public class ModelConverter
    {
        private readonly IUnitOfWork _unitOfWork;

        public ModelConverter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //public AppointmentData EnvelopeOf(VoteRequest appointment)
        //{
        //    return new AppointmentData
        //    {
        //        Id = appointment.Id,
        //        PatientUsername = appointment.PatientUsername,
        //        DoctorUsername = appointment.DoctorUsername,
        //        RoomId = appointment.RoomId,
        //        HospitalId = appointment.HospitalId,
        //        Duration = appointment.Duration,
        //        Description = appointment.Description,
        //        From = appointment.From,
        //        To = appointment.To
        //    };
        //}
        //public DoctorData EnvelopeOf(VotingSessionRequest doctorRequest)
        //{
        //    return new DoctorData
        //    {
        //        Username = doctorRequest.Username,
        //        Consultations = EnvelopeOf(doctorRequest.Consultations),
        //        Specialities = EnvelopeOfSpecialityToDoctor(doctorRequest.SpecialityIds)
        //    };
        //}
        //public VotingGroupsResponse ResponseOf(DoctorData doctorData)
        //{
        //    var doctorResponse = new VotingGroupsResponse
        //    {
        //        Username = doctorData.Username,
        //        Consultations = ResponseOf(doctorData.Consultations)
        //    };
        //    doctorResponse.Specialities = ResponseOf(
        //        _unitOfWork.Speciality.GetSpecialities(doctorData.Specialities?.Select(speciality => speciality.SpecialityId).ToList())
        //        );
        //    return doctorResponse;
        //}
        //public VotingSessionResponse ResponseOf(HospitalData hospitalData)
        //{
        //    var hospitalResponse = new VotingSessionResponse
        //    {
        //        Id = hospitalData.Id,
        //        Name = hospitalData.Name,
        //        Address = ResponseOf(hospitalData.Address),
        //        OpeningHours = ResponseOf(hospitalData.OpeningHours)

        //    };
        //    return hospitalResponse;
        //}
        //public AddressResponse ResponseOf(AddressData addressData)
        //{
        //    return new AddressResponse
        //    {
        //        Id = addressData.Id,
        //        Street = addressData.Street,
        //        City = addressData.City,
        //        Country = addressData.Country,
        //        PostalCode = addressData.PostalCode,
        //        BoxNumber = addressData.BoxNumber

        //    };
        //}
        //public List<OpeningHoursResponse> ResponseOf(List<OpeningHoursData> openingHoursData)
        //{
        //    return openingHoursData?.Select(openingHours => new OpeningHoursResponse()
        //    {
        //        Id = openingHours.Id,
        //        Day = openingHours.Day,
        //        Closed = openingHours.Closed,
        //        HourFrom = openingHours.HourFrom,
        //        MinuteFrom = openingHours.MinuteFrom,
        //        HourTo = openingHours.HourTo,
        //        MinuteTo = openingHours.MinuteTo
        //    }).ToList();
        //}
        //public ConsultationData EnvelopeOf(VotingUsersRequest consultationRequest)
        //{
        //    return new ConsultationData
        //    {
        //        Id = consultationRequest.Id,
        //        HospitalId = consultationRequest.HospitalId,
        //        SpecialityId = consultationRequest.SpecialityId,
        //        Duration = consultationRequest.Duration
        //    };
        //}
        //public List<ConsultationData> EnvelopeOf(List<VotingUsersRequest> consultationRequest)
        //{
        //    return consultationRequest?.Select(consultation => EnvelopeOf(consultation)).ToList();
        //}
        //public VotingArticleResponse ResponseOf(ConsultationData consultationData)
        //{
        //    VotingSessionResponse hospital = ResponseOf(_unitOfWork.Hospital.GetHospital(consultationData.HospitalId));
        //    VotingRolesResponse speciality = ResponseOf(_unitOfWork.Speciality.Get(consultationData.SpecialityId));
        //    return new VotingArticleResponse
        //    {
        //        Id = consultationData.Id,
        //        Hospital = hospital,
        //        Speciality = speciality,
        //        Duration = consultationData.Duration
        //    };
        //}
        //public List<VotingArticleResponse> ResponseOf(List<ConsultationData> consultationData)
        //{
        //    return consultationData?.Select(consultation => ResponseOf(consultation)).ToList();
        //}
        //public RoomData EnvelopeOf(VotingGroupsRequest roomRequest)
        //{
        //    return new RoomData
        //    {
        //        Id = roomRequest.Id,
        //        HospitalId = roomRequest.HospitalId,
        //        Name = roomRequest.Name,
        //        Specialities = EnvelopeOfSpecialityToRoom(roomRequest.SpecialityIds)
        //    };
        //}
        //public VotingUsersResponse ResponseOf(RoomData roomData)
        //{
        //    var roomResponse = new VotingUsersResponse
        //    {
        //        Id = roomData.Id,
        //        Name = roomData.Name
        //    };
        //    roomResponse.Specialities = ResponseOf(
        //        _unitOfWork.Speciality.GetSpecialities(roomData.Specialities?.Select(speciality => speciality.SpecialityId).ToList())
        //        );
        //    return roomResponse;
        //}
        //public List<VotingUsersResponse> ResponseOf(List<RoomData> roomDatas)
        //{
        //    return roomDatas?.Select(roomData => ResponseOf(roomData)).ToList();
        //}

        //public VotingRolesResponse ResponseOf(SpecialityData speciality)
        //{
        //    return new VotingRolesResponse
        //    {
        //        Id = speciality.Id,
        //        Name = speciality.Name
        //    };
        //}

        //public HospitalData EnvelopeOf(HospitalRequest hospital)
        //{
        //    return new HospitalData
        //    {
        //        Id = hospital.Id,
        //        Name = hospital.Name,
        //        Address = EnvelopeOf(hospital.Address),
        //        OpeningHours = hospital.OpeningHours?.Select(openingHours => new OpeningHoursData()
        //        {
        //            Id = openingHours.Id,
        //            Day = openingHours.Day == null ? null : openingHours.Day,
        //            HourFrom = openingHours.HourFrom,
        //            HourTo = openingHours.HourTo,
        //            MinuteFrom = openingHours.MinuteFrom,
        //            MinuteTo = openingHours.MinuteTo,
        //            Closed = openingHours.Closed
        //        }).ToList()
        //    };
        //}
        //public SpecialityData EnvelopeOf(VotingArticleRequest speciality)
        //{
        //    return new SpecialityData
        //    {
        //        Id = speciality.Id,
        //        Name = speciality.Name
        //    };
        //}
        //public AddressData EnvelopeOf(AddressRequest address)
        //{
        //    return new AddressData
        //    {
        //        Id = address.Id,
        //        PostalCode = address.PostalCode,
        //        City = address.City,
        //        Country = address.Country,
        //        Street = address.Street,
        //        BoxNumber = address.BoxNumber
        //    };
        //}
        //public List<VotingRolesResponse> ResponseOf(List<SpecialityData> specialities)
        //{
        //    return specialities?.Select(speciality => new VotingRolesResponse()
        //    {
        //        Id = speciality.Id,
        //        Name = speciality.Name
        //    }).ToList();
        //}
        //public List<SpecialityToDoctorData> EnvelopeOfSpecialityToDoctor(List<long> specialityIds)
        //{
        //    return specialityIds?.Select(specialityId => new SpecialityToDoctorData()
        //    {
        //        SpecialityId = specialityId
        //    }).ToList();
        //}
        //public List<SpecialityToRoomData> EnvelopeOfSpecialityToRoom(List<long> specialityIds)
        //{
        //    return specialityIds?.Select(specialityId => new SpecialityToRoomData()
        //    {
        //        SpecialityId = specialityId
        //    }).ToList();
        //}
    }
}
