using UnitTests.Domain.MovieTheaterUseCase.Exceptions;
using UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

namespace UnitTests.Domain.MovieTheaterUseCase.Entities;

public class HallReservation
{
    public static readonly int MaxSeatCount = 5;
    public static readonly int MinSeatCount = 1;

    public HallReservation(List<SeatPosition> seats)
    {
        ArgumentNullException.ThrowIfNull(seats, nameof(seats));
        EnsureHasAtLeastOneSeat(seats);

        var seatsByRows = GroupSeatsByRows(seats);
        ValidateSeatPositions(seatsByRows);

        SeatsByRows = seatsByRows;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public Dictionary<Guid, List<int>> SeatsByRows { get; }

    private static void EnsureHasAtLeastOneSeat(List<SeatPosition> seats)
    {
        if (seats.Count == 0)
            throw new BusinessRuleViolationException(
                "HallReservation must have at least one seat.");
    }

    private static void ValidateSeatPositions(Dictionary<Guid, List<int>> seatDict)
    {
        if (seatDict.Any(rowSeats =>
                rowSeats.Value.Count < MinSeatCount || rowSeats.Value.Count > MaxSeatCount))
            throw new BusinessRuleViolationException(
                "HallReservation must be between 1 and 5 seats.");
    }

    private static Dictionary<Guid, List<int>> GroupSeatsByRows(List<SeatPosition> seats)
    {
        return seats
            .GroupBy(x => x.RowId)
            .ToDictionary(g => g.Key,
                g => g.Select(s => s.SeatNumber).ToList());
    }
}