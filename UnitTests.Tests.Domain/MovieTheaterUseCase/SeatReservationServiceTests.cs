using FluentAssertions;
using Moq;
using UnitTests.Domain.MovieTheaterUseCase.Entities;
using UnitTests.Domain.MovieTheaterUseCase.Exceptions;
using UnitTests.Domain.MovieTheaterUseCase.Repositories;
using UnitTests.Domain.MovieTheaterUseCase.Services;

namespace UnitTests.Tests.Domain.MovieTheaterUseCase;

public class SeatReservationServiceTests
{
    private TestDataProvider _dataProvider;
    private Mock<ICinemaHallRepository> _movieTheaterRepositoryMock;
    private SeatReservationService _service;

    [SetUp]
    public void SetUp()
    {
        _movieTheaterRepositoryMock = new Mock<ICinemaHallRepository>();
        _dataProvider = new TestDataProvider();
        _service = new SeatReservationService(_movieTheaterRepositoryMock.Object);
    }

    [Test]
    public void ReservationCannotBeInPast()
    {
        Assert.That(true, Is.False);
    }

    [Test]
    public void ReservationCannotBeMoreThan30DaysInFuture()
    {
        Assert.That(true, Is.False);
    }

    [Test]
    public void CannotAddTwoRowsWithSameNumber()
    {
        // Arrange & Act
        var act = () => CinemaHall.Create(new List<HallRow>
        {
            new(1, 3),
            new(2, 4),
            new(2, 5)
        });

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Cannot add two or more rows with the same number to the cinema hall.");
    }

    [Test]
    public void RowsSeatCountShouldIncreaseWithEachRow()
    {
        // Arrange & Act
        Action act = () => _dataProvider.GetCorrectCinemaHall();

        // Assert
        act.Should().NotThrow();
    }


    [TestCase(1, 1)]
    [TestCase(2, 1)]
    [TestCase(8, 1)]
    [TestCase(9, 1)]
    public void CanCreateRowsThatAreNumberedFrom1To9(int rowNumber, int seatNumber)
    {
        // Arrange & Act
        Action act2 = () => new HallRow(rowNumber, seatNumber);

        // Assert
        act2.Should().NotThrow<BusinessRuleViolationException>();
    }

    [Test]
    public void CannotCreateReservationIfLessThanOneSeat()
    {
        // Arrange
        var theater = CinemaHall.Create(new List<HallRow>
        {
            new(1, 3),
            new(2, 4),
            new(3, 5)
        });

        _movieTheaterRepositoryMock.Setup(x => x.GetCinemaHall(It.IsAny<Guid>()))
            .ReturnsAsync(theater);

        // Act
        Action act = () => _dataProvider.GetEmptyReservation();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("HallReservation must have at least one seat.");
    }

    [Test]
    public void CannotCreateReservationIfMoreThanFiveSeats()
    {
        // Arrange
        var theater = CinemaHall.Create(new List<HallRow>
        {
            new(1, 3),
            new(2, 4),
            new(3, 5)
        });

        _movieTheaterRepositoryMock.Setup(x => x.GetCinemaHall(It.IsAny<Guid>()))
            .ReturnsAsync(theater);

        // Act
        Action act = () => _dataProvider.GetLengthyReservation();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("HallReservation must be between 1 and 5 seats.");
    }

    [Test]
    public void CannotCreateReservationIfNotEnoughSeatsInRow()
    {
        // Arrange
        var theater = CinemaHall.Create(new List<HallRow>
        {
            new(1, 1),
            new(2, 4),
            new(3, 5),
            new(9, 6)
        });

        var reservation = _dataProvider.GetCorrectReservation(theater.Rows.ToList());

        _movieTheaterRepositoryMock.Setup(x => x.GetCinemaHall(It.IsAny<Guid>()))
            .ReturnsAsync(theater);

        // Act
        var act = () => _service.Reserve(theater.Id, reservation).Wait();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Not enough seats available in the row *");
    }
}