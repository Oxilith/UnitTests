namespace UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

public record SeatPosition
{
    public SeatPosition(Guid rowId, int seatNumber)
    {
        RowId = rowId;
        SeatNumber = seatNumber;
    }

    public Guid RowId { get; }
    public int SeatNumber { get; }
}