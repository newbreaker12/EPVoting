using voting_bl.Util;
using voting_data_access.Entities;
using voting_data_access.Repositories.Interfaces;
using voting_exceptions.Exceptions;
using voting_models.Models;
using System.Collections.Generic;
using System.Linq;

namespace voting_bl.Service
{
    public class RoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ModelConverter modelConverter;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            modelConverter = new ModelConverter(_unitOfWork);
        }

        public VotingUsersResponse GetRoom(long id)
        {
            RoomData roomData = _unitOfWork.Room.GetRoom(id);
            return modelConverter.ResponseOf(roomData);
        }

        public List<VotingUsersResponse> GetRooms()
        {
            List<RoomData> roomData = _unitOfWork.Room.GetRooms();
            return roomData?.Select(room => modelConverter.ResponseOf(room)).ToList();
        }
        public List<VotingUsersResponse> GetRoomsByHospitalId(long hospitalId)
        {
            List<RoomData> roomData = _unitOfWork.Room.GetRoomsByHospitalId(hospitalId);
            return roomData?.Select(room => modelConverter.ResponseOf(room)).ToList();
        }

        public VotingUsersResponse SaveRoom(VotingGroupsRequest roomRequest)
        {
            if (!HospitalExists(roomRequest.HospitalId))
            {
                throw new InvalidRoom("Hospital with ID " + roomRequest.HospitalId + " does not exist.");
            }

            var roomData = modelConverter.EnvelopeOf(roomRequest);
            _unitOfWork.Room.Add(roomData);
            _unitOfWork.Save();
            _unitOfWork.Save();

            var roomResponse = modelConverter.ResponseOf(_unitOfWork.Room.GetRoom(roomData.Id));
            return roomResponse;
        }

        private bool HospitalExists(long id)
        {
            return _unitOfWork.Hospital.Get(id) != null;
        }
    }
}
