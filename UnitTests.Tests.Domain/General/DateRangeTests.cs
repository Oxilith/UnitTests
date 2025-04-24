using System.Diagnostics;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using UnitTests.Domain;

namespace UnitTests.Tests.Domain;

[TestFixture]
public class DateRangeTests
{
    private readonly Mock<IDateRange> _dateRangeMock = new();

    [TestCase("01/01/2000", "01/01/1001")]
    [TestCase("01/01/2000", "01/01/2001")]
    public void mocking(string startDate, string endDate)
    {
        _dateRangeMock.Setup(x =>
                x.AreDatesInRange(It.IsAny<List<DateTime>>()))
            .Returns(true);

        _dateRangeMock.Setup(x => x.End)
            .Returns(DateTime.UtcNow.AddHours(10));

        _dateRangeMock.Setup(x => x.Start)
            .Returns(DateTime.UtcNow);

        // Arrange & Act
        var sut = _dateRangeMock.Object;

        // Assert
        Assert.That(sut.End, Is.GreaterThan(sut.Start));
        Assert.That(sut.AreDatesInRange(new List<DateTime> { DateTime.Parse(startDate), DateTime.Parse(endDate) }),
            Is.True);
    }

    [TestCase("01/01/2000", "01/01/2001")]
    public void StartDateShouldBeBeforeEndDate(string startDate, string endDate)
    {
        var sut = new DateRange(DateTime.Parse(startDate), DateTime.Parse(endDate));
        // Assert
        Assert.That(sut.End, Is.GreaterThan(sut.Start));
    }

    [TestCase("01/01/2001", "01/01/2000")]
    [TestCase("01/01/2001", "01/01/2001")]
    public void ShouldThrowInvalidOperationExceptionWhenTheStartDateIsAfterOrEqualEndDate(string startDate,
        string endDate)
    {
        Action act = () => new DateRange(DateTime.Parse(startDate), DateTime.Parse(endDate));
        act.Should().ThrowExactly<InvalidOperationException>("Poczatkowa data powinna byc mniejsza od koncowej");
    }

    public static IEnumerable<TestCaseData> Lista
    {
        get
        {
            yield return new TestCaseData(
                new List<DateTime>
                {
                    new(2025, 04, 23),
                    new(2025, 04, 01)
                }
            ).SetName("ListaDobra");

            yield return new TestCaseData(
                new List<DateTime>
                {
                    new(2025, 04, 03)
                }
            ).SetName("DrugaListaDobra");
        }
    }

    [TestCaseSource(nameof(Lista))]
    public void ShouldHandleDateLists(List<DateTime> dates)
    {
        var maxTime = 1000;
        var startDate = new DateTime(2025, 04, 01);
        var sut = new DateRange(startDate, startDate.AddMonths(2));
        var sw = Stopwatch.StartNew();

        Assert.That(sut.AreDatesInRange(dates), Is.True);
        sw.Stop();
        Assert.That(sw.ElapsedMilliseconds, Is.LessThanOrEqualTo(maxTime));

        var date = 20.March(2025);
    }
}