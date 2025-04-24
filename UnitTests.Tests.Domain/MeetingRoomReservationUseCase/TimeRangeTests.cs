using UnitTests.Domain.MeetingRoomReservationUseCase.ValueObjects;

namespace UnitTests.Tests.Domain.MeetingRoomReservationUseCase;

public class TimeRangeTests
{
    [Test]
    public void TimeRange_Should_Create_When_EndDate_After_StartDate()
    {
        Action act = () => new TimeRange(DateTime.UtcNow.AddHours(2), DateTime.UtcNow.AddHours(3));

        Assert.DoesNotThrow(act.Invoke, "Time range should not throw when start date is before end date.");
    }

    [Test]
    public void TimeRange_Should_Throw_When_EndDate_Before_StartDate()
    {
        Action act = () => new TimeRange(DateTime.UtcNow.AddHours(3), DateTime.UtcNow.AddHours(2));

        Assert.Throws<InvalidOperationException>(act.Invoke,
            "Time range should throw when end date is before start date.");
    }
}