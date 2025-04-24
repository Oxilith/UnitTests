using UnitTests.Domain;

namespace UnitTests.Interface;

internal class Program
{
    private static void Main(string[] args)
    {
        var range = new DateRange(DateTime.MinValue, DateTime.MaxValue);

        Console.WriteLine(range.Start.ToString("G"));
        Console.WriteLine(range.End.ToString("G"));
    }
}