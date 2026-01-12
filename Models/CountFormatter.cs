using System.Globalization;

namespace SearchHitCounter.Models
{
    public static class CountFormatter
    {
        public static string Format(long value) => value switch
        {
            < 1_000 => value.ToString(CultureInfo.InvariantCulture),
            < 1_000_000 => FormatTotalResult(value, 1_000, "k"),
            < 1_000_000_000 => FormatTotalResult(value, 1_000_000, "M"),
            _ => FormatTotalResult(value, 1_000_000_000, "mdr"),
        };

        private static string FormatTotalResult(long value, long unit, string unitSuffix)
        {
            var scaled = value / (double)unit;
            return scaled.ToString("0.0#", CultureInfo.InvariantCulture) + unitSuffix;
        }
    }
}
