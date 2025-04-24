namespace UnitTests.Domain;

public class DateRange : IDateRange
{
    public DateRange(DateTime start, DateTime end)
    {
        if (start >= end) throw new InvalidOperationException("Poczatkowa data powinna byc mniejsza od koncowej");

        Start = start;
        End = end;
    }

    public DateTime Start { get; }
    public DateTime End { get; }


    public virtual bool AreDatesInRange(List<DateTime> dates)
    {
        return dates.All(x => Start <= x && x <= End);
    }

    public virtual bool OnlyFirstInRange(List<DateTime> dates)
    {
        var first = dates.FirstOrDefault();
        var firstIsNotInRange = Start > first || first > End;
        if (first == default || firstIsNotInRange) return false;
        if (dates.Count == 1) return true;

        var restOfDates = dates.Skip(1);
        return !AreDatesInRange(restOfDates.ToList());
    }
}