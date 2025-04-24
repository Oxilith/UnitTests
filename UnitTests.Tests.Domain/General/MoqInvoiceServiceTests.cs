using FluentAssertions;
using Moq;
using UnitTests.Domain.Interfaces;
using UnitTests.Domain.Services;

namespace UnitTests.Tests.Domain;

public class MoqInvoiceServiceTests
{
    private Mock<IDiscountService> _discountServiceMock;
    private Mock<ITaxService> _taxServiceMock;
    
    private InvoiceService _invoiceService;

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
        
        _discountServiceMock.Setup(x => 
                x.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>()))
            .Returns(20m)
            .Callback<decimal, string>((amount, customerType) =>
            {
                var discount = 0.2m;
                capturedDiscount = amount * discount;
            });

        // Act
        _invoiceService.CalculateTotal(100m, "Regular");

        // Assert
        capturedDiscount.Should().Be(20m);
    }

    [Test]
    public void CalculateInvoiceAmount_UsesTaxAndDiscountServices()
    {
        // Arrange
        _discountServiceMock.Setup(s => s.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>())).Returns(10);
        _taxServiceMock.Setup(s => s.GetTax(It.IsAny<decimal>())).Returns(5);
        
        // Act
        var result = _invoiceService.CalculateInvoiceAmount(1, "Any");

        // Assert
        result.Should().Be(95); // 100 - 10 + 5
        _discountServiceMock.Verify(s => s.CalculateDiscount(100, "Any"), Times.Once);
        _taxServiceMock.Verify(s => s.GetTax(90), Times.Once);
    }

    [Test]
    public void CalculateInvoiceAmount_ThrowsWhenTaxServiceFails()
    {
        // Arrange
        _taxServiceMock.Setup(s => s.GetTax(It.IsAny<decimal>())).Throws(new InvalidOperationException("Tax calculation failed"));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _invoiceService.CalculateInvoiceAmount(1, "Any"), "Tax calculation failed");
    }

    [Test]
    public void CalculateInvoiceAmount_CallsServicesInSequence()
    {
        // Arrange
        var sequence = new MockSequence();

        _discountServiceMock.InSequence(sequence)
            .Setup(s => s.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>())).Returns(10);
        _taxServiceMock.InSequence(sequence).Setup(s => s.GetTax(It.IsAny<decimal>())).Returns(5);

        // Act
        var result = _invoiceService.CalculateInvoiceAmount(1, "Any");

        // Assert
        result.Should().Be(95); // 100 - 10 + 5
    }

    [Test]
    public void CalculateInvoiceAmount_UsesMonitorToTrackMethodCalls()
    {
        // Arrange
        _discountServiceMock.Setup(s => s.CalculateDiscount(It.IsAny<decimal>(), It.IsAny<string>())).Returns(10);
        _taxServiceMock.Setup(s => s.GetTax(It.IsAny<decimal>())).Returns(5);

        // Act
        var result = _invoiceService.CalculateInvoiceAmount(1, "Any");

        // Assert
        result.Should().Be(95); // 100 - 10 + 5

        // Użyj monitora, aby sprawdzić, czy metoda GetTax została wywołana
        _taxServiceMock.Invocations.Should().ContainSingle(i => i.Method.Name == "GetTax");

        // Możesz także użyć monitora do sprawdzenia, czy metoda została wywołana z określonym argumentem
        _taxServiceMock.Invocations.Should().Contain(i => i.Method.Name == "GetTax" && (decimal)i.Arguments[0] == 90);
    }
}