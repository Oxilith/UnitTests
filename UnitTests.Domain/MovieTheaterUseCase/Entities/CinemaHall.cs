using UnitTests.Domain.MovieTheaterUseCase.Exceptions;

namespace UnitTests.Domain.MovieTheaterUseCase.Entities;

public class CinemaHall
{
    public static readonly int MinRowCount = 1;
    public static readonly int MaxRowCount = 9;
    public static readonly int ExpectedFirstRowNumber = 1;
    public static readonly int SocialDistance = 1;

    private readonly List<HallReservation> _reservations = new();
    private readonly List<HallRow> _rows;

    private CinemaHall(List<HallRow> rows)
    {
        _rows = rows;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public IReadOnlyCollection<HallReservation> Reservations => _reservations;
    public IReadOnlyCollection<HallRow> Rows => _rows;

    public static CinemaHall Create(List<HallRow> rows)
    {
        ValidateRows(rows);
        return new CinemaHall(rows);
    }

    private static void ValidateRows(IReadOnlyList<HallRow> rows)
    {
        ValidateRowCount(rows.Count);
        ValidateFirstRowNumber(rows.First().Number);
        ValidateDuplicateRows(rows);
        ValidateRowOrder(rows);
        ValidateSeatCountIncreasing(rows);
    }

    private static void ValidateRowOrder(IReadOnlyList<HallRow> rows)
    {
        for (var i = 1; i < rows.Count; i++)
            if (rows[i].Number <= rows[i - 1].Number)
                throw new BusinessRuleViolationException(
                    "Cinema hall rows should be in order.");
    }

    private static void ValidateFirstRowNumber(int number)
    {
        if (number != ExpectedFirstRowNumber)
            throw new BusinessRuleViolationException(
                $"Cinema hall first row should have number {ExpectedFirstRowNumber}.");
    }

    private static void ValidateRowCount(int count)
    {
        if (count < MinRowCount)
            throw new BusinessRuleViolationException(
                $"Cinema hall should have at least {MinRowCount} row.");
        if (count > MaxRowCount)
            throw new BusinessRuleViolationException(
                $"Cinema hall should have at most {MaxRowCount} rows.");
    }

    private static void ValidateDuplicateRows(IEnumerable<HallRow> rows)
    {
        var duplicates = rows
            .GroupBy(r => r.Number)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        if (duplicates.Any())
            throw new BusinessRuleViolationException(
                "Cannot add two or more rows with the same number to the cinema hall.");
    }

    private static void ValidateSeatCountIncreasing(IReadOnlyList<HallRow> rows)
    {
        for (var i = 1; i < rows.Count; i++)
            if (rows[i].Seats <= rows[i - 1].Seats)
                throw new BusinessRuleViolationException(
                    "Rows seat count should increase with each row.");
    }

    public void Reserve(HallReservation hallReservation)
    {
        _reservations.Add(hallReservation);
    }

    public bool DoesReservationExistInTheHall(HallReservation hallReservation)
    {
        return _reservations.Any(x => x.Id == hallReservation.Id);
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

    public List<Dictionary<Guid, List<int>>> GetReservedSeatsByRow()
    {
        return _reservations.Select(x => x.SeatsByRows).ToList();
    }
}