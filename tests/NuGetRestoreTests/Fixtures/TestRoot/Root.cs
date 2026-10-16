namespace NuGetRestoreFixture;

public static class Root
{
    public static int Value => Dependency.Value + 1;
    public static int Combine(int left, int right) => left + right;
}
