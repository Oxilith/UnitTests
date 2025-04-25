namespace UnitTests.Tests.Domain.MovieTheaterUseCase;

public class HallRowsTests
{
    private TestDataProvider _dataProvider;

    [SetUp]
    public void SetUp()
    {
        _dataProvider = new TestDataProvider();
    }

    [Test]
    public void RowShouldBeImmutable()
    {
        Assert.That(true, Is.False);
    }
}