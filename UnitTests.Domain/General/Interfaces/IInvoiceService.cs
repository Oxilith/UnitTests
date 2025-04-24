namespace UnitTests.Domain.Interfaces;

public interface IInvoiceService
{
    decimal CalculateTotal(decimal amount, string customerType);
}