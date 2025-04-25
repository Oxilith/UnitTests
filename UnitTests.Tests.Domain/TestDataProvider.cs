using UnitTests.Domain.MovieTheaterUseCase.Entities;
using UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

namespace UnitTests.Tests.Domain;

public class TestDataProvider
{
    #region Reservation
    public Reservation GetReservationWithNullSeatPositions()
    {
        return new Reservation(null);
    }

    public Reservation? GetNullReservation()
    {
        return null;
    }

    public Reservation GetEmptyReservation()
    {
        return new Reservation(new List<SeatPosition>());
    }

    public Reservation GetCorrectReservation()
    {
        return new Reservation(new List<SeatPosition>
        {
            new(1, 1),
            new(1, 2),
            new(2, 1),
            new(2, 2),
            new(2, 3),
            new(3, 1),
            new(3, 2),
            new(3, 3),
            new(3, 4),
            new(9, 1),
            new(9, 2),
            new(9, 3),
            new(9, 4),
            new(9, 5)
        });
    }

    public Reservation GetIncorrectReservation()
    {
        return new Reservation(new List<SeatPosition>
        {
            new(2, 1),
            new(2, 2),
            new(2, 3),
            new(1, 1),
            new(1, 2),
            new(1, 3),
            new(1, 4),
            new(1, 5),
            new(1, 6)
        });
    }

    public Reservation GetLengthyReservation()
    {
        return new Reservation(new List<SeatPosition>
        {
            new(2, 1),
            new(2, 2),
            new(2, 3),
            new(1, 1),
            new(1, 2),
            new(1, 3),
            new(1, 4),
            new(1, 5),
            new(1, 6)
        });
    }
    
    #endregion
    
    #region MovieTheater
    public MovieTheater GetCorrectMovieTheater() => new MovieTheater(new List<Row>
    {
        new(1, 3),
        new(2, 4),
        new(3, 5)
    });       
    
    public MovieTheater GetEmptyMovieTheater() => new MovieTheater(new List<Row>
    { });   
    
    public MovieTheater GetLengthyMovieTheater() => new MovieTheater(new List<Row>
    {
        new(1, 1),
        new(2, 2),
        new(3, 3),
        new(4, 4),
        new(5, 5),
        new(6, 6),
        new(7, 7),
        new(8, 8),
        new(9, 9),
        new(10, 9),
    });
    
    public MovieTheater GetMovieTheaterWithWrongSeatCountInRows() => new MovieTheater(new List<Row>
    {
        new(1, 1),
        new(2, 2),
        new(3, 3),
        new(4, 2),
        new(5, 5),
        new(6, 6),
        new(7, 7),
        new(8, 8),
    });

    
    #endregion
}