using UnitTests.Domain;

namespace UnitTests.Console;

internal class Program
{
    private static void Main(string[] args)
    {
        var range = new DateRange(DateTime.MinValue, DateTime.MaxValue);

        System.Console.WriteLine(range.Start.ToString("G"));
        System.Console.WriteLine(range.End.ToString("G"));
    }
}