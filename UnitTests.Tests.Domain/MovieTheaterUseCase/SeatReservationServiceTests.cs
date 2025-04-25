using FluentAssertions;
using Moq;
using UnitTests.Domain.MovieTheaterUseCase.Entities;
using UnitTests.Domain.MovieTheaterUseCase.Exceptions;
using UnitTests.Domain.MovieTheaterUseCase.Repositories;
using UnitTests.Domain.MovieTheaterUseCase.Services;
using UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

namespace UnitTests.Tests.Domain.MovieTheaterUseCase;

public class SeatReservationServiceTests
{
    private SeatReservationService _service;
    private Mock<IMovieTheaterRepository> _movieTheaterRepositoryMock;
    private TestDataProvider _dataProvider;
    
    [SetUp]
    public void SetUp()
    {
        _movieTheaterRepositoryMock = new Mock<IMovieTheaterRepository>();
        _dataProvider = new TestDataProvider();
        _service = new SeatReservationService(_movieTheaterRepositoryMock.Object);
    }
    
    [Test]
    // Rezerwacja nie może być mniejsza niż 1 miejsce i większa niż 5 miejsc.
    public async Task CannotCreateReservationIfLessThanOneSeat()
    {
        // Arrange
        var theater = new MovieTheater(new List<Row>(){
            new Row(1, 3),
            new Row(2, 4),
            new Row(3, 5)
        });
        
        _movieTheaterRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(theater);

        // Act
        Action act = () => _dataProvider.GetEmptyReservation();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Reservation must be between 1 and 5 seats.");
    }    
    
    [Test]
    public async Task CannotCreateReservationIfMoreThanFiveSeats()
    {
        // Arrange
        var theater = new MovieTheater(new List<Row>(){
            new Row(1, 3),
            new Row(2, 4),
            new Row(3, 5)
        });
        
        _movieTheaterRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(theater);

        // Act
        Action act = () => _dataProvider.GetLengthyReservation();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Reservation must be between 1 and 5 seats.");
    }
    
    [Test]
    public async Task CannotCreateReservationIfNotEnoughSeatsInRow()
    {
        // Arrange
        var theater = new MovieTheater(new List<Row>(){
            new Row(1, 3),
            new Row(2, 4),
            new Row(3, 5),
            new Row(9, 5)
        });
        
        _movieTheaterRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(theater);

        var reservation = _dataProvider.GetCorrectReservation();

        // Act
        Action act = () =>  _service.Reserve(theater.Id, reservation).Wait();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Not enough seats available in the row ?.");
    }
}
