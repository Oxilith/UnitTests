using FluentAssertions;
using UnitTests.Domain.General.Services;

namespace UnitTests.Tests.Domain.General;

public class InvoiceServiceTests
{
    private InvoiceService _invoiceService;

    [SetUp]
    public void Setup()
    {
        _invoiceService = new InvoiceService();
    }

    // Testy dla GenerateInvoiceNumber
    [Test]
    public void GenerateInvoiceNumber_ShouldStartWith_INV()
    {
        // Act
        var invoiceNumber = _invoiceService.GenerateInvoiceNumber();

        // Assert
        invoiceNumber.Should().StartWith("INV-");
    }

    [Test]
    public void GenerateInvoiceNumber_ShouldEndWith_NumericSuffix()
    {
        // Act
        var invoiceNumber = _invoiceService.GenerateInvoiceNumber();

        // Assert
        invoiceNumber.Should().MatchRegex(@"INV-\d{8}-\d{3}$");
    }

    [Test]
    public void GenerateInvoiceNumber_ShouldContain_CurrentDate()
    {
        // Arrange
        var currentDate = DateTime.Now.ToString("yyyyMMdd");

        // Act
        var invoiceNumber = _invoiceService.GenerateInvoiceNumber();

        // Assert
        invoiceNumber.Should().Contain(currentDate);
    }

    [Test]
    public void GenerateInvoiceNumber_ShouldHaveLength_Of_16()
    {
        // Act
        var invoiceNumber = _invoiceService.GenerateInvoiceNumber();

        // Assert
        invoiceNumber.Should().HaveLength(16);
    }

    [Test]
    public void GenerateInvoiceNumber_ShouldNotBeEmpty()
    {
        // Act
        var invoiceNumber = _invoiceService.GenerateInvoiceNumber();

        // Assert
        invoiceNumber.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void GenerateInvoiceNumber_ShouldMatch_ExpectedFormat()
    {
        // Act
        var invoiceNumber = _invoiceService.GenerateInvoiceNumber();

        // Assert
        invoiceNumber.Should().Match("INV-????????-???").And.BeOfType<string>();
    }

    // Testy dla GenerateInvoiceItems
    [Test]
    public void GenerateInvoiceItems_ShouldReturn_NonEmptyCollection()
    {
        // Act
        var items = _invoiceService.GenerateInvoiceItems();

        // Assert
        items.Should().NotBeEmpty();
    }

    [Test]
    public void GenerateInvoiceItems_ShouldContain_ItemWithSpecificName()
    {
        // Arrange
        var expectedProductNames = new[] { "Laptop", "Smartphone", "Tablet" };

        // Act
        var items = _invoiceService.GenerateInvoiceItems();

        // Assert
        items.Select(x => x.ProductName)
            .Should()
            .AllSatisfy(x => { x.Should().BeOneOf(expectedProductNames); });
    }

    [Test]
    public void GenerateInvoiceItems_AllItems_ShouldHavePositiveQuantity()
    {
        // Act
        var items = _invoiceService.GenerateInvoiceItems();

        // Assert
        items.Should().OnlyContain(item => item.Quantity > 0);
    }

    [Test]
    public void GenerateInvoiceItems_ShouldHave_ItemWithQuantityGreaterThanOne()
    {
        // Act
        var items = _invoiceService.GenerateInvoiceItems();

        // Assert
        items.Should().Contain(item => item.Quantity > 1);
    }

    [Test]
    public void GenerateInvoiceItems_ShouldHave_ExactNumberOfItems()
    {
        // Act
        var items = _invoiceService.GenerateInvoiceItems();

        // Assert
        items.Should().HaveCount(3);
    }

    [Test]
    public void GenerateInvoiceItems_ShouldHave_ItemsInAscendingOrderByQuantity()
    {
        // Act
        var items = _invoiceService.GenerateInvoiceItems();

        // Assert
        items.Should().BeInAscendingOrder(item => item.Quantity);
    }
}