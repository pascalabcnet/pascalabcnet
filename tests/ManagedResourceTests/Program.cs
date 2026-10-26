using System.Buffers.Binary;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

string repo = Path.GetFullPath(args.Length == 1 ? args[0] : Path.Combine(AppContext.BaseDirectory, "../../../../../"));
string root = Path.Combine(Path.GetTempPath(), "PABC ресурсы " + Guid.NewGuid().ToString("N"));
foreach (string target in new[] { "net472", "net10" })
{
    string dir = Path.Combine(root, target);
    Directory.CreateDirectory(Path.Combine(dir, "UsesInUnits3"));
    foreach (string name in new[] { "u.pas", "uui3_lib.pas", "1.txt" })
        File.Copy(Path.Combine(repo, "TestSuite/usesunits/UsesInUnits3", name), Path.Combine(dir, "UsesInUnits3", name));
    File.Copy(Path.Combine(repo, "TestSuite/usesunits/uses_in3_derectives.pas"), Path.Combine(dir, "uses_in3_derectives.pas"));
    await Compile("UsesInUnits3/uui3_lib.pas", "UsesInUnits3");
    string original = await Compile("uses_in3_derectives.pas");
    Validate(original, new() { ["1.txt"] = File.ReadAllBytes(Path.Combine(dir, "UsesInUnits3/1.txt")) });
    await Run(original);
    Console.WriteLine($"PASS {target}: original uses_in3_derectives executes with 1.txt");

    var resources = new Dictionary<string, byte[]> {
        ["a.bin"] = new byte[] { 0, 255, 1, 2, 128 },
        ["пустой.bin"] = Array.Empty<byte>(),
        ["текст.txt"] = System.Text.Encoding.UTF8.GetBytes("Привет, ресурсы!")
    };
    foreach (var pair in resources) File.WriteAllBytes(Path.Combine(dir, pair.Key), pair.Value);
    string directives = string.Join("\n", resources.Keys.Select(name => "{$resource '" + name + "'}"));
    string checks = string.Join("\n", resources.Select(pair =>
        "begin var s := System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream('" + pair.Key + "'); " +
        "assert(s <> nil); assert(s.Length = " + pair.Value.Length + "); " +
        string.Join(" ", pair.Value.Select(value => "assert(s.ReadByte = " + value + ");")) + " s.Close; end;"));
    File.WriteAllText(Path.Combine(dir, "Resources.pas"), directives + "\nbegin\n" + checks + "\nend.");
    string exe = await Compile("Resources.pas");
    Validate(exe, resources);
    await Run(exe);
    Console.WriteLine($"PASS {target}: multiple resources, binary bytes, empty resource, Unicode names");

    File.WriteAllText(Path.Combine(dir, "ResourceLibrary.pas"), "library ResourceLibrary;\n" + directives +
        "\nprocedure Verify;\nbegin\n" + checks + "\nend;\nend.");
    string dll = await Compile("ResourceLibrary.pas");
    Validate(dll, resources);
    File.WriteAllText(Path.Combine(dir, "LibraryClient.pas"), "{$reference ResourceLibrary.dll}\nbegin Verify; end.");
    string client = await Compile("LibraryClient.pas");
    Validate(client, new());
    await Run(client);
    Console.WriteLine($"PASS {target}: library resources and executable without resources");

    async Task<string> Compile(string relative, string outputRelative = "")
    {
        string runtime = Path.Combine(repo, target == "net10" ? "bin-net10" : "bin");
        string compiler = Path.Combine(runtime, target == "net10" ? "pabcnetcclear.dll" : "pabcnetcclear.exe");
        string source = Path.Combine(dir, relative);
        string output = Path.Combine(dir, outputRelative, Path.GetFileNameWithoutExtension(relative) +
            (Path.GetFileName(relative) is "ResourceLibrary.pas" or "uui3_lib.pas" ? ".dll" : ".exe"));
        await ProcessRun(target == "net10" ? "dotnet" : compiler,
            target == "net10" ? new[] { compiler, "/Debug:1", "/Output:" + output, source } :
                new[] { "/Debug:1", "/Output:" + output, source }, dir);
        if (!File.Exists(output)) throw new Exception("Missing output: " + output);
        return output;
    }

    Task Run(string file) => ProcessRun(target == "net10" ? "dotnet" : file,
        target == "net10" ? new[] { file } : Array.Empty<string>(), dir);
}
Console.WriteLine("PASS all managed resource checks. Temporary files: " + root);

static void Validate(string file, Dictionary<string, byte[]> expected)
{
    using var stream = File.OpenRead(file);
    using var pe = new PEReader(stream);
    var metadata = pe.GetMetadataReader();
    if (metadata.ManifestResources.Count != expected.Count) throw new Exception("Wrong resource count: " + file);
    foreach (var handle in metadata.ManifestResources)
    {
        var resource = metadata.GetManifestResource(handle);
        string name = metadata.GetString(resource.Name);
        if (!resource.Implementation.IsNil || resource.Attributes != ManifestResourceAttributes.Public ||
            !expected.TryGetValue(name, out var bytes))
            throw new Exception($"Invalid manifest resource: {name}, implementation={resource.Implementation.Kind}/{resource.Implementation.IsNil}, attributes={resource.Attributes}, offset={resource.Offset}, file={file}");
        var content = pe.GetSectionData(pe.PEHeaders.CorHeader!.ResourcesDirectory.RelativeVirtualAddress)
            .GetContent(checked((int)resource.Offset), 4 + bytes.Length).ToArray();
        if (BinaryPrimitives.ReadInt32LittleEndian(content) != bytes.Length || !content.AsSpan(4).SequenceEqual(bytes))
            throw new Exception("Invalid resource bytes: " + name);
    }
}

static async Task ProcessRun(string file, IEnumerable<string> arguments, string directory)
{
    var start = new ProcessStartInfo(file) { WorkingDirectory = directory, UseShellExecute = false,
        CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true };
    foreach (string argument in arguments) start.ArgumentList.Add(argument);
    using var process = Process.Start(start)!;
    Task<string> output = process.StandardOutput.ReadToEndAsync(), errors = process.StandardError.ReadToEndAsync();
    try { await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(60)); }
    catch (TimeoutException) { process.Kill(true); throw; }
    if (process.ExitCode != 0) throw new Exception(file + " failed: " + await output + "\n" + await errors);
}
