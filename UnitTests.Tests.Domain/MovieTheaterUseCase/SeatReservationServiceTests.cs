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
    private const int MaxRowNumbers = 9;
    private const int MinRowNumbers = 1;
    
    private TestDataProvider _dataProvider;
    private Mock<IMovieTheaterRepository> _movieTheaterRepositoryMock;
    private SeatReservationService _service;

    [SetUp]
    public void SetUp()
    {
        _movieTheaterRepositoryMock = new Mock<IMovieTheaterRepository>();
        _dataProvider = new TestDataProvider();
        _service = new SeatReservationService(_movieTheaterRepositoryMock.Object);
    }

    [Test]
    // Nie można dodać dwóch lub więcej rzędów o tym samym numerze do sali kinowej
    public void CannotAddTwoRowsWithSameNumber()
    {
        // Arrange
        var theater = new MovieTheater(new List<Row>
        {
            new(1, 3),
            new(2, 4),
            new(2, 5)
        });

        // _movieTheaterRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>()))
        //     .ReturnsAsync(theater);
        //
        // var reservation = _dataProvider.GetCorrectReservation();
        //
        // // Act
        // Action act = () => _service.Reserve(theater.Id, reservation).Wait();
        //
        // // Assert
        // act.Should().Throw<BusinessRuleViolationException>()
        //     .WithMessage("Cannot add two or more rows with the same number to the movie theater.");
    }
    
    [Test]
    public void RowsSeatCountShouldIncreaseWithEachRow()
    {
        // Arrange & Act
        Action act = () =>  _dataProvider.GetCorrectMovieTheater();

        // Assert
        act.Should().NotThrow();
    }    
   
    [Test]
    public void CannotCreateMovieTheaterWhenRowCountIsIncorrect()
    {
        // Arrange & Act
        Action act = () =>  _dataProvider.GetEmptyMovieTheater();
        Action act2 = () =>  _dataProvider.GetLengthyMovieTheater();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Movie theater should have at least one row.");
        act2.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Movie theater should have at most 9 rows.");
    }    
    
    [Test]
    public void CannotCreateMovieTheaterWhenRowsSeatCountDoesNotIncreaseWithEachRow()
    {
        // Arrange & Act
        Action act = () =>  _dataProvider.GetMovieTheaterWithWrongSeatCountInRows();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Rows seat count should increase with each row.");
    }
    
    
    [TestCase(-1, 1)]
    [TestCase(0, 1)]
    [TestCase(10, 1)]
    public void CannotCreateRowsThatAreWithWrongRowNumber(int rowNumber, int seatNumber)
    {
        // Arrange & Act
        Action act = () => new SeatPosition(rowNumber, seatNumber);
        Action act2 = () => new Row(rowNumber, seatNumber);

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Row number must be between 1 and 9 inclusive.");

        act2.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Row number must be between 1 and 9 inclusive.");
    }

    [TestCase(1, 1)]
    [TestCase(2, 1)]
    [TestCase(8, 1)]
    [TestCase(9, 1)]
    public void CanCreateRowsThatAreNumberedFrom1To9(int rowNumber, int seatNumber)
    {
        // Arrange & Act
        Action act = () => new SeatPosition(rowNumber, seatNumber);
        Action act2 = () => new Row(rowNumber, seatNumber);

        // Assert
        act.Should().NotThrow<BusinessRuleViolationException>();
        act2.Should().NotThrow<BusinessRuleViolationException>();
    }

    [Test]
    public void CannotCreateReservationIfLessThanOneSeat()
    {
        // Arrange
        var theater = new MovieTheater(new List<Row>
        {
            new(1, 3),
            new(2, 4),
            new(3, 5)
        });

        _movieTheaterRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(theater);

        // Act
        Action act = () => _dataProvider.GetEmptyReservation();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Reservation must have at least one seat.");
    }

    [Test]
    public void CannotCreateReservationIfMoreThanFiveSeats()
    {
        // Arrange
        var theater = new MovieTheater(new List<Row>
        {
            new(1, 3),
            new(2, 4),
            new(3, 5)
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
    public void CannotCreateReservationIfNotEnoughSeatsInRow()
    {
        // Arrange
        var theater = new MovieTheater(new List<Row>
        {
            new(1, 1),
            new(2, 4),
            new(3, 5),
            new(9, 6),
        });

        _movieTheaterRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>()))
            .ReturnsAsync(theater);

        var reservation = _dataProvider.GetCorrectReservation();

        // Act
        var act = () => _service.Reserve(theater.Id, reservation).Wait();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Not enough seats available in the row ?.");
    }
}