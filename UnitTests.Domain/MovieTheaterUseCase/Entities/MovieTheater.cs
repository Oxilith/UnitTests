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

    public void Reserve(Reservation reservation)
    {
        if (_reservations.Any(x => x.Id == reservation.Id))
            throw new BusinessRuleViolationException("Reservation was already added.");
        
        var seatDict = reservation.Seats
            .GroupBy(x => x.Row)
            .ToDictionary(g => g.Key,
                g => g.Select(s => s.Number).ToList());

        foreach (var rowSeats in seatDict)
        {
            if (_rows.All(x => x.Number != rowSeats.Key))
                throw new BusinessRuleViolationException($"Row {rowSeats.Key} does not exist in the theater.");

            if (_rows.First(x => x.Number == rowSeats.Key).Seats <= rowSeats.Value.Count)
                throw new BusinessRuleViolationException($"Not enough seats available in the row {rowSeats.Key}.");
        }
        
        _reservations.Add(reservation);
    }

    public IReadOnlyCollection<Reservation> Reservations => _reservations;
    public IReadOnlyCollection<Row> Rows => _rows;
}