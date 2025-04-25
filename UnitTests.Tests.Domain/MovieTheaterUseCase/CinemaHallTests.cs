using FluentAssertions;
using UnitTests.Domain.MovieTheaterUseCase.Exceptions;

namespace UnitTests.Tests.Domain.MovieTheaterUseCase;

public class CinemaHallTests
{
    private TestDataProvider _dataProvider;

    [SetUp]
    public void SetUp()
    {
        _dataProvider = new TestDataProvider();
    }


    [Test]
    public void CinemaHallFirstRowShouldHaveNumberOne()
    {
        Assert.That(true, Is.False);
    }

    [Test]
    public void CinemaHallRowsShouldHaveConsecutiveNumbers()
    {
        Assert.That(true, Is.False);
    }


    [Test]
    public void CannotCreateCinemHallWhenRowCountIsIncorrect()
    {
        // Arrange & Act
        Action act = () => _dataProvider.GetEmptyCinemaHall();
        Action act2 = () => _dataProvider.GetLengthyCinemaHall();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Cinema hall should have at least 1 row.");
        act2.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Cinema hall should have at most 9 rows.");
    }

    [Test]
    public void CannotCreateCinemHallWhenRowsSeatCountDoesNotIncreaseWithEachRow()
    {
        // Arrange & Act
        Action act = () => _dataProvider.GetCinemaHallWithWrongSeatCountInRows();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Rows seat count should increase with each row.");
    }
}