namespace UnitTests.Domain.Services;

public class DateCalculator
{
    public DateTime GetNextBusinessDay(DateTime date)
    {
        do
        {
            date = date.AddDays(1);
        } while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);

        return date;
    }
}