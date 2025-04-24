using FluentAssertions;
using Moq;

namespace UnitTests.Tests.Domain;

public class InvoiceItem
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public interface IInvoiceService
{
    decimal CalculateTotal(decimal amount, string customerType);
}

public interface ITaxService
{
    decimal GetTax(decimal amount);
}

public interface IDiscountService
{
    decimal CalculateDiscount(decimal amount, string customerType);
}

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
        // Act
        var items = _invoiceService.GenerateInvoiceItems();

        // Assert
        items.Should().Contain(item => item.ProductName == "Laptop");
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

public class MoqInvoiceServiceTests
{
    private Mock<IDiscountService> _discountServiceMock;
    private InvoiceService _invoiceService;
    private Mock<ITaxService> _taxServiceMock;

    [SetUp]
    public void Setup()
    {
        _taxServiceMock = new Mock<ITaxService>();
        _discountServiceMock = new Mock<IDiscountService>();
        _invoiceService = new InvoiceService(_taxServiceMock.Object, _discountServiceMock.Object);
    }

    [Test]
    public void CalculateTotal_WhenCalled_VerifiesTaxServiceGetTaxIsCalled()
    {
        // Arrange
        var amount = 100m;
        var customerType = "Regular";
        _discountServiceMock.Setup(x => x.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>())).Returns(10m);
        _taxServiceMock.Setup(x => x.GetTax(It.IsAny<decimal>())).Returns(5m);

        // Act
        _invoiceService.CalculateTotal(amount, customerType);

        // Assert
        _taxServiceMock.Verify(x => x.GetTax(90m),
            Times.Once); // Verifies that GetTax was called with the amount after discount
    }

    [Test]
    public void CalculateTotal_WhenCalled_ReturnsExpectedTotal()
    {
        // Arrange
        var amount = 100m;
        var customerType = "Regular";
        _discountServiceMock.Setup(x => x.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>())).Returns(10m);
        _taxServiceMock.Setup(x => x.GetTax(It.IsAny<decimal>())).Returns(5m);

        // Act
        var total = _invoiceService.CalculateTotal(amount, customerType);

        // Assert
        total.Should().Be(95m); // Amount after discount + tax
    }

    [Test]
    public void CalculateDiscount_WhenCalled_UsesCallbackToManipulateAmount()
    {
        // Arrange
        decimal capturedDiscount = 0;
        _discountServiceMock.Setup(x => x.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>()))
            .Returns(10m)
            .Callback<decimal, string>((amount, customerType) => capturedDiscount = amount * 0.1m);

        // Act
        _invoiceService.CalculateTotal(100m, "Regular");

        // Assert
        capturedDiscount.Should().Be(10m);
    }

    [Test]
    public void CalculateInvoiceAmount_UsesTaxAndDiscountServices()
    {
        // Arrange
        var mockTaxService = new Mock<ITaxService>();
        var mockDiscountService = new Mock<IDiscountService>();
        mockDiscountService.Setup(s => s.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>())).Returns(10);
        mockTaxService.Setup(s => s.GetTax(It.IsAny<decimal>())).Returns(5);

        var invoiceService = new InvoiceService(mockTaxService.Object, mockDiscountService.Object);

        // Act
        var result = invoiceService.CalculateInvoiceAmount(1, "Any");

        // Assert
        result.Should().Be(95); // 100 - 10 + 5
        mockDiscountService.Verify(s => s.CalculateDiscount(100, "Any"), Times.Once);
        mockTaxService.Verify(s => s.GetTax(90), Times.Once);
    }

    [Test]
    public void CalculateInvoiceAmount_ThrowsWhenTaxServiceFails()
    {
        // Arrange
        var mockTaxService = new Mock<ITaxService>();
        var mockDiscountService = new Mock<IDiscountService>();
        mockTaxService.Setup(s => s.GetTax(It.IsAny<decimal>())).Throws(new Exception("Tax calculation failed"));

        var invoiceService = new InvoiceService(mockTaxService.Object, mockDiscountService.Object);

        // Act & Assert
        Assert.Throws<Exception>(() => invoiceService.CalculateInvoiceAmount(1, "Any"), "Tax calculation failed");
    }

    [Test]
    public void CalculateInvoiceAmount_CallsServicesInSequence()
    {
        // Arrange
        var sequence = new MockSequence();
        var mockTaxService = new Mock<ITaxService>();
        var mockDiscountService = new Mock<IDiscountService>();

        mockDiscountService.InSequence(sequence)
            .Setup(s => s.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>())).Returns(10);
        mockTaxService.InSequence(sequence).Setup(s => s.GetTax(It.IsAny<decimal>())).Returns(5);

        var invoiceService = new InvoiceService(mockTaxService.Object, mockDiscountService.Object);

        // Act
        var result = invoiceService.CalculateInvoiceAmount(1, "Any");

        // Assert
        result.Should().Be(95); // 100 - 10 + 5
    }

    [Test]
    public void CalculateInvoiceAmount_UsesMonitorToTrackMethodCalls()
    {
        // Arrange
        var mockTaxService = new Mock<ITaxService>();
        var mockDiscountService = new Mock<IDiscountService>();
        mockDiscountService.Setup(s => s.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>())).Returns(10);
        mockTaxService.Setup(s => s.GetTax(It.IsAny<decimal>())).Returns(5);

        var invoiceService = new InvoiceService(mockTaxService.Object, mockDiscountService.Object);

        // Utwórz monitor dla mockTaxService
        var monitor = Mock.Get(mockTaxService.Object);

        // Act
        var result = invoiceService.CalculateInvoiceAmount(1, "Any");

        // Assert
        result.Should().Be(95); // 100 - 10 + 5

        // Użyj monitora, aby sprawdzić, czy metoda GetTax została wywołana
        monitor.Invocations.Should().ContainSingle(i => i.Method.Name == "GetTax");

        // Możesz także użyć monitora do sprawdzenia, czy metoda została wywołana z określonym argumentem
        monitor.Invocations.Should().Contain(i => i.Method.Name == "GetTax" && (decimal)i.Arguments[0] == 90);
    }
}