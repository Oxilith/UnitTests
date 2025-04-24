namespace UnitTests.Domain.General.Interfaces;

public interface IInvoiceService
{
    decimal CalculateTotal(decimal amount, string customerType);
}