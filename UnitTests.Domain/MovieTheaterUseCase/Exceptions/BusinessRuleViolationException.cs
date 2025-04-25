namespace UnitTests.Domain.MovieTheaterUseCase.Exceptions;

public class BusinessRuleViolationException : Exception
{
    public BusinessRuleViolationException(string message) : base(message) { }
}