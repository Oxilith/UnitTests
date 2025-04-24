namespace UnitTests.Domain.General.Interfaces;

public interface IDateRange
{
    DateTime Start { get; }
    DateTime End { get; }
    bool AreDatesInRange(List<DateTime> dates);
    bool OnlyFirstInRange(List<DateTime> dates);
}