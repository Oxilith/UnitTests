using UnitTests.Domain.MovieTheaterUseCase.Entities;
using UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

namespace UnitTests.Tests.Domain;

public class TestDataProvider
{
    public Reservation GetEmptyReservation() => 
        new Reservation(new List<SeatPosition>());
    public Reservation GetCorrectReservation()
    {
        return new Reservation(new List<SeatPosition>
        {

            new SeatPosition(1, 1),
            new SeatPosition(1, 2),
            new SeatPosition(2, 1),
            new SeatPosition(2, 2),
            new SeatPosition(2, 3),
            new SeatPosition(3, 1),
            new SeatPosition(3, 2),
            new SeatPosition(3, 3),
            new SeatPosition(3, 4),
            new SeatPosition(9, 1),
            new SeatPosition(9, 2),
            new SeatPosition(9, 3),
            new SeatPosition(9, 4),
            new SeatPosition(9, 5)
        });
    }    
    
    public Reservation GetIncorrectReservation()
    {

        return new Reservation(new List<SeatPosition>
        {
            new SeatPosition(2, 1),
            new SeatPosition(2, 2),
            new SeatPosition(2, 3),
            new SeatPosition(1, 1),
            new SeatPosition(1, 2),
            new SeatPosition(1, 3),
            new SeatPosition(1, 4),
            new SeatPosition(1, 5),
            new SeatPosition(1, 6)
        });
    }
    
    public Reservation GetLengthyReservation()
    {

        return new Reservation(new List<SeatPosition>
        {
            new SeatPosition(2, 1),
            new SeatPosition(2, 2),
            new SeatPosition(2, 3),
            new SeatPosition(1, 1),
            new SeatPosition(1, 2),
            new SeatPosition(1, 3),
            new SeatPosition(1, 4),
            new SeatPosition(1, 5),
            new SeatPosition(1, 6)
        });
    }
    
}