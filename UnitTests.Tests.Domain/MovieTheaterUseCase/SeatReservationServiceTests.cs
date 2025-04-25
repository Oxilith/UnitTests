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
    public void CannotReserveSeatsWithoutSocialDistance()
    {
        // Arrange
        var cinemaHall = _dataProvider.GetIMaxCinemaHall();

        var reservation = _dataProvider.GetBaseReservationForSocialDistanceCheck(cinemaHall.Rows.ToList());
        var failingReservation = _dataProvider.GetReservationForSocialDistanceCheckToFail(cinemaHall.Rows.ToList());

        _movieTheaterRepositoryMock.Setup(x => x.GetCinemaHall(It.IsAny<Guid>()))
            .ReturnsAsync(cinemaHall);

        // Act
        var act = () => _service.Reserve(cinemaHall.Id, reservation).Wait();
        var act2 = () => _service.Reserve(cinemaHall.Id, failingReservation).Wait();

        // Assert
        act.Should().NotThrow();
        act2.Should().Throw<BusinessRuleViolationException>()
            .WithMessage(
                $"Cannot reserve seats without social distance. Minimum distance required {CinemaHall.SocialDistance} seats.");
    }

    [Test]
    public void ReserveSeatsWithSocialDistanceShouldPass()
    {
        // Arrange
        var cinemaHall = _dataProvider.GetIMaxCinemaHall();

        var reservation = _dataProvider.GetBaseReservationForSocialDistanceCheck(cinemaHall.Rows.ToList());
        var failingReservation = _dataProvider.GetReservationForSocialDistanceCheckToSucceed(cinemaHall.Rows.ToList());

        _movieTheaterRepositoryMock.Setup(x => x.GetCinemaHall(It.IsAny<Guid>()))
            .ReturnsAsync(cinemaHall);

        // Act
        var act = () => _service.Reserve(cinemaHall.Id, reservation).Wait();
        var act2 = () => _service.Reserve(cinemaHall.Id, failingReservation).Wait();

        // Assert
        act.Should().NotThrow();
        act2.Should().NotThrow();
    }

    [Test]
    public void CannotReserveAlreadyReservedSeats()
    {
        // Arrange
        var cinemaHall = CinemaHall.Create(new List<HallRow>
        {
            new(1, 2),
            new(2, 3),
            new(3, 4),
            new(9, 5)
        });

        var reservation = _dataProvider.GetCorrectReservation(cinemaHall.Rows.ToList());
        var reservation2 = _dataProvider.GetCorrectReservation(cinemaHall.Rows.ToList());

        _movieTheaterRepositoryMock.Setup(x => x.GetCinemaHall(It.IsAny<Guid>()))
            .ReturnsAsync(cinemaHall);

        // Act
        var act = () => _service.Reserve(cinemaHall.Id, reservation).Wait();
        var act2 = () => _service.Reserve(cinemaHall.Id, reservation2).Wait();

        // Assert
        act.Should().NotThrow();
        act2.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Seats are already reserved.");
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
            .WithMessage(
                $"HallReservation must be between {HallReservation.MinSeatCount} and {HallReservation.MaxSeatCount} seats.");
    }

    [Test]
    public void CannotCreateReservationIfNotEnoughSeatsInRow()
    {
        // Arrange
        var cinemaHall = CinemaHall.Create(new List<HallRow>
        {
            new(1, 1),
            new(2, 4),
            new(3, 5),
            new(9, 6)
        });

        var reservation = _dataProvider.GetCorrectReservation(cinemaHall.Rows.ToList());

        _movieTheaterRepositoryMock.Setup(x => x.GetCinemaHall(It.IsAny<Guid>()))
            .ReturnsAsync(cinemaHall);

        // Act
        var act = () => _service.Reserve(cinemaHall.Id, reservation).Wait();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Not enough seats available in the row *");
    }
}