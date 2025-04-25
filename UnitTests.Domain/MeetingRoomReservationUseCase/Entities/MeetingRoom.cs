using UnitTests.Domain.MeetingRoomReservationUseCase.ValueObjects;

namespace UnitTests.Domain.MeetingRoomReservationUseCase.Entities;

public class MeetingRoom
{
    private readonly List<MeetingRoomReservation> _reservations = new();

    public MeetingRoom(string name, TimeBoxLimit? limit = null)
    {
        Name = name;
        TimeBoxLimit = limit ?? TimeBoxLimit.Default();

        Id = Guid.NewGuid();
    }

    public string Name { get; }
    public TimeBoxLimit TimeBoxLimit { get; }
    public Guid Id { get; }

    public IReadOnlyCollection<MeetingRoomReservation> Reservations => _reservations;

    public bool AddReservation(MeetingRoomReservation meetingRoomReservation)
    {
        var succeeded = false;
        var durationTotalMinutes = meetingRoomReservation.Duration.TotalMinutes;

        if (!meetingRoomReservation.IsInThePast &&
            durationTotalMinutes >= TimeBoxLimit.Min &&
            durationTotalMinutes <= TimeBoxLimit.Max &&
            !_reservations.Any(x => x.Overlaps(meetingRoomReservation)))
        {
            _reservations.Add(meetingRoomReservation);
            succeeded = true;
        }

        return succeeded;
    }
}