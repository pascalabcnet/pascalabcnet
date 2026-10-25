using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.Json;

if (args.Length != 1) throw new ArgumentException("Usage: PortablePdbTests <prepared modern runtime>");
AlignmentChecks.Run();
string runtime = Path.GetFullPath(args[0]);
string root = Path.Combine(Path.GetTempPath(), "PABC PDB Кириллица " + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(root);
var controllerInfo = new ProcessStartInfo("dotnet") { WorkingDirectory = runtime,
    UseShellExecute = false, CreateNoWindow = true, RedirectStandardInput = true,
    RedirectStandardOutput = true, RedirectStandardError = true };
controllerInfo.ArgumentList.Add(Path.Combine(runtime, "PABCCompilerController.dll"));
using var controller = Process.Start(controllerInfo)!;
Task<string> controllerErrors = controller.StandardError.ReadToEndAsync();
int id = 0;
try
{
    var cases = new[] {
        ("ReadLines", "begin\n  foreach var s in ReadLines('missing-23.txt') do\n  begin\n  end;\nend.", 2),
        ("Nested", "function Fail: integer;\nbegin\n  Result := 10 div integer.Parse('0');\nend;\nbegin\n  Print(Fail);\nend.", 3),
        ("Branches", "begin\n  var n := integer.Parse('0');\n  if n = 1 then Print(1);\n  if n = 0 then\n  begin\n    raise new System.Exception('branch');\n  end\n  else Print(2);\nend.", 6),
        ("Lambda", "begin\n  var p: procedure := procedure ->\n  begin\n    raise new System.Exception('lambda');\n  end;\n  p;\nend.", 4)
    };
    foreach (var (name, source, line) in cases)
    {
        string directory = Path.Combine(root, name);
        Directory.CreateDirectory(directory);
        string file = Path.Combine(directory, name + ".pas");
        File.WriteAllText(file, source);
        string exe = await Compile(file, directory, true);
        ValidatePdb(exe, file, line);
        await RunException(exe, file, line);
    }

    string unitDirectory = Path.Combine(root, "Unit");
    Directory.CreateDirectory(unitDirectory);
    string unit = Path.Combine(unitDirectory, "PdbUnit.pas");
    File.WriteAllText(unit, "unit PdbUnit;\ninterface\nprocedure FailInUnit;\nimplementation\nprocedure FailInUnit;\nbegin\n  raise new System.Exception('unit');\nend;\nend.");
    string unitMain = Path.Combine(unitDirectory, "Main.pas");
    File.WriteAllText(unitMain, "uses PdbUnit;\nbegin\n  FailInUnit;\nend.");
    string unitExe = await Compile(unitMain, unitDirectory, true);
    ValidatePdb(unitExe, unit, 7);
    await RunException(unitExe, unit, 7);

    string libraryDirectory = Path.Combine(root, "Library");
    Directory.CreateDirectory(libraryDirectory);
    string library = Path.Combine(libraryDirectory, "PdbLibrary.pas");
    File.WriteAllText(library, "library PdbLibrary;\nprocedure FailInLibrary;\nbegin\n  raise new System.Exception('library');\nend;\nend.");
    string libraryDll = await Compile(library, libraryDirectory, true);
    ValidatePdb(libraryDll, library, 4);
    string libraryMain = Path.Combine(libraryDirectory, "Main.pas");
    File.WriteAllText(libraryMain, "{$reference PdbLibrary.dll}\nbegin\n  FailInLibrary;\nend.");
    string libraryExe = await Compile(libraryMain, libraryDirectory, true);
    ValidatePdb(libraryExe, libraryMain, 3);
    await RunException(libraryExe, library, 4);

    // A successful debug build also exercises loops, empty blocks, constructors and PCU code.
    string normalFile = Path.Combine(root, "Normal.pas");
    File.WriteAllText(normalFile, "type T = class constructor; begin end; end;\nbegin\n var a := new T;\n for var i := 1 to 2 do begin end;\n try Print(42); except Print(0); end;\nend.");
    string normalExe = await Compile(normalFile, root, true);
    ValidatePdb(normalExe, normalFile, 3);
    using (var normal = Process.Start(new ProcessStartInfo("dotnet", "\"" + normalExe + "\"") {
        WorkingDirectory = root, UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true })!)
    {
        Task<string> output = normal.StandardOutput.ReadToEndAsync(), errors = normal.StandardError.ReadToEndAsync();
        await normal.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(20));
        Check(normal.ExitCode == 0 && (await output).Trim() == "42" && (await errors).Length == 0, "ordinary debug program executes unchanged");
    }
    string releaseDirectory = Path.Combine(root, "release");
    Directory.CreateDirectory(releaseDirectory);
    string release = await Compile(normalFile, releaseDirectory, false);
    Check(!File.Exists(Path.ChangeExtension(release, ".pdb")), "release build still emits no PDB");
    Console.WriteLine("PASS all Portable PDB checks. Fixtures: " + root);
}
finally
{
    await controller.StandardInput.WriteLineAsync(JsonSerializer.Serialize(new { id = ++id, command = "shutdown" }));
    await controller.StandardInput.FlushAsync();
    try { await controller.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10)); }
    catch (TimeoutException) { controller.Kill(true); }
}

async Task<string> Compile(string file, string directory, bool debug)
{
    await controller.StandardInput.WriteLineAsync(JsonSerializer.Serialize(new { id = ++id, command = "compile",
        fileName = file, outputDirectory = directory, runtimeModule = "__RedirectIOMode", debug }));
    await controller.StandardInput.FlushAsync();
    string? response = await controller.StandardOutput.ReadLineAsync().WaitAsync(TimeSpan.FromSeconds(60));
    Check(response != null, "controller returned a response");
    using var json = JsonDocument.Parse(response!);
    Check(json.RootElement.GetProperty("success").GetBoolean(), "compile " + file + ": " + response);
    return json.RootElement.GetProperty("outputFile").GetString()!;
}

static async Task RunException(string exe, string file, int line)
{
    var info = new ProcessStartInfo("dotnet") { WorkingDirectory = Path.GetDirectoryName(exe), UseShellExecute = false,
        CreateNoWindow = true, RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true };
    info.ArgumentList.Add(exe);
    info.ArgumentList.Add("[REDIRECTIOMODE]");
    using var process = Process.Start(info)!;
    Task<string> stdout = process.StandardOutput.ReadToEndAsync(), stderr = process.StandardError.ReadToEndAsync();
    await process.StandardInput.WriteLineAsync("GO");
    await process.StandardInput.FlushAsync();
    try { await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(20)); }
    catch (TimeoutException) { process.Kill(true); throw; }
    string error = await stderr;
    Check(process.ExitCode != 0 && error.Contains("[EXCEPTION]"), "real exception reported: " + file);
    Check(error.Contains(file + ":line " + line) || error.Contains(file + ":строка " + line),
        "real stack identifies Pascal line " + line + ": " + error);
    Console.WriteLine("PASS stack: " + error.Split('\n').First(s => s.Contains(file)));
}

static void ValidatePdb(string exe, string source, int expectedLine)
{
    using var peStream = File.OpenRead(exe);
    using var pe = new PEReader(peStream);
    Check(pe.TryOpenAssociatedPortablePdb(exe, path => File.Exists(path) ? File.OpenRead(path) : null,
        out var provider, out _), "PE locates its matching Portable PDB");
    using (provider)
    {
        var reader = provider!.GetMetadataReader();
        Check(reader.MethodDebugInformation.Count == pe.GetMetadataReader().MethodDefinitions.Count,
            "PDB keeps an aligned row for every method, including external methods");
        bool found = false;
        int count = 0;
        foreach (var handle in reader.MethodDebugInformation)
        {
            int previousOffset = -1;
            foreach (var point in reader.GetMethodDebugInformation(handle).GetSequencePoints())
            {
                Check(point.Offset > previousOffset, "sequence point offsets strictly increase");
                previousOffset = point.Offset;
                Check(!point.Document.IsNil, "sequence point has a source document");
                string document = reader.GetString(reader.GetDocument(point.Document).Name);
                if (!point.IsHidden)
                {
                    Check(point.StartLine > 0 && point.StartLine < 0xFEEFEE, "normal Pascal source line is valid");
                    if (document == source && point.StartLine == expectedLine) found = true;
                }
                count++;
            }
        }
        Check(found, "PDB contains " + source + ":" + expectedLine);
        Console.WriteLine("PASS decoded all " + count + " sequence points: " + Path.GetFileName(source));
    }
}

static void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
}
