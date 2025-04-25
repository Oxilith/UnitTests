using UnitTests.Domain.MovieTheaterUseCase.Exceptions;

namespace UnitTests.Domain.MovieTheaterUseCase.ValueObjects;

public record Row
{
    public Row(int number, int seats)
    {
        if (number is < 1 or > 9)
            throw new BusinessRuleViolationException("Row number must be between 1 and 9 inclusive.");

        Number = number;
        Seats = seats;
    }

    public int Number { get; }
    public int Seats { get; }
}