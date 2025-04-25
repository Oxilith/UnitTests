using UnitTests.Domain.MeetingRoomReservationUseCase.ValueObjects;

namespace UnitTests.Domain.MeetingRoomReservationUseCase.Entities;

public class MeetingRoomReservation
{
    public MeetingRoomReservation(TimeRange time, DateTime? creationTimestamp = null)
    {
        CreationTimestamp = creationTimestamp ?? DateTime.UtcNow;
        if (time.Start <= creationTimestamp)
            throw new InvalidOperationException("Reservation start time cannot be set to time when requested.");

        Id = Guid.NewGuid();
        Time = time;
    }

    public DateTime CreationTimestamp { get; }
    public Guid Id { get; }
    public TimeRange Time { get; }
    public bool IsInThePast => Time.Start < DateTime.UtcNow;

    public TimeSpan Duration => Time.End - Time.Start;

    public bool Overlaps(MeetingRoomReservation meetingRoomReservation)
    {
        return meetingRoomReservation.Id == Id || Time.Overlaps(meetingRoomReservation.Time);
    }
}