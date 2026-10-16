namespace NuGetRestoreFixture;

public static class Root
{
    public static int Value => Dependency.Value + 2;
    public static int Combine(int left, int right) => left + right;
    public static int OnlyInV2 => 44;
}
