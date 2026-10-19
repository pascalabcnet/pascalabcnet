namespace ReflectionCacheUnloadFixture
{
    public sealed class ConversionValue
    {
        public int Value { get; }
        public ConversionValue(int value) { Value = value; }
        public static implicit operator int(ConversionValue value) => value.Value;
        public static implicit operator ConversionValue(int value) => new ConversionValue(value);
        public static explicit operator string(ConversionValue value) => value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        public static explicit operator ConversionValue(string value) => new ConversionValue(int.Parse(value, System.Globalization.CultureInfo.InvariantCulture));
    }

    public sealed class SecondConversionValue
    {
        public int Value { get; }
        public SecondConversionValue(int value) { Value = value; }
        public static implicit operator int(SecondConversionValue value) => value.Value + 100;
    }
}
