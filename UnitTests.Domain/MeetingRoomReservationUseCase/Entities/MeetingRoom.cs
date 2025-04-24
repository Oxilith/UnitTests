using UnitTests.Domain.MeetingRoomReservationUseCase.ValueObjects;

namespace UnitTests.Domain.MeetingRoomReservationUseCase.Entities;

public class MeetingRoom
{
    private readonly List<Reservation> _reservations = new();

    public MeetingRoom(string name, TimeBoxLimit? limit = null)
    {
        Name = name;
        TimeBoxLimit = limit ?? TimeBoxLimit.Default();

        Id = Guid.NewGuid();
    }

    public string Name { get; }
    public TimeBoxLimit TimeBoxLimit { get; }
    public Guid Id { get; }

    public IReadOnlyCollection<Reservation> Reservations => _reservations;

    public bool AddReservation(Reservation reservation)
    {
        var succeeded = false;
        var durationTotalMinutes = reservation.Duration.TotalMinutes;

        if (!reservation.IsInThePast &&
            durationTotalMinutes >= TimeBoxLimit.Min &&
            durationTotalMinutes <= TimeBoxLimit.Max &&
            !_reservations.Any(x => x.Overlaps(reservation)))
        {
            _reservations.Add(reservation);
            succeeded = true;
        }

        return succeeded;
    }
}