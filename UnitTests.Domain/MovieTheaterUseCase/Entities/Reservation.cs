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

        var seatsByRows = GroupSeatsByRows(seats);
        ValidateSeatPositions(seatsByRows);

        SeatsByRows = seatsByRows;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public Dictionary<Guid, List<int>> SeatsByRows { get; }

    private static void ValidateSeatPositions(Dictionary<Guid, List<int>> seatDict)
    {
        if (seatDict.Any(rowSeats => rowSeats.Value.Count is < 1 or > 5))
            throw new BusinessRuleViolationException(
                "Reservation must be between 1 and 5 seats.");
    }

    private static Dictionary<Guid, List<int>> GroupSeatsByRows(List<SeatPosition> seats)
    {
        return seats
            .GroupBy(x => x.RowId)
            .ToDictionary(g => g.Key,
                g => g.Select(s => s.SeatNumber).ToList());
    }
}