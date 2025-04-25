using FluentAssertions;
using UnitTests.Domain.MovieTheaterUseCase.Entities;
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
        // Arrange
        var cinemaHall = _dataProvider.GetCorrectCinemaHall();

        // Act
        var firstRow = cinemaHall.Rows.First();

        // Assert
        firstRow.Number.Should().Be(CinemaHall.ExpectedFirstRowNumber);
    }

    [Test]
    public void CinemaHallShouldThrowsWhenFirstRowDoesNotHaveNumberOne()
    {
        // Arrange & Act
        var act = () => _dataProvider.GetCinemaHallWithWrongFirstRowNumber();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage($"Cinema hall first row should have number {CinemaHall.ExpectedFirstRowNumber}.");
    }

    [Test]
    public void CinemaHallRowsShouldHaveConsecutiveNumbers()
    {
        // Arrange && Act
        var act = () => _dataProvider.GetCorrectCinemaHall();

        // Assert
        act.Should().NotThrow<BusinessRuleViolationException>();
    }

    [Test]
    public void CinemaHallShouldThrowsWhenRowsAreNotInOrder()
    {
        // Arrange && Act
        var act = () => _dataProvider.GetCinemaHallWithRowsInWrongOrder();

        // Assert

        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage("Cinema hall rows should be in order.");
    }


    [Test]
    public void CannotCreateCinemHallWhenRowCountIsIncorrect()
    {
        // Arrange & Act
        Action act = () => _dataProvider.GetEmptyCinemaHall();
        Action act2 = () => _dataProvider.GetLengthyCinemaHall();

        // Assert
        act.Should().Throw<BusinessRuleViolationException>()
            .WithMessage($"Cinema hall should have at least {CinemaHall.MinRowCount} row.");
        act2.Should().Throw<BusinessRuleViolationException>()
            .WithMessage($"Cinema hall should have at most {CinemaHall.MaxRowCount} rows.");
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