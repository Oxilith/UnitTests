using UnitTests.Domain.MeetingRoomReservationUseCase.Entities;
using UnitTests.Domain.MeetingRoomReservationUseCase.Interfaces;

namespace UnitTests.Domain.MeetingRoomReservationUseCase.Services;

public class ReservationService : IReservationService
{
    public bool AddReservation(MeetingRoom room, Reservation reservation)
    {
        return room.AddReservation(reservation);
    }
}