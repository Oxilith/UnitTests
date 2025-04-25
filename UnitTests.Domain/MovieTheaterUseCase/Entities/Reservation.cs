using UnitTests.Domain.MovieTheaterUseCase.Exceptions;
using UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

namespace UnitTests.Domain.MovieTheaterUseCase.Entities;

public class Reservation
{
    public Reservation(List<SeatPosition> seats)
    {
        if (seats == null || seats.Count == 0)
            throw new BusinessRuleViolationException(
                "Reservation must have at least one seat.");


        var seatDict = seats
            .GroupBy(x => x.Row)
            .ToDictionary(g => g.Key,
                g => g.Select(s => s.Number).ToList());

        foreach (var rowSeats in seatDict)
        {
            var isInvalidSeatCount = rowSeats.Value.Count is < 1 or > 5;
            if (isInvalidSeatCount)
                throw new BusinessRuleViolationException(
                    "Reservation must be between 1 and 5 seats.");
        }

        Seats = seats;


        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public IEnumerable<SeatPosition> Seats { get; }
}