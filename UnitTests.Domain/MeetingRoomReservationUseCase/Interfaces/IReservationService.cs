using UnitTests.Domain.MeetingRoomReservationUseCase.Entities;

namespace UnitTests.Domain.MeetingRoomReservationUseCase.Interfaces;

public interface IReservationService
{
    bool AddReservation(MeetingRoom room, MeetingRoomReservation meetingRoomReservation);
}