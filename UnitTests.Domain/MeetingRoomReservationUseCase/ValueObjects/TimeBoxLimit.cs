namespace UnitTests.Domain.MeetingRoomReservationUseCase.ValueObjects;

public class TimeBoxLimit
{
    private TimeBoxLimit(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public int Max { get; }
    public int Min { get; }

    public static TimeBoxLimit Default()
    {
        return new TimeBoxLimit(30, 90);
    }

    public static TimeBoxLimit Zero()
    {
        return new TimeBoxLimit(0, 0);
    }

    public static TimeBoxLimit Custom(int min, int max)
    {
        return new TimeBoxLimit(min, max);
    }
}