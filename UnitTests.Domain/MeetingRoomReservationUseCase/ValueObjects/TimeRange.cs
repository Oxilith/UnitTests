namespace UnitTests.Domain.MeetingRoomReservationUseCase.ValueObjects;

public class TimeRange
{
    public TimeRange(DateTime start, DateTime end)
    {
        if (start > end) throw new InvalidOperationException("End date should not occur before start date.");

        Start = start;
        End = end;
    }

    public DateTime Start { get; }
    public DateTime End { get; }

    public bool Overlaps(TimeRange reservationTime)
    {
        return Start <= reservationTime.End && End >= reservationTime.Start;
    }
}