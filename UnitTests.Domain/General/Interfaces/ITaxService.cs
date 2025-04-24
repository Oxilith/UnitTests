namespace UnitTests.Domain.General.Interfaces;

public interface ITaxService
{
    decimal GetTax(decimal amount);
}