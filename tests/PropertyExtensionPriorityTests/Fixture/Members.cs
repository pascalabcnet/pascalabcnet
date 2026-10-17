namespace PropertyExtensionFixture
{
    public class Gauge
    {
        public int? Width { get; set; }
        public string Label { get; set; }
        public int ReadOnly { get { return 42; } }
    }

    public class DerivedGauge : Gauge { }

    public static class GaugeExtensions
    {
        public static Gauge Width(this Gauge gauge, int value)
        {
            gauge.Width = value;
            return gauge;
        }

        public static Gauge Width(this Gauge gauge, int value, bool twice)
        {
            gauge.Width = twice ? value * 2 : value;
            return gauge;
        }

        public static Gauge Label(this Gauge gauge, string value)
        {
            gauge.Label = value;
            return gauge;
        }

        public static Gauge ReadOnly(this Gauge gauge, int value) { return gauge; }
        public static Gauge OnlyMethod(this Gauge gauge, int value) { return gauge; }
    }
}
