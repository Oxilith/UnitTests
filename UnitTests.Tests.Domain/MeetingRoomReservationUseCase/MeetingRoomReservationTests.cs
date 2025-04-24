namespace UnitTests.Tests.Domain.MeetingRoomReservationUseCase;

public class MeetingRoomReservationTests
{
    private IReservationService _reservationService;

    [SetUp]
    public void SetUp()
    {
        _reservationService = new ReservationService();
    }

    [Test]
    public void Reservation_Should_Be_Correct_When_No_Conflicts()
    {
        // Arrange
        var room = new MeetingRoom("Gdynia");
        var reservation = new Reservation(
            new TimeRange(DateTime.UtcNow.AddHours(2), DateTime.UtcNow.AddHours(3)));

        // Act
        var result = _reservationService.AddReservation(room, reservation);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Reservation_Should_Fail_When_Occurs_In_The_Past()
    {
        // Arrange
        var room = new MeetingRoom("Gdynia");
        var reservation = new Reservation(
            new TimeRange(DateTime.UtcNow.AddHours(-3), DateTime.UtcNow.AddHours(-2)));

        // Act
        var result = _reservationService.AddReservation(room, reservation);

        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Reservation_Should_Fail_When_Conflicts()
    {
        // Arrange
        var room = new MeetingRoom("Gdynia");
        var reservation = new Reservation(
            new TimeRange(DateTime.UtcNow.AddHours(2), DateTime.UtcNow.AddHours(3)));
        
        var conflictingReservation = new Reservation(
            new TimeRange(DateTime.UtcNow.AddHours(2.5), DateTime.UtcNow.AddHours(3.5)));

        // Act && Assert
        var result = _reservationService.AddReservation(room, reservation);
        Assert.That(result, Is.True);   
        
        var conflictingResult = _reservationService.AddReservation(room, conflictingReservation);
        Assert.That(conflictingResult, Is.False);
    }
    
        
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

        Assert.Throws<InvalidOperationException>(act.Invoke, "Time range should throw when end date is before start date.");
    }
}

#region MoveToDomain

// Entity
public class MeetingRoom
{
    private readonly List<Reservation> _reservations = new();

    public MeetingRoom(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
    }

    public string Name { get; }
    public Guid Id { get; }

    public IReadOnlyCollection<Reservation> Reservations => _reservations;

    public bool AddReservation(Reservation reservation)
    {
        var succeeded = false;
        if (!reservation.IsInThePast && !_reservations.Any(x => x.Overlaps(reservation)))
        {
            _reservations.Add(reservation);
            succeeded = true;
        }

        return succeeded;
    }
}

// Entity
public class Reservation
{
    public Reservation(TimeRange time)
    {
        Id = Guid.NewGuid();
        Time = time;
    }

    public Guid Id { get; }
    public TimeRange Time { get; }
    public bool IsInThePast => Time.Start < DateTime.UtcNow;

    public bool Overlaps(Reservation reservation)
    {
        return reservation.Id == Id || Time.Overlaps(reservation.Time);
    }
}

// ValueObject
public class TimeRange
{
    public TimeRange(DateTime start, DateTime end)
    {
        if (start > end)
        {
            throw new InvalidOperationException("End date should not occur before start date.");
        }
        
        Start = start;
        End = end;
    }

    public DateTime Start { get; }
    public DateTime End { get; }

    public bool Overlaps(TimeRange reservationTime)
    {
        return Start < reservationTime.End && End > reservationTime.Start;
    }
}

public class ReservationService : IReservationService
{
    public bool AddReservation(MeetingRoom room, Reservation reservation)
    {
        return room.AddReservation(reservation);
    }
}

public interface IReservationService
{
    bool AddReservation(MeetingRoom room, Reservation reservation);
}

#endregion