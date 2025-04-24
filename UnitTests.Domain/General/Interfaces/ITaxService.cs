namespace UnitTests.Domain.Interfaces;

public interface ITaxService
{
    decimal GetTax(decimal amount);
}