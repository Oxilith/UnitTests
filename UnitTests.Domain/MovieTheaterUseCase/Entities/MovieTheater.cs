using UnitTests.Domain.MovieTheaterUseCase.Exceptions;
using UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

namespace UnitTests.Domain.MovieTheaterUseCase.Entities;

public class MovieTheater
{
    public Guid Id { get; }
    private readonly List<Reservation> _reservations = new();
    private readonly List<Row> _rows = new();

    public MovieTheater(List<Row> rows)
    {
        if (rows == null || rows.Count == 0)
            throw new BusinessRuleViolationException("Movie theater must have at least one row.");
    
        _rows = rows;
        Id = Guid.NewGuid();
    }

    public void Reserve(Guid reservationId, IEnumerable<SeatPosition> seats)
    {
        // TODO: 
    }

    public IReadOnlyCollection<Reservation> Reservations => _reservations;
    public IReadOnlyCollection<Row> Rows => _rows;
}