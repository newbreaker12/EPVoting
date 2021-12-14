﻿using voting_bl.Util;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using voting_models.Models;

namespace voting_bl.Service
{
    public class DoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ModelConverter modelConverter;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            modelConverter = new ModelConverter(_unitOfWork);
        }

        public VotingGroupsResponse GetDoctor(string username)
        {
            DoctorData doctorData = _unitOfWork.Doctor.GetDoctor(username);
            return modelConverter.ResponseOf(doctorData);
        }

        public List<VotingGroupsResponse> GetDoctors()
        {
            List<DoctorData> doctorData = _unitOfWork.Doctor.GetDoctors();
            return doctorData?.Select(doctor => modelConverter.ResponseOf(doctor)).ToList();
        }

        public VotingGroupsResponse SaveDoctor(VotingSessionRequest doctorRequest)
        {
            var doctorData = modelConverter.EnvelopeOf(doctorRequest);
            _unitOfWork.Doctor.Add(doctorData);
            _unitOfWork.Save();

            var doctorResponse = modelConverter.ResponseOf(_unitOfWork.Doctor.GetDoctor(doctorData.Username));
            return doctorResponse;
        }
    }
}
