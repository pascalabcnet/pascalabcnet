using System.Reflection;
using System.Runtime.Loader;
using System.Runtime.Versioning;

if (args.Length != 3) throw new ArgumentException("Usage: ClassLibraryChecks <original> <rebuilt> <net472|net10>");
Check(args[0], ".NETFramework,Version=v4.7.2");
Check(args[1], args[2] == "net10" ? ".NETCoreApp,Version=v10.0" : ".NETFramework,Version=v4.7.2");
Console.WriteLine("PASS original and rebuilt ClassLibrary1: plain CLR struct, public fields, constructor, target framework");

static void Check(string path, string framework)
{
    var context = new AssemblyLoadContext(Guid.NewGuid().ToString(), isCollectible: true);
    try
    {
        var assembly = context.LoadFromAssemblyPath(Path.GetFullPath(path));
        var name = assembly.GetName();
        Require(name.Name == "ClassLibrary1" && name.Version == new Version(1, 0, 0, 0), "assembly identity");
        Require(assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName == framework, "target framework");
        var types = assembly.GetTypes();
        Require(types.Length == 1 && types[0].FullName == "ClassLibrary1.Rec1", "single original type");
        var type = types[0];
        Require(type.IsPublic && type.IsValueType && type.IsLayoutSequential, "plain public struct");
        const BindingFlags declared = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                      BindingFlags.Static | BindingFlags.DeclaredOnly;
        var fields = type.GetFields(declared).OrderBy(f => f.Name).ToArray();
        Require(fields.Length == 2 && fields[0].Name == "a" && fields[1].Name == "b" &&
                fields.All(f => f.FieldType == typeof(int) && f.IsPublic && !f.IsStatic && !f.IsInitOnly), "original fields");
        Require(type.GetMethods(declared).Length == 0 && type.GetProperties(declared).Length == 0,
            "no added equality operators or generated record methods");
        var constructors = type.GetConstructors(declared);
        Require(constructors.Length == 1 && constructors[0].IsPublic &&
                constructors[0].GetParameters().Select(p => p.ParameterType).SequenceEqual(new[] { typeof(int), typeof(int) }),
            "original constructor signature");
        object value = constructors[0].Invoke(new object[] { 17, -9 });
        Require((int)fields[0].GetValue(value)! == 17 && (int)fields[1].GetValue(value)! == -9, "constructor assignments");
        object equal = constructors[0].Invoke(new object[] { 17, -9 });
        object differentA = constructors[0].Invoke(new object[] { 18, -9 });
        object differentB = constructors[0].Invoke(new object[] { 17, -8 });
        Require(value.Equals(equal) && !value.Equals(differentA) && !value.Equals(differentB), "CLR value equality");
        Require(!assembly.GetReferencedAssemblies().Any(a => a.Name is "PABCRtl" or "PABCSystem"), "no Pascal implementation substituted");
    }
    finally { context.Unload(); }
}

static void Require(bool condition, string check)
{
    if (!condition) throw new Exception("ClassLibrary1 contract mismatch: " + check);
}
