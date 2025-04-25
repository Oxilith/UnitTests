using UnitTests.Domain.MovieTheaterUseCase.Exceptions;

namespace UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

public record SeatPosition
{
    public SeatPosition(int row, int number)
    {
        if (row is < 1 or > 9)
            throw new BusinessRuleViolationException("Row number must be between 1 and 9 inclusive.");

        Row = row;
        Number = number;
    }

    public int Row { get; }
    public int Number { get; }
}