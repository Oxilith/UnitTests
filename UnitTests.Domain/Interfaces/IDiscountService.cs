namespace UnitTests.Domain.Interfaces;

public interface IDiscountService
{
    decimal CalculateDiscount(decimal amount, string customerType);
}