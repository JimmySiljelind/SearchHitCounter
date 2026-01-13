using System.Globalization;

namespace SearchHitCounter.Helpers
{
    public static class CountFormatter
    {
        // Formaterar ett heltal till en sträng med förkortningar för tusen (k), miljon (M) och miljard (mdr).
        public static string Format(long value) => value switch
        {
            // Om värdet är mindre än X, returnera formaterat värde.
            < 1_000 => value.ToString(CultureInfo.InvariantCulture),
            < 1_000_000 => FormatTotalResult(value, 1_000, "k"),
            < 1_000_000_000 => FormatTotalResult(value, 1_000_000, "M"),
            _ => FormatTotalResult(value, 1_000_000_000, "mdr"),
        };

        private static string FormatTotalResult(long value, long unit, string unitSuffix)
        {
            // Dela värdet med enheten och formatera det med upp till två decimaler.
            var scaled = value / (double)unit;
            return scaled.ToString("0.##", CultureInfo.InvariantCulture) + unitSuffix;
        }
    }
}