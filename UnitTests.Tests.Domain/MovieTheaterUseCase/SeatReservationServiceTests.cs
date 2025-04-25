using FluentAssertions;
using UnitTests.Domain.MovieTheaterUseCase.Entities;
using UnitTests.Domain.MovieTheaterUseCase.Exceptions;
using UnitTests.Domain.MovieTheaterUseCase.Services;
using UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

namespace UnitTests.Tests.Domain.MovieTheaterUseCase;

public class SeatReservationServiceTests
{
    private SeatReservationService _service;

    [SetUp]
    public void SetUp()
    {
        
        _service = new SeatReservationService();
    }
    
    [Test]
    //Nie można utworzyć rezerwacji, jeśli nie ma wystarczającej liczby miejsc w rzędzie.
    public async Task CannotCreateReservationIfNotEnoughSeatsInRow()
    {
        // Arrange
        var theater = new MovieTheater(new List<Row>(){
            new Row(1, 5),
            new Row(2, 10),
            new Row(3, 10)
        });
        
        var reservation = new Reservation(new List<SeatPosition>()
        {
            new SeatPosition(1, 1),
            new SeatPosition(1, 2),
            new SeatPosition(1, 3),
            new SeatPosition(1, 4),
            new SeatPosition(1, 5),
            new SeatPosition(1, 6)
        });

        // Act
        Action act = () =>  _service.Reserve(theater.Id, reservation).Wait();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Not enough seats available in the row.");
    }
}
