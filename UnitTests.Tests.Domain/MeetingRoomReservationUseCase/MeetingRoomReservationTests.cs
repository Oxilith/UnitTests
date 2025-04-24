using UnitTests.Domain.MeetingRoomReservationUseCase.Entities;
using UnitTests.Domain.MeetingRoomReservationUseCase.Interfaces;
using UnitTests.Domain.MeetingRoomReservationUseCase.Services;
using UnitTests.Domain.MeetingRoomReservationUseCase.ValueObjects;

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
    public void Reservation_Should_Fail_When_Start_Occurs_When_Previous_Reservation_Ends()
    {
        // Arrange
        var room = new MeetingRoom("Gdynia");

        var firstReservationTime = new TimeRange(DateTime.UtcNow.AddHours(2), DateTime.UtcNow.AddHours(3));
        var secondReservationTime = new TimeRange(firstReservationTime.End, firstReservationTime.End.AddHours(1));

        var firstReservation = new Reservation(firstReservationTime);
        var secondReservation = new Reservation(secondReservationTime);

        // Act && Assert
        var result = _reservationService.AddReservation(room, firstReservation);
        Assert.That(result, Is.True, "First reservation should be successful.");

        var conflictingResult = _reservationService.AddReservation(room, secondReservation);
        Assert.That(conflictingResult, Is.False, "Second reservation should fail due to conflict.");
    }

    [Test]
    public void Reservation_Should_Pass_When_Start_Occurs_Right_Before_Or_After_Previous_Reservation_Ends()
    {
        // Arrange
        var room = new MeetingRoom("Gdynia");

        var firstStartTime = DateTime.UtcNow.AddHours(4);
        var firstEndTime = firstStartTime.AddHours(1);

        var firstReservationTime = new TimeRange(firstStartTime, firstEndTime);
        var rightAfterReservationTime = new TimeRange(firstEndTime.AddMilliseconds(1), firstEndTime.AddHours(1));
        var rightBeforeReservationTime = new TimeRange(firstStartTime.AddHours(-1), firstStartTime.AddMilliseconds(-1));

        var firstReservation = new Reservation(firstReservationTime);
        var rightAfterReservation = new Reservation(rightAfterReservationTime);
        var rightBeforeReservation = new Reservation(rightBeforeReservationTime);

        // Act && Assert
        var result = _reservationService.AddReservation(room, firstReservation);
        Assert.That(result, Is.True, "First reservation should be successful.");

        var rightAfterReservationResult = _reservationService.AddReservation(room, rightAfterReservation);
        Assert.That(rightAfterReservationResult, Is.True, "Right after reservation should be successful.");

        var rightBeforeReservationResult = _reservationService.AddReservation(room, rightBeforeReservation);
        Assert.That(rightBeforeReservationResult, Is.True, "Right before reservation should be successful.");
    }

    [Test]
    public void Reservation_Should_Fail_When_Is_Shorter_Than_Room_Allows()
    {
        // Arrange
        var room = new MeetingRoom("Gdynia", TimeBoxLimit.Default());

        var firstStartTime = DateTime.UtcNow.AddHours(4);
        var firstEndTime = firstStartTime.AddMinutes(30).AddMilliseconds(-1);
        var reservation = new Reservation(
            new TimeRange(firstStartTime, firstEndTime));

        // Act
        var result = _reservationService.AddReservation(room, reservation);

        // Assert
        Assert.That(result, Is.False, "Reservation should fail when it is shorter than the room allows.");
    }

    [Test]
    public void Reservation_Should_Fail_When_Is_Longer_Than_Room_Allows()
    {
        // Arrange
        var room = new MeetingRoom("Gdynia");

        var firstStartTime = DateTime.UtcNow.AddHours(4);
        var firstEndTime = firstStartTime.AddMinutes(90).AddMilliseconds(1);
        var reservation = new Reservation(
            new TimeRange(firstStartTime, firstEndTime));

        // Act
        var result = _reservationService.AddReservation(room, reservation);

        // Assert
        Assert.That(result, Is.False, "Reservation should fail when it is longer than the room allows.");
    }

    [Test]
    public void Reservation_Should_Fail_When_Is_Made_To_Start_When_Requested()
    {
        // Arrange
        var requestedStartTime = DateTime.UtcNow.AddHours(4);
        var endTime = requestedStartTime.AddHours(1);

        // Act
        Action act = () => new Reservation(new TimeRange(requestedStartTime, endTime), requestedStartTime);

        // Assert
        Assert.Throws<InvalidOperationException>(act.Invoke,
            "Reservation instantiation should throw creation timestamp is before or equals to start time.");
    }
}