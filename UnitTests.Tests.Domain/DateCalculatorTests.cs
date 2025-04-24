using FluentAssertions;

namespace UnitTests.Tests.Domain;

public class DateCalculator
{
    private DateCalculator _calculator;
    public int Counter;

    public int OnceCounter;

    public DateTime GetNextBusinessDay(DateTime date)
    {
        do
        {
            date = date.AddDays(1);
        } while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);

        return date;
    }

    [OneTimeSetUp]
    public void OnceTestSetUp()
    {
        OnceCounter++;
    }

    [SetUp]
    public void TestSetUp()
    {
        Counter++;
        _calculator = new DateCalculator();
    }

    [Test]
    public void GetNextBusinessDay_ShouldSkipWeekends()
    {
        Console.WriteLine("Counter: " + Counter + " OnceCounter: " + OnceCounter);

        // Piątek -> Następny dzień roboczy to poniedziałek
        _calculator.GetNextBusinessDay(new DateTime(2024, 2, 23)).Should().Be(new DateTime(2024, 2, 26));

        // Sobota -> Następny dzień roboczy to poniedziałek
        _calculator.GetNextBusinessDay(new DateTime(2024, 2, 24)).Should().Be(new DateTime(2024, 2, 26));

        // Niedziela -> Następny dzień roboczy to poniedziałek
        _calculator.GetNextBusinessDay(new DateTime(2024, 2, 25)).Should().Be(new DateTime(2024, 2, 26));
    }

    [Test]
    public void GetNextBusinessDay_ShouldHandleLeapYear()
    {
        Console.WriteLine("Counter: " + Counter + " OnceCounter: " + OnceCounter);

        // Test w roku przestępnym, 28 lutego -> Następny dzień roboczy to 29 lutego
        _calculator.GetNextBusinessDay(new DateTime(2024, 2, 28)).Should().Be(new DateTime(2024, 2, 29));
    }

    [Test]
    public void GetNextBusinessDay_ShouldNotChangeTimePart()
    {
        Console.WriteLine("Counter: " + Counter + " OnceCounter: " + OnceCounter);

        // Test zachowania części czasowej
        var time = new DateTime(2024, 2, 23, 15, 30, 0); // 15:30
        _calculator.GetNextBusinessDay(time).TimeOfDay.Should().Be(time.TimeOfDay);
    }

    [Test]
    public void GetNextBusinessDay_ShouldBeAfterInputDate([Random(1900, 2100, 1)] int year,
        [Random(1, 12, 1)] int month, [Random(1, 28, 1)] int day)
    {
        var date = new DateTime(year, month, day);
        _calculator.GetNextBusinessDay(date).Should().BeAfter(date);
    }

    [Test]
    public void GetNextBusinessDay_ShouldBeCloseToInputDate()
    {
        Console.WriteLine("Counter: " + Counter + " OnceCounter: " + OnceCounter);

        // Test, że zwracana data jest blisko podanej daty (nie więcej niż 3 dni później)
        var date = new DateTime(2024, 2, 23);
        _calculator.GetNextBusinessDay(date).Should().BeCloseTo(date, TimeSpan.FromDays(3));
    }

    [Test]
    public void GetNextBusinessDay_ShouldBeOnWeekday()
    {
        Console.WriteLine("Counter: " + Counter + " OnceCounter: " + OnceCounter);

        // Test, że zwracana data jest dniem roboczym
        var date = new DateTime(2024, 2, 23);
        var nextBusinessDay = _calculator.GetNextBusinessDay(date);
        nextBusinessDay.DayOfWeek.Should().NotBe(DayOfWeek.Saturday).And.NotBe(DayOfWeek.Sunday);
    }

    [Test]
    public void GetNextBusinessDay_ShouldReturnOneOfExpectedDays()
    {
        Console.WriteLine("Counter: " + Counter + " OnceCounter: " + OnceCounter);

        // Załóżmy, że testujemy dla daty piątkowej
        var friday = new DateTime(2024, 2, 23);

        // Następne dni robocze to poniedziałek i wtorek
        var expectedNextBusinessDays = new[]
        {
            new DateTime(2024, 2, 26), // Poniedziałek
            new DateTime(2024, 2, 27) // Wtorek
        };

        // Metoda GetNextBusinessDay powinna zwrócić jeden z oczekiwanych dni roboczych
        var nextBusinessDay = _calculator.GetNextBusinessDay(friday);
        nextBusinessDay.Should().BeOneOf(expectedNextBusinessDays);
    }
}