namespace UnitTests.Domain.Extensions;

public static class EnumerableExtensions
{
    public static bool CheckForaDuplicates<T>(this IEnumerable<T>? rows, Func<T, object> selector) where T : class
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(selector);

        var enumerable = rows.ToList();

        return enumerable.Any() && enumerable.GroupBy(selector).Any(g => g.Count() > 1);
    }
}