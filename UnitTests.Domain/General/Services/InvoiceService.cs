using UnitTests.Domain.General.Interfaces;

namespace UnitTests.Domain.General.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IDiscountService _discountService;
    private readonly ITaxService _taxService;

    public InvoiceService()
    {
    }

    public InvoiceService(ITaxService taxService, IDiscountService discountService)
    {
        _taxService = taxService;
        _discountService = discountService;
    }

    public decimal CalculateTotal(decimal amount, string customerType)
    {
        var discount = _discountService.CalculateDiscount(amount, customerType);
        var taxableAmount = amount - discount;
        var tax = _taxService.GetTax(taxableAmount);
        return taxableAmount + tax;
    }

    public string GenerateInvoiceNumber()
    {
        var datePart = DateTime.Now.ToString("yyyyMMdd");
        var randomPart = new Random().Next(100, 999).ToString();
        return $"INV-{datePart}-{randomPart}";
    }

    public List<InvoiceItem> GenerateInvoiceItems()
    {
        return new List<InvoiceItem>
        {
            new() { ProductName = "Laptop", Quantity = 1, UnitPrice = 1000m },
            new() { ProductName = "Smartphone", Quantity = 2, UnitPrice = 500m },
            new() { ProductName = "Tablet", Quantity = 3, UnitPrice = 300m }
            // więcej pozycji może być dodanych tutaj
        };
    }

    public decimal CalculateInvoiceAmount(int orderId, string customerType)
    {
        // Przykładowa implementacja
        decimal baseAmount = 100; // Załóżmy, że to jest bazowa kwota na podstawie orderId
        var discount = _discountService.CalculateDiscount(baseAmount, customerType);
        var amountAfterDiscount = baseAmount - discount;
        var tax = _taxService.GetTax(amountAfterDiscount);
        return amountAfterDiscount + tax;
    }
}