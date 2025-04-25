using UnitTests.Domain.MovieTheaterUseCase.Exceptions;

namespace UnitTests.Domain.MovieTheaterUseCase.Entities;

public class CinemaHall
{
    private readonly List<Reservation> _reservations = new();
    private readonly List<Row> _rows;

    private CinemaHall(List<Row> rows)
    {
        _rows = rows;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public IReadOnlyCollection<Reservation> Reservations => _reservations;
    public IReadOnlyCollection<Row> Rows => _rows;

    public static CinemaHall Create(List<Row> rows)
    {
        ValidateRows(rows);
        return new CinemaHall(rows);
    }

    private static void ValidateRows(List<Row> rows)
    {
        if (rows is not { Count: > 0 })
            throw new BusinessRuleViolationException("Cinema hall should have at least one row.");

        if (rows.Count > 9)
            throw new BusinessRuleViolationException("Cinema hall should have at most 9 rows.");

        if (rows.GroupBy(x => x.Number)
            .Any(g => g.Count() > 1))
            throw new BusinessRuleViolationException(
                "Cannot add two or more rows with the same number to the cinema hall.");

        var lastCount = 0;
        foreach (var row in rows)
        {
            if (row.Seats <= lastCount)
                throw new BusinessRuleViolationException("Rows seat count should increase with each row.");

            lastCount = row.Seats;
        }
    }

    public void Reserve(Reservation reservation)
    {
        _reservations.Add(reservation);
    }

    public bool DoesReservationExistInTheHall(Reservation reservation)
    {
        return _reservations.Any(x => x.Id == reservation.Id);
    }

    public bool DoesRowContainEnoughSeats(Guid rowId, int seatCount)
    {
        return _rows.First(x => x.Id == rowId).Seats >= seatCount;
    }

    public bool DoesRowExistInTheHall(Guid rowId)
    {
        return _rows.Any(x => x.Id == rowId);
    }

    public bool DoesReservationExistInTheHall(Guid reservationId)
    {
        return _reservations.Any(x => x.Id == reservationId);
    }
}