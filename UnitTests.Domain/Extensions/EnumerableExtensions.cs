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

    public static List<List<int>> SplitByGaps(this IEnumerable<int>? rows)
    {
        ArgumentNullException.ThrowIfNull(rows);

        var enumerable = rows.ToList();
        var lists = new List<List<int>>();

        if (!enumerable.Any()) return lists;

        if (enumerable.Count == 1) return new List<List<int>> { enumerable };

        var first = enumerable.First();
        foreach (var number in enumerable.Skip(1))
            if (Math.Abs(number - first) != 1)
            {
                lists.Add(new List<int> { number });
                first = number;
            }
            else
            {
                lists.Last().Add(number);
            }

        return lists;
    }
}