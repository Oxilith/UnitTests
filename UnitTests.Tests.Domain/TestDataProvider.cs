using UnitTests.Domain.MovieTheaterUseCase.Entities;
using UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

namespace UnitTests.Tests.Domain;

public class TestDataProvider
{
    #region HallReservation

    public HallReservation GetReservationWithNullSeatPositions()
    {
        return new HallReservation(null);
    }

    public HallReservation? GetNullReservation()
    {
        return null;
    }

    public HallReservation GetEmptyReservation()
    {
        return new HallReservation(new List<SeatPosition>());
    }

    public HallReservation GetCorrectReservation(List<HallRow>? rows = null)
    {
        rows ??= new List<HallRow>
        {
            new(1, 2),
            new(2, 3),
            new(3, 4),
            new(9, 5)
        };

        return new HallReservation(new List<SeatPosition>
        {
            new(rows.First(x => x.Number == 1).Id, 1),
            new(rows.First(x => x.Number == 1).Id, 2),
            new(rows.First(x => x.Number == 2).Id, 1),
            new(rows.First(x => x.Number == 2).Id, 2),
            new(rows.First(x => x.Number == 2).Id, 3),
            new(rows.First(x => x.Number == 3).Id, 1),
            new(rows.First(x => x.Number == 3).Id, 2),
            new(rows.First(x => x.Number == 3).Id, 3),
            new(rows.First(x => x.Number == 3).Id, 4),
            new(rows.First(x => x.Number == 9).Id, 1),
            new(rows.First(x => x.Number == 9).Id, 2),
            new(rows.First(x => x.Number == 9).Id, 3),
            new(rows.First(x => x.Number == 9).Id, 4),
            new(rows.First(x => x.Number == 9).Id, 5)
        });
    }

    public HallReservation GetLengthyReservation()
    {
        var rows = new List<HallRow>
        {
            new(1, 6),
            new(2, 3)
        };
        return new HallReservation(new List<SeatPosition>
        {
            new(rows.First(x => x.Number == 2).Id, 1),
            new(rows.First(x => x.Number == 2).Id, 2),
            new(rows.First(x => x.Number == 2).Id, 3),
            new(rows.First(x => x.Number == 1).Id, 1),
            new(rows.First(x => x.Number == 1).Id, 2),
            new(rows.First(x => x.Number == 1).Id, 3),
            new(rows.First(x => x.Number == 1).Id, 4),
            new(rows.First(x => x.Number == 1).Id, 5),
            new(rows.First(x => x.Number == 1).Id, 6)
        });
    }

    #endregion

    #region CinemaHall

    public CinemaHall GetCorrectCinemaHall()
    {
        return CinemaHall.Create(new List<HallRow>
        {
            new(1, 3),
            new(2, 4),
            new(3, 5)
        });
    }

    public CinemaHall GetEmptyCinemaHall()
    {
        return CinemaHall.Create(new List<HallRow>());
    }

    public CinemaHall GetLengthyCinemaHall()
    {
        return CinemaHall.Create(new List<HallRow>
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
            new(10, 9)
        });
    }

    public CinemaHall GetCinemaHallWithWrongSeatCountInRows()
    {
        return CinemaHall.Create(new List<HallRow>
        {
            new(1, 1),
            new(2, 2),
            new(3, 3),
            new(4, 2),
            new(5, 5),
            new(6, 6),
            new(7, 7),
            new(8, 8)
        });
    }

    #endregion
}