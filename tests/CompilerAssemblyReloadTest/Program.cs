using System.Diagnostics;
using System.Text;
using PascalABCCompiler;

internal static class Program
{
    private const int Ready = ConsoleCompilerConstants.CommandStartNumber;
    private const int CompilationFinished = Ready + 14;
    private const string FixtureAssemblyName = "ReflectionCacheUnloadFixture.dll";
    private const string DependencyAssemblyName = "ReflectionCacheUnloadDependency.dll";
    private const string SecondFixtureAssemblyName = "ReflectionCacheUnloadSecondFixture.dll";
    private const string SecondDependencyAssemblyName = "ReflectionCacheUnloadSecondDependency.dll";
    private const string CommonAssemblyName = "ReflectionCacheUnloadCommon.dll";
    private const string LeafAssemblyName = "ReflectionCacheUnloadLeaf.dll";
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(60);

    public static async Task<int> Main(string[] args)
    {
        if (args.Length < 1 || args.Length > 2 ||
            (args.Length == 2 && !string.Equals(args[1], "--stress", StringComparison.Ordinal)))
        {
            Console.Error.WriteLine(
                "Usage: CompilerAssemblyReloadTest <pabcnetc.exe|pabcnetc.dll> [--stress]");
            return 2;
        }

        try
        {
            await Run(Path.GetFullPath(args[0]), args.Length == 2 ? 50 : 0);
            return 0;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error);
            return 1;
        }
    }

    private static async Task Run(string compilerPath, int stressIterations)
    {
        string repositoryRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
            "..", "..", "..", "..", ".."));
        string versionOnePath = Path.Combine(repositoryRoot, "tests", "ReflectionCacheUnloadFixture",
            "bin", "Debug", "net10.0", FixtureAssemblyName);
        string versionTwoPath = Path.Combine(repositoryRoot, "tests", "ReflectionCacheUnloadFixtureV2",
            "bin", "Debug", "net10.0", FixtureAssemblyName);
        string assemblyVersionTwoPath = Path.Combine(repositoryRoot, "tests",
            "ReflectionCacheUnloadFixtureVersion2", "bin", "Debug", "net10.0",
            FixtureAssemblyName);
        string dependencyVersionOnePath = Path.Combine(Path.GetDirectoryName(versionOnePath)!,
            DependencyAssemblyName);
        string dependencyVersionTwoPath = Path.Combine(Path.GetDirectoryName(versionTwoPath)!,
            DependencyAssemblyName);
        string secondFixtureSourcePath = Path.Combine(repositoryRoot, "tests",
            "ReflectionCacheUnloadSecondFixture", "bin", "Debug", "net10.0", SecondFixtureAssemblyName);
        string secondFixtureVersionTwoSourcePath = Path.Combine(repositoryRoot, "tests",
            "ReflectionCacheUnloadSecondFixtureV2", "bin", "Debug", "net10.0",
            SecondFixtureAssemblyName);
        string secondDependencySourcePath = Path.Combine(Path.GetDirectoryName(secondFixtureSourcePath)!,
            SecondDependencyAssemblyName);
        string commonVersionOneSourcePath = Path.Combine(repositoryRoot, "tests",
            "ReflectionCacheUnloadCommon", "bin", "Debug", "net10.0", CommonAssemblyName);
        string commonVersionTwoSourcePath = Path.Combine(repositoryRoot, "tests",
            "ReflectionCacheUnloadCommonV2", "bin", "Debug", "net10.0", CommonAssemblyName);
        string leafVersionOneSourcePath = Path.Combine(repositoryRoot, "tests",
            "ReflectionCacheUnloadLeaf", "bin", "Debug", "net10.0", LeafAssemblyName);
        string leafVersionTwoSourcePath = Path.Combine(repositoryRoot, "tests",
            "ReflectionCacheUnloadLeafV2", "bin", "Debug", "net10.0", LeafAssemblyName);
        if (!File.Exists(versionOnePath) || !File.Exists(versionTwoPath) ||
            !File.Exists(assemblyVersionTwoPath) ||
            !File.Exists(dependencyVersionOnePath) || !File.Exists(dependencyVersionTwoPath) ||
            !File.Exists(secondFixtureSourcePath) ||
            !File.Exists(secondFixtureVersionTwoSourcePath) ||
            !File.Exists(secondDependencySourcePath) ||
            !File.Exists(commonVersionOneSourcePath) || !File.Exists(commonVersionTwoSourcePath) ||
            !File.Exists(leafVersionOneSourcePath) || !File.Exists(leafVersionTwoSourcePath))
            throw new FileNotFoundException("Fixture assemblies were not built.");

        string workDirectory = Path.Combine(Path.GetTempPath(),
            "pabc-compiler-assembly-reload-" + Guid.NewGuid().ToString("N"));
        string firstDirectory = Path.Combine(workDirectory, "first");
        string secondDirectory = Path.Combine(workDirectory, "second");
        Directory.CreateDirectory(firstDirectory);
        Directory.CreateDirectory(secondDirectory);
        string fixturePath = Path.Combine(firstDirectory, FixtureAssemblyName);
        string dependencyPath = Path.Combine(firstDirectory, DependencyAssemblyName);
        string secondFixturePath = Path.Combine(secondDirectory, SecondFixtureAssemblyName);
        string secondDependencyPath = Path.Combine(secondDirectory, SecondDependencyAssemblyName);
        string runtimeDependencyPath = Path.Combine(workDirectory, DependencyAssemblyName);
        string runtimeSecondDependencyPath = Path.Combine(workDirectory, SecondDependencyAssemblyName);
        string commonPath = Path.Combine(firstDirectory, CommonAssemblyName);
        string runtimeCommonPath = Path.Combine(workDirectory, CommonAssemblyName);
        string leafPath = Path.Combine(firstDirectory, LeafAssemblyName);
        string runtimeLeafPath = Path.Combine(workDirectory, LeafAssemblyName);
        string statisticsPath = Path.Combine(workDirectory, "collectible-reference-stats.txt");
        string sourcePath = Path.Combine(workDirectory, "reload_test.pas");
        string sourceText = CreateSource(fixturePath, secondFixturePath,
            "VersionOneOnly", 1, "VersionOneOnly", 101,
            "VersionOneOnly", 404, "VersionOneOnly", 606);
        string expectedRuntimeOutput = "1\n101\n303\n404\n404\n606\n606";

        var start = new ProcessStartInfo
        {
            FileName = compilerPath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? "dotnet" : compilerPath,
            WorkingDirectory = Path.GetDirectoryName(compilerPath)!,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardInputEncoding = new UTF8Encoding(false),
            StandardOutputEncoding = new UTF8Encoding(false),
            StandardErrorEncoding = new UTF8Encoding(false)
        };
        if (start.FileName == "dotnet")
            start.ArgumentList.Add(compilerPath);
        start.ArgumentList.Add("commandmode");
        start.Environment["PABCNET_TEST_COLLECTIBLE_STATS_FILE"] = statisticsPath;

        using var process = new Process { StartInfo = start };
        Task<string>? errors = null;
        try
        {
            CopyFixture(versionOnePath, fixturePath, DateTime.UtcNow.AddMinutes(-4));
            CopyFixture(dependencyVersionOnePath, dependencyPath, DateTime.UtcNow.AddMinutes(-4));
            CopyFixture(secondFixtureSourcePath, secondFixturePath, DateTime.UtcNow.AddMinutes(-4));
            CopyFixture(secondDependencySourcePath, secondDependencyPath, DateTime.UtcNow.AddMinutes(-4));
            CopyFixture(dependencyVersionOnePath, runtimeDependencyPath, DateTime.UtcNow.AddMinutes(-4));
            CopyFixture(secondDependencySourcePath, runtimeSecondDependencyPath, DateTime.UtcNow.AddMinutes(-4));
            CopyFixture(commonVersionOneSourcePath, commonPath, DateTime.UtcNow.AddMinutes(-4));
            CopyFixture(commonVersionOneSourcePath, runtimeCommonPath, DateTime.UtcNow.AddMinutes(-4));
            CopyFixture(leafVersionOneSourcePath, leafPath, DateTime.UtcNow.AddMinutes(-4));
            CopyFixture(leafVersionOneSourcePath, runtimeLeafPath, DateTime.UtcNow.AddMinutes(-4));
            process.Start();
            errors = process.StandardError.ReadToEndAsync();
            int compilerProcessId = process.Id;

            async Task<(int Command, string Argument)> ReadAndHandle()
            {
                string frame = await ReadFrame(process.StandardOutput);
                if (frame.Length < 3 || !int.TryParse(frame.AsSpan(0, 3), out int command))
                    throw new InvalidDataException("Invalid protocol frame: " + frame);
                string argument = frame.Length > 3 ? frame.Substring(4) : "";
                switch (command)
                {
                    case ConsoleCompilerConstants.SourceFileText:
                        if (string.Equals(argument, sourcePath, StringComparison.OrdinalIgnoreCase))
                            SendText(process, ConsoleCompilerConstants.SourceFileText, sourceText);
                        else if (File.Exists(argument))
                            SendText(process, ConsoleCompilerConstants.SourceFileText,
                                File.ReadAllText(argument));
                        else
                            Send(process, ConsoleCompilerConstants.Error);
                        break;
                    case ConsoleCompilerConstants.FileExsist:
                        Send(process, command, argument == sourcePath || File.Exists(argument));
                        break;
                    case ConsoleCompilerConstants.GetLastWriteTime:
                        Send(process, command, argument == sourcePath
                            ? DateTime.UtcNow.Ticks
                            : File.GetLastWriteTime(argument).Ticks);
                        break;
                }
                return (command, argument);
            }

            while ((await ReadAndHandle()).Command != Ready) { }
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            SendText(process, ConsoleCompilerConstants.CompilerOptionsOutputDirectory, workDirectory);
            Send(process, ConsoleCompilerConstants.CompilerOptionsDebug, false);
            Send(process, ConsoleCompilerConstants.CompilerOptionsRebuild, false);
            Send(process, ConsoleCompilerConstants.CompilerOptionsOutputType, 1);
            Send(process, ConsoleCompilerConstants.CompilerOptionsProjectCompiled, false);
            Send(process, ConsoleCompilerConstants.UseDllForSystemUnits, false);
            Send(process, ConsoleCompilerConstants.InternalDebugSavePCU, false);
            Send(process, ConsoleCompilerConstants.CompilerOptionsForDebugging, false);
            Send(process, ConsoleCompilerConstants.CompilerOptionsRunWithEnvironment, false);
            Send(process, ConsoleCompilerConstants.CompilerLocale, "en-US");
            Send(process, ConsoleCompilerConstants.IDELocale,
                Encoding.UTF8.GetString(Encoding.Unicode.GetBytes("en")));
            Send(process, ConsoleCompilerConstants.CompilerOptionsClearStandartModules);
            Send(process, ConsoleCompilerConstants.CompilerOptionsStandartModule,
                "PABCSystem" + ConsoleCompilerConstants.MessageSeparator + "0" +
                ConsoleCompilerConstants.MessageSeparator + "PascalABC.NET");

            async Task Compile(bool expectSuccess = true, string? outputDirectory = null,
                bool rejectInternalError = false)
            {
                outputDirectory ??= workDirectory;
                Directory.CreateDirectory(outputDirectory);
                var diagnostics = new List<string>();
                bool finished = false;
                bool receivedInternalError = false;
                SendText(process, ConsoleCompilerConstants.CompilerOptionsOutputDirectory, outputDirectory);
                Send(process, ConsoleCompilerConstants.CommandCompile);
                while (true)
                {
                    var frame = await ReadAndHandle();
                    if (frame.Command == ConsoleCompilerConstants.Error ||
                        frame.Command == ConsoleCompilerConstants.InternalError)
                        diagnostics.Add(frame.Argument);
                    if (frame.Command == ConsoleCompilerConstants.InternalError)
                        receivedInternalError = true;
                    if (frame.Command == CompilationFinished)
                        finished = true;
                    if (frame.Command == Ready)
                        break;
                }
                if (process.HasExited || process.Id != compilerProcessId)
                    throw new Exception("Compiler did not stay alive between compilations.");
                if (!expectSuccess)
                {
                    if (diagnostics.Count == 0)
                        throw new Exception("The deliberately invalid program compiled successfully.");
                    if (rejectInternalError && receivedInternalError)
                        throw new Exception("Expected a normal compiler diagnostic, got an internal error: " +
                            string.Join(Environment.NewLine, diagnostics));
                    return;
                }
                if (!finished || diagnostics.Count != 0)
                    throw new Exception("Compilation failed: " + string.Join(Environment.NewLine, diagnostics));

                string outputPath = Path.Combine(outputDirectory,
                    Path.GetFileNameWithoutExtension(sourcePath) + ".exe");
                var run = new ProcessStartInfo("dotnet")
                {
                    WorkingDirectory = outputDirectory,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                run.ArgumentList.Add(outputPath);
                using var outputProcess = Process.Start(run)!;
                string output = await outputProcess.StandardOutput.ReadToEndAsync();
                string outputErrors = await outputProcess.StandardError.ReadToEndAsync();
                await outputProcess.WaitForExitAsync().WaitAsync(Timeout);
                string normalizedOutput = output.Replace("\r\n", "\n").Trim();
                if (outputProcess.ExitCode != 0 || normalizedOutput != expectedRuntimeOutput)
                    throw new Exception($"Generated program failed with {outputProcess.ExitCode}. " +
                        $"Expected '{expectedRuntimeOutput}', got '{normalizedOutput}'. {outputErrors}");
            }

            // An explicitly referenced framework assembly outside the implicit root list must
            // be resolved from the installed runtime and must never enter the collectible
            // user-reference context.
            string reloadSourcePath = sourcePath;
            string reloadSourceText = sourceText;
            string reloadExpectedRuntimeOutput = expectedRuntimeOutput;
            sourcePath = Path.Combine(workDirectory, "explicit_framework_reference.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = "{$reference System.Reflection.Metadata.dll}" + Environment.NewLine +
                "uses System.Reflection.Metadata;" + Environment.NewLine +
                "begin var b := new BlobBuilder; Println(b.Count); end.";
            expectedRuntimeOutput = "0";
            await Compile();
            string[] frameworkStatistics = File.ReadAllText(statisticsPath).Split('|');
            if (frameworkStatistics.Length < 3 ||
                !int.TryParse(frameworkStatistics[2], out int frameworkCollectibleLoads) ||
                frameworkCollectibleLoads != 0)
                throw new Exception("A framework assembly was loaded into the collectible " +
                    "user-reference context: " + string.Join("|", frameworkStatistics));

            sourcePath = reloadSourcePath;
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = reloadSourceText;
            expectedRuntimeOutput = reloadExpectedRuntimeOutput;

            await Compile();

            // Repeated compilation of an unchanged reference graph must reuse both cached PE
            // metadata and the current AssemblyLoadContext.
            string unchangedStatistics = File.ReadAllText(statisticsPath);
            for (int iteration = 0; iteration < 10; iteration++)
                await Compile();
            string repeatedStatistics = File.ReadAllText(statisticsPath);
            if (!string.Equals(unchangedStatistics, repeatedStatistics, StringComparison.Ordinal))
                throw new Exception("Unchanged references were reread or reloaded: " +
                    unchangedStatistics + " -> " + repeatedStatistics);

            sourceText = CreateInvalidSource(fixturePath, secondFixturePath);
            await Compile(expectSuccess: false);

            // Change only the bottom assembly in Root -> Common -> Leaf.
            CopyFixture(leafVersionTwoSourcePath, leafPath, DateTime.UtcNow.AddMinutes(-3));
            CopyFixture(leafVersionTwoSourcePath, runtimeLeafPath, DateTime.UtcNow.AddMinutes(-3));
            sourceText = CreateSource(fixturePath, secondFixturePath,
                "VersionOneOnly", 1, "VersionOneOnly", 101,
                "VersionOneOnly", 404, "VersionTwoOnly", 707);
            expectedRuntimeOutput = "1\n101\n303\n404\n404\n707\n707";
            await Compile();

            // Both explicitly referenced roots bind to this one shared dependency.
            CopyFixture(commonVersionTwoSourcePath, commonPath, DateTime.UtcNow.AddMinutes(-3));
            CopyFixture(commonVersionTwoSourcePath, runtimeCommonPath, DateTime.UtcNow.AddMinutes(-3));
            sourceText = CreateSource(fixturePath, secondFixturePath,
                "VersionOneOnly", 1, "VersionOneOnly", 101,
                "VersionTwoOnly", 505, "VersionTwoOnly", 707);
            expectedRuntimeOutput = "1\n101\n303\n505\n505\n707\n707";
            await Compile();

            // A changed transitive dependency must invalidate the generation even
            // when the explicitly referenced root assembly has not changed.
            CopyFixture(dependencyVersionTwoPath, dependencyPath, DateTime.UtcNow.AddMinutes(-2));
            CopyFixture(dependencyVersionTwoPath, runtimeDependencyPath, DateTime.UtcNow.AddMinutes(-2));
            sourceText = CreateSource(fixturePath, secondFixturePath,
                "VersionOneOnly", 1, "VersionTwoOnly", 202,
                "VersionTwoOnly", 505, "VersionTwoOnly", 707);
            expectedRuntimeOutput = "1\n202\n303\n505\n505\n707\n707";
            await Compile();

            // Now change only the root assembly while retaining dependency version 2.
            CopyFixture(versionTwoPath, fixturePath, DateTime.UtcNow);
            sourceText = CreateSource(fixturePath, secondFixturePath,
                "VersionTwoOnly", 2, "VersionTwoOnly", 202,
                "VersionTwoOnly", 505, "VersionTwoOnly", 707);
            expectedRuntimeOutput = "2\n202\n303\n505\n505\n707\n707";
            await Compile();

            // Return both files to version 1 and prove that no stale metadata remains.
            CopyFixture(versionOnePath, fixturePath, DateTime.UtcNow.AddMinutes(2));
            CopyFixture(dependencyVersionOnePath, dependencyPath, DateTime.UtcNow.AddMinutes(2));
            CopyFixture(dependencyVersionOnePath, runtimeDependencyPath, DateTime.UtcNow.AddMinutes(2));
            CopyFixture(commonVersionOneSourcePath, commonPath, DateTime.UtcNow.AddMinutes(2));
            CopyFixture(commonVersionOneSourcePath, runtimeCommonPath, DateTime.UtcNow.AddMinutes(2));
            CopyFixture(leafVersionOneSourcePath, leafPath, DateTime.UtcNow.AddMinutes(2));
            CopyFixture(leafVersionOneSourcePath, runtimeLeafPath, DateTime.UtcNow.AddMinutes(2));
            sourceText = CreateSource(fixturePath, secondFixturePath,
                "VersionOneOnly", 1, "VersionOneOnly", 101,
                "VersionOneOnly", 404, "VersionOneOnly", 606);
            expectedRuntimeOutput = "1\n101\n303\n404\n404\n606\n606";
            await Compile();

            long stressBaseline = 0;
            for (int iteration = 0; iteration < stressIterations; iteration++)
            {
                bool useVersionTwo = (iteration & 1) == 0;
                string leafSource = useVersionTwo ? leafVersionTwoSourcePath : leafVersionOneSourcePath;
                string leafMethod = useVersionTwo ? "VersionTwoOnly" : "VersionOneOnly";
                int leafValue = useVersionTwo ? 707 : 606;
                DateTime writeTime = DateTime.UtcNow.AddMinutes(10 + iteration);
                CopyFixture(leafSource, leafPath, writeTime);
                CopyFixture(leafSource, runtimeLeafPath, writeTime);
                sourceText = CreateSource(fixturePath, secondFixturePath,
                    "VersionOneOnly", 1, "VersionOneOnly", 101,
                    "VersionOneOnly", 404, leafMethod, leafValue);
                expectedRuntimeOutput = $"1\n101\n303\n404\n404\n{leafValue}\n{leafValue}";
                await Compile();

                process.Refresh();
                if (iteration == 9)
                    stressBaseline = process.PrivateMemorySize64;
            }

            if (stressIterations != 0)
            {
                process.Refresh();
                long stressFinal = process.PrivateMemorySize64;
                const long allowedGrowth = 256L * 1024 * 1024;
                if (stressFinal > stressBaseline + allowedGrowth)
                    throw new Exception(
                        $"Compiler private memory grew from {stressBaseline} to {stressFinal} bytes.");
                Console.WriteLine(
                    $"STRESS: {stressIterations} replacements, private memory " +
                    $"{stressBaseline / 1024 / 1024} -> {stressFinal / 1024 / 1024} MB.");
            }

            string alternateReferenceDirectory = Path.Combine(workDirectory, "alternate-reference");
            string firstOutputDirectory = Path.Combine(workDirectory, "output-one");
            string secondOutputDirectory = Path.Combine(workDirectory, "output-two");
            Directory.CreateDirectory(alternateReferenceDirectory);
            string alternateFixturePath = Path.Combine(alternateReferenceDirectory, FixtureAssemblyName);
            DateTime alternateTime = DateTime.UtcNow.AddMinutes(100);
            CopyFixture(versionTwoPath, alternateFixturePath, alternateTime);
            CopyFixture(dependencyVersionTwoPath,
                Path.Combine(alternateReferenceDirectory, DependencyAssemblyName), alternateTime);
            CopyFixture(commonVersionTwoSourcePath,
                Path.Combine(alternateReferenceDirectory, CommonAssemblyName), alternateTime);
            CopyFixture(leafVersionTwoSourcePath,
                Path.Combine(alternateReferenceDirectory, LeafAssemblyName), alternateTime);

            StageRuntimeDependencies(firstOutputDirectory, dependencyVersionTwoPath,
                secondDependencySourcePath, commonVersionTwoSourcePath, leafVersionTwoSourcePath, alternateTime);
            sourceText = CreateSource(alternateFixturePath, secondFixturePath,
                "VersionTwoOnly", 2, "VersionTwoOnly", 202,
                "VersionTwoOnly", 505, "VersionTwoOnly", 707);
            expectedRuntimeOutput = "2\n202\n303\n505\n505\n707\n707";
            await Compile(outputDirectory: firstOutputDirectory);

            StageRuntimeDependencies(secondOutputDirectory, dependencyVersionTwoPath,
                secondDependencySourcePath, commonVersionTwoSourcePath, leafVersionTwoSourcePath,
                alternateTime.AddMinutes(1));
            await Compile(outputDirectory: secondOutputDirectory);

            DateTime returnTime = alternateTime.AddMinutes(2);
            StageRuntimeDependencies(firstOutputDirectory, dependencyVersionOnePath,
                secondDependencySourcePath, commonVersionOneSourcePath, leafVersionOneSourcePath, returnTime);
            sourceText = CreateSource(fixturePath, secondFixturePath,
                "VersionOneOnly", 1, "VersionOneOnly", 101,
                "VersionOneOnly", 404, "VersionOneOnly", 606);
            expectedRuntimeOutput = "1\n101\n303\n404\n404\n606\n606";
            await Compile(outputDirectory: firstOutputDirectory);

            // Resolve a bare reference beside the source file. The compiler process has a
            // different working directory and the generated program goes to another directory.
            string relativeSourceDirectory = Path.Combine(workDirectory, "relative-source");
            string relativeSharedDirectory = Path.Combine(workDirectory, "relative-shared");
            string relativeOutputDirectory = Path.Combine(workDirectory, "relative-output");
            Directory.CreateDirectory(relativeSourceDirectory);
            Directory.CreateDirectory(relativeSharedDirectory);
            DateTime relativeTime = alternateTime.AddMinutes(3);
            CopyFixture(versionTwoPath,
                Path.Combine(relativeSourceDirectory, FixtureAssemblyName), relativeTime);
            CopyFixture(dependencyVersionTwoPath,
                Path.Combine(relativeSourceDirectory, DependencyAssemblyName), relativeTime);
            CopyFixture(commonVersionTwoSourcePath,
                Path.Combine(relativeSourceDirectory, CommonAssemblyName), relativeTime);
            CopyFixture(leafVersionTwoSourcePath,
                Path.Combine(relativeSourceDirectory, LeafAssemblyName), relativeTime);
            CopyFixture(secondFixtureSourcePath,
                Path.Combine(relativeSharedDirectory, SecondFixtureAssemblyName), relativeTime);
            CopyFixture(secondDependencySourcePath,
                Path.Combine(relativeSharedDirectory, SecondDependencyAssemblyName), relativeTime);
            StageRuntimeDependencies(relativeOutputDirectory, dependencyVersionTwoPath,
                secondDependencySourcePath, commonVersionTwoSourcePath, leafVersionTwoSourcePath,
                relativeTime);
            sourcePath = Path.Combine(relativeSourceDirectory, "reload_relative.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = CreateSource(FixtureAssemblyName,
                Path.Combine("..", "relative-shared", SecondFixtureAssemblyName),
                "VersionTwoOnly", 2, "VersionTwoOnly", 202,
                "VersionTwoOnly", 505, "VersionTwoOnly", 707);
            expectedRuntimeOutput = "2\n202\n303\n505\n505\n707\n707";
            await Compile(outputDirectory: relativeOutputDirectory);

            // Resolve both references through parent-relative paths, again independently of
            // the compiler working directory and output directory.
            string relativeLibraryDirectory = Path.Combine(workDirectory, "relative-lib");
            Directory.CreateDirectory(relativeLibraryDirectory);
            relativeTime = relativeTime.AddMinutes(1);
            CopyFixture(versionOnePath,
                Path.Combine(relativeLibraryDirectory, FixtureAssemblyName), relativeTime);
            CopyFixture(dependencyVersionOnePath,
                Path.Combine(relativeLibraryDirectory, DependencyAssemblyName), relativeTime);
            CopyFixture(commonVersionOneSourcePath,
                Path.Combine(relativeLibraryDirectory, CommonAssemblyName), relativeTime);
            CopyFixture(leafVersionOneSourcePath,
                Path.Combine(relativeLibraryDirectory, LeafAssemblyName), relativeTime);
            StageRuntimeDependencies(relativeOutputDirectory, dependencyVersionOnePath,
                secondDependencySourcePath, commonVersionOneSourcePath, leafVersionOneSourcePath,
                relativeTime);
            sourceText = CreateSource(
                Path.Combine("..", "relative-lib", FixtureAssemblyName),
                Path.Combine("..", "relative-shared", SecondFixtureAssemblyName),
                "VersionOneOnly", 1, "VersionOneOnly", 101,
                "VersionOneOnly", 404, "VersionOneOnly", 606);
            expectedRuntimeOutput = "1\n101\n303\n404\n404\n606\n606";
            await Compile(outputDirectory: relativeOutputDirectory);

            // Build tools can replace a DLL while preserving LastWriteTime. An explicit Rebuild
            // must discard the reference context and read the current contents at the same path.
            CopyFixture(versionTwoPath,
                Path.Combine(relativeLibraryDirectory, FixtureAssemblyName), relativeTime);
            sourceText = CreateSource(
                Path.Combine("..", "relative-lib", FixtureAssemblyName),
                Path.Combine("..", "relative-shared", SecondFixtureAssemblyName),
                "VersionTwoOnly", 2, "VersionOneOnly", 101,
                "VersionOneOnly", 404, "VersionOneOnly", 606);
            expectedRuntimeOutput = "2\n101\n303\n404\n404\n606\n606";
            Send(process, ConsoleCompilerConstants.CompilerOptionsRebuild, true);
            await Compile(outputDirectory: relativeOutputDirectory);
            Send(process, ConsoleCompilerConstants.CompilerOptionsRebuild, false);

            // A failed dependency resolution must not poison the long-lived compiler process.
            // First compile without the transitive dependency, then restore it and retry using
            // the same source, reference paths and compiler process.
            string missingDependencyDirectory = Path.Combine(workDirectory, "missing-dependency");
            string recoveryOutputDirectory = Path.Combine(workDirectory, "recovery-output");
            Directory.CreateDirectory(missingDependencyDirectory);
            DateTime recoveryTime = relativeTime.AddMinutes(1);
            CopyFixture(versionOnePath,
                Path.Combine(missingDependencyDirectory, FixtureAssemblyName), recoveryTime);
            CopyFixture(commonVersionOneSourcePath,
                Path.Combine(missingDependencyDirectory, CommonAssemblyName), recoveryTime);
            CopyFixture(leafVersionOneSourcePath,
                Path.Combine(missingDependencyDirectory, LeafAssemblyName), recoveryTime);
            sourceText = CreateSource(
                Path.Combine("..", "missing-dependency", FixtureAssemblyName),
                Path.Combine("..", "relative-shared", SecondFixtureAssemblyName),
                "VersionOneOnly", 1, "VersionOneOnly", 101,
                "VersionOneOnly", 404, "VersionOneOnly", 606);
            await Compile(expectSuccess: false, outputDirectory: recoveryOutputDirectory);

            CopyFixture(dependencyVersionOnePath,
                Path.Combine(missingDependencyDirectory, DependencyAssemblyName), recoveryTime);
            StageRuntimeDependencies(recoveryOutputDirectory, dependencyVersionOnePath,
                secondDependencySourcePath, commonVersionOneSourcePath, leafVersionOneSourcePath,
                recoveryTime);
            expectedRuntimeOutput = "1\n101\n303\n404\n404\n606\n606";
            await Compile(outputDirectory: recoveryOutputDirectory);

            // The reference directive may live in a Pascal unit rather than in the main
            // program. Compile the unit from source once, then remove its source and prove
            // that the next compilation reads the resulting PCU successfully.
            string unitReferenceDirectory = Path.Combine(workDirectory, "unit-reference");
            string unitOutputDirectory = Path.Combine(workDirectory, "unit-output");
            Directory.CreateDirectory(unitReferenceDirectory);
            DateTime unitTime = recoveryTime.AddMinutes(1);
            CopyFixture(versionOnePath,
                Path.Combine(unitReferenceDirectory, FixtureAssemblyName), unitTime);
            CopyFixture(dependencyVersionOnePath,
                Path.Combine(unitReferenceDirectory, DependencyAssemblyName), unitTime);
            CopyFixture(commonVersionOneSourcePath,
                Path.Combine(unitReferenceDirectory, CommonAssemblyName), unitTime);
            CopyFixture(leafVersionOneSourcePath,
                Path.Combine(unitReferenceDirectory, LeafAssemblyName), unitTime);
            StageRuntimeDependencies(unitOutputDirectory, dependencyVersionOnePath,
                secondDependencySourcePath, commonVersionOneSourcePath, leafVersionOneSourcePath,
                unitTime);
            string unitSourcePath = Path.Combine(relativeSourceDirectory,
                "ExternalReferenceUnit.pas");
            string unitPcuPath = Path.ChangeExtension(unitSourcePath, ".pcu");
            File.WriteAllText(unitSourcePath,
                "unit ExternalReferenceUnit;" + Environment.NewLine +
                "{$reference '.." + Path.DirectorySeparatorChar + "unit-reference" +
                Path.DirectorySeparatorChar + FixtureAssemblyName + "'}" + Environment.NewLine +
                "interface" + Environment.NewLine +
                "function ExternalEcho(value: integer): integer;" + Environment.NewLine +
                "implementation" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture;" + Environment.NewLine +
                "function ExternalEcho(value: integer) := (new FixtureType).Echo(value);" +
                Environment.NewLine + "end.");
            sourcePath = Path.Combine(relativeSourceDirectory, "unit_consumer.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            Send(process, ConsoleCompilerConstants.InternalDebugSavePCU, true);
            sourceText = "uses ExternalReferenceUnit;" + Environment.NewLine +
                "begin Println(ExternalEcho(37)); end.";
            expectedRuntimeOutput = "37";
            await Compile(outputDirectory: unitOutputDirectory);
            if (!File.Exists(unitPcuPath))
                throw new Exception("Pascal unit PCU was not created: " + unitPcuPath);

            DateTime unitPcuWriteTime = File.GetLastWriteTimeUtc(unitPcuPath);
            File.Delete(unitSourcePath);
            await Compile(outputDirectory: unitOutputDirectory);

            // Keep the PCU unchanged but replace its external reference. Reading the same PCU
            // must bind it to the new assembly generation and produce the new behavior.
            CopyFixture(versionTwoPath,
                Path.Combine(unitReferenceDirectory, FixtureAssemblyName),
                unitTime.AddMinutes(1));
            expectedRuntimeOutput = "74";
            await Compile(outputDirectory: unitOutputDirectory);
            if (File.GetLastWriteTimeUtc(unitPcuPath) != unitPcuWriteTime)
                throw new Exception("The PCU changed while testing external DLL replacement.");

            // Two independently compiled Pascal units may reference the same external DLL.
            // Reusing both PCUs must load that DLL only once per generation.
            string secondUnitSourcePath = Path.Combine(relativeSourceDirectory,
                "SecondExternalReferenceUnit.pas");
            string secondUnitPcuPath = Path.ChangeExtension(secondUnitSourcePath, ".pcu");
            File.WriteAllText(secondUnitSourcePath,
                "unit SecondExternalReferenceUnit;" + Environment.NewLine +
                "{$reference '.." + Path.DirectorySeparatorChar + "unit-reference" +
                Path.DirectorySeparatorChar + FixtureAssemblyName + "'}" + Environment.NewLine +
                "interface" + Environment.NewLine +
                "function SecondExternalEcho(value: integer): integer;" + Environment.NewLine +
                "implementation" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture;" + Environment.NewLine +
                "function SecondExternalEcho(value: integer) := (new FixtureType).Echo(value);" +
                Environment.NewLine + "end.");
            sourcePath = Path.Combine(relativeSourceDirectory, "two_pcu_consumer.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = "uses ExternalReferenceUnit, SecondExternalReferenceUnit;" +
                Environment.NewLine +
                "begin Println(ExternalEcho(10) + SecondExternalEcho(20)); end.";
            expectedRuntimeOutput = "60";
            await Compile(outputDirectory: unitOutputDirectory);
            if (!File.Exists(secondUnitPcuPath))
                throw new Exception("Second Pascal unit PCU was not created: " + secondUnitPcuPath);

            DateTime secondUnitPcuWriteTime = File.GetLastWriteTimeUtc(secondUnitPcuPath);
            File.Delete(secondUnitSourcePath);
            await Compile(outputDirectory: unitOutputDirectory);

            CopyFixture(versionOnePath,
                Path.Combine(unitReferenceDirectory, FixtureAssemblyName),
                unitTime.AddMinutes(2));
            expectedRuntimeOutput = "30";
            await Compile(outputDirectory: unitOutputDirectory);
            if (File.GetLastWriteTimeUtc(unitPcuPath) != unitPcuWriteTime ||
                File.GetLastWriteTimeUtc(secondUnitPcuPath) != secondUnitPcuWriteTime)
                throw new Exception("A PCU changed while testing their shared external DLL.");

            // Exercise protocol and reference resolution with spaces and non-ASCII characters
            // in the source, PCU, DLL and output directory paths.
            string unicodeSourceDirectory = Path.Combine(workDirectory,
                "исходники с пробелами");
            string unicodeReferenceDirectory = Path.Combine(workDirectory,
                "библиотеки с пробелами");
            string unicodeOutputDirectory = Path.Combine(workDirectory,
                "результат с пробелами");
            Directory.CreateDirectory(unicodeSourceDirectory);
            Directory.CreateDirectory(unicodeReferenceDirectory);
            DateTime unicodeTime = unitTime.AddMinutes(3);
            CopyFixture(versionTwoPath,
                Path.Combine(unicodeReferenceDirectory, FixtureAssemblyName), unicodeTime);
            CopyFixture(dependencyVersionTwoPath,
                Path.Combine(unicodeReferenceDirectory, DependencyAssemblyName), unicodeTime);
            CopyFixture(commonVersionTwoSourcePath,
                Path.Combine(unicodeReferenceDirectory, CommonAssemblyName), unicodeTime);
            CopyFixture(leafVersionTwoSourcePath,
                Path.Combine(unicodeReferenceDirectory, LeafAssemblyName), unicodeTime);
            StageRuntimeDependencies(unicodeOutputDirectory, dependencyVersionTwoPath,
                secondDependencySourcePath, commonVersionTwoSourcePath, leafVersionTwoSourcePath,
                unicodeTime);
            string unicodeUnitSourcePath = Path.Combine(unicodeSourceDirectory,
                "UnicodePathReferenceUnit.pas");
            string unicodeUnitPcuPath = Path.ChangeExtension(unicodeUnitSourcePath, ".pcu");
            File.WriteAllText(unicodeUnitSourcePath,
                "unit UnicodePathReferenceUnit;" + Environment.NewLine +
                "{$reference '.." + Path.DirectorySeparatorChar + "библиотеки с пробелами" +
                Path.DirectorySeparatorChar + FixtureAssemblyName + "'}" + Environment.NewLine +
                "interface" + Environment.NewLine +
                "function UnicodePathEcho(value: integer): integer;" + Environment.NewLine +
                "implementation" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture;" + Environment.NewLine +
                "function UnicodePathEcho(value: integer) := (new FixtureType).Echo(value);" +
                Environment.NewLine + "end.");
            sourcePath = Path.Combine(unicodeSourceDirectory, "главная программа.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = "uses UnicodePathReferenceUnit;" + Environment.NewLine +
                "begin Println(UnicodePathEcho(21)); end.";
            expectedRuntimeOutput = "42";
            await Compile(outputDirectory: unicodeOutputDirectory);
            if (!File.Exists(unicodeUnitPcuPath))
                throw new Exception("Unicode-path PCU was not created: " + unicodeUnitPcuPath);

            File.Delete(unicodeUnitSourcePath);
            await Compile(outputDirectory: unicodeOutputDirectory);

            // Two paths can point to byte-for-byte copies of the same assembly. Treat them as
            // one identity within the generation instead of attempting a duplicate load.
            string identicalOneDirectory = Path.Combine(workDirectory, "identical-one");
            string identicalTwoDirectory = Path.Combine(workDirectory, "identical-two");
            string identicalOutputDirectory = Path.Combine(workDirectory, "identical-output");
            Directory.CreateDirectory(identicalOneDirectory);
            Directory.CreateDirectory(identicalTwoDirectory);
            DateTime identicalTime = unicodeTime.AddMinutes(1);
            foreach (string directory in new[] { identicalOneDirectory, identicalTwoDirectory })
            {
                CopyFixture(versionOnePath,
                    Path.Combine(directory, FixtureAssemblyName), identicalTime);
                CopyFixture(dependencyVersionOnePath,
                    Path.Combine(directory, DependencyAssemblyName), identicalTime);
                CopyFixture(commonVersionOneSourcePath,
                    Path.Combine(directory, CommonAssemblyName), identicalTime);
                CopyFixture(leafVersionOneSourcePath,
                    Path.Combine(directory, LeafAssemblyName), identicalTime);
            }
            StageRuntimeDependencies(identicalOutputDirectory, dependencyVersionOnePath,
                secondDependencySourcePath, commonVersionOneSourcePath, leafVersionOneSourcePath,
                identicalTime);
            sourcePath = Path.Combine(workDirectory, "identical_copies.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = "{$reference '" + Path.Combine(identicalOneDirectory,
                FixtureAssemblyName) + "'}" + Environment.NewLine +
                "{$reference '" + Path.Combine(identicalTwoDirectory,
                FixtureAssemblyName) + "'}" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture;" + Environment.NewLine +
                "begin Println((new FixtureType).Echo(5)); end.";
            expectedRuntimeOutput = "5";
            await Compile(outputDirectory: identicalOutputDirectory);

            // Different assemblies with the same simple name cannot be addressed unambiguously
            // without assembly aliases. Reject the pair normally, then prove that the same
            // compiler process remains usable.
            sourcePath = Path.Combine(workDirectory, "conflicting_identity.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = "{$reference '" + Path.Combine(identicalOneDirectory,
                FixtureAssemblyName) + "'}" + Environment.NewLine +
                "{$reference '" + versionTwoPath + "'}" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture;" + Environment.NewLine +
                "begin Println((new FixtureType).Echo(5)); end.";
            await Compile(expectSuccess: false, outputDirectory: identicalOutputDirectory,
                rejectInternalError: true);

            // A byte-for-byte copy remains the same assembly even if the file itself was
            // renamed. It must be accepted and deduplicated by its internal identity.
            string renamedIdenticalDirectory = Path.Combine(workDirectory, "renamed-identical");
            Directory.CreateDirectory(renamedIdenticalDirectory);
            string renamedIdenticalPath = Path.Combine(renamedIdenticalDirectory,
                "RenamedIdenticalFixture.dll");
            CopyFixture(versionOnePath, renamedIdenticalPath, identicalTime);
            sourcePath = Path.Combine(workDirectory, "renamed_identical_copy.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = "{$reference '" + Path.Combine(identicalOneDirectory,
                FixtureAssemblyName) + "'}" + Environment.NewLine +
                "{$reference '" + renamedIdenticalPath + "'}" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture;" + Environment.NewLine +
                "begin Println((new FixtureType).Echo(5)); end.";
            expectedRuntimeOutput = "5";
            await Compile(outputDirectory: identicalOutputDirectory);

            // If one formerly identical copy is replaced, the next compilation must detect
            // the divergence. Restoring the original bytes must make the pair valid again.
            CopyFixture(versionTwoPath, renamedIdenticalPath, identicalTime);
            await Compile(expectSuccess: false, outputDirectory: identicalOutputDirectory,
                rejectInternalError: true);
            CopyFixture(versionOnePath, renamedIdenticalPath, identicalTime);
            expectedRuntimeOutput = "5";
            await Compile(outputDirectory: identicalOutputDirectory);

            // Renaming a file does not change its internal assembly identity. A differently
            // named file containing another generation of the same assembly must therefore
            // be rejected as well.
            string renamedConflictDirectory = Path.Combine(workDirectory, "renamed-conflict");
            Directory.CreateDirectory(renamedConflictDirectory);
            string renamedConflictPath = Path.Combine(renamedConflictDirectory,
                "RenamedFixture.dll");
            CopyFixture(versionTwoPath, renamedConflictPath, identicalTime);
            sourcePath = Path.Combine(workDirectory, "renamed_conflicting_identity.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = "{$reference '" + Path.Combine(identicalOneDirectory,
                FixtureAssemblyName) + "'}" + Environment.NewLine +
                "{$reference '" + renamedConflictPath + "'}" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture;" + Environment.NewLine +
                "begin Println((new FixtureType).Echo(5)); end.";
            await Compile(expectSuccess: false, outputDirectory: identicalOutputDirectory,
                rejectInternalError: true);

            sourceText = "{$reference '" + Path.Combine(identicalOneDirectory,
                FixtureAssemblyName) + "'}" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture;" + Environment.NewLine +
                "begin Println((new FixtureType).Echo(5)); end.";
            expectedRuntimeOutput = "5";
            await Compile(outputDirectory: identicalOutputDirectory);

            // A damaged root reference must produce a normal compiler diagnostic and must not
            // poison the long-lived process. Restoring the assembly must make it usable again.
            string damagedDirectory = Path.Combine(workDirectory, "damaged-reference");
            string damagedOutputDirectory = Path.Combine(workDirectory, "damaged-output");
            Directory.CreateDirectory(damagedDirectory);
            DateTime damagedTime = identicalTime.AddMinutes(1);
            string damagedFixturePath = Path.Combine(damagedDirectory, FixtureAssemblyName);
            CopyFixture(versionOnePath, damagedFixturePath, damagedTime);
            CopyFixture(dependencyVersionOnePath,
                Path.Combine(damagedDirectory, DependencyAssemblyName), damagedTime);
            CopyFixture(commonVersionOneSourcePath,
                Path.Combine(damagedDirectory, CommonAssemblyName), damagedTime);
            CopyFixture(leafVersionOneSourcePath,
                Path.Combine(damagedDirectory, LeafAssemblyName), damagedTime);
            StageRuntimeDependencies(damagedOutputDirectory, dependencyVersionOnePath,
                secondDependencySourcePath, commonVersionOneSourcePath, leafVersionOneSourcePath,
                damagedTime);
            sourcePath = Path.Combine(workDirectory, "damaged_reference_recovery.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = "{$reference '" + damagedFixturePath + "'}" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture;" + Environment.NewLine +
                "begin Println((new FixtureType).Echo(5)); end.";
            expectedRuntimeOutput = "5";
            await Compile(outputDirectory: damagedOutputDirectory);

            File.WriteAllBytes(damagedFixturePath, Encoding.UTF8.GetBytes("not a managed assembly"));
            File.SetLastWriteTimeUtc(damagedFixturePath, damagedTime.AddMinutes(1));
            await Compile(expectSuccess: false, outputDirectory: damagedOutputDirectory,
                rejectInternalError: true);

            CopyFixture(versionOnePath, damagedFixturePath, damagedTime.AddMinutes(2));
            expectedRuntimeOutput = "5";
            await Compile(outputDirectory: damagedOutputDirectory);

            // Removing an explicitly referenced root assembly must also be recoverable without
            // restarting the compiler process.
            File.Delete(damagedFixturePath);
            await Compile(expectSuccess: false, outputDirectory: damagedOutputDirectory,
                rejectInternalError: true);
            CopyFixture(versionOnePath, damagedFixturePath, damagedTime.AddMinutes(3));
            expectedRuntimeOutput = "5";
            await Compile(outputDirectory: damagedOutputDirectory);

            // Different AssemblyVersion values do not make two assemblies with the same simple
            // name simultaneously addressable without aliases. Reject the pair, but allow the
            // version-2 assembly when it is the only selected generation.
            string versionedDirectory = Path.Combine(workDirectory, "versioned-identity");
            string versionedOutputDirectory = Path.Combine(workDirectory, "versioned-output");
            Directory.CreateDirectory(versionedDirectory);
            string versionedAssemblyPath = Path.Combine(versionedDirectory,
                FixtureAssemblyName);
            CopyFixture(assemblyVersionTwoPath, versionedAssemblyPath,
                damagedTime.AddMinutes(4));
            sourcePath = Path.Combine(workDirectory, "different_assembly_versions.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = "{$reference '" + damagedFixturePath + "'}" + Environment.NewLine +
                "{$reference '" + versionedAssemblyPath + "'}" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture;" + Environment.NewLine +
                "begin Println((new FixtureType).Echo(5)); end.";
            await Compile(expectSuccess: false, outputDirectory: versionedOutputDirectory,
                rejectInternalError: true);

            sourceText = "{$reference '" + versionedAssemblyPath + "'}" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture;" + Environment.NewLine +
                "begin Println((new FixtureType).Echo(5)); end.";
            expectedRuntimeOutput = "2005";
            await Compile(outputDirectory: versionedOutputDirectory);

            // Two different roots may require incompatible generations of the same transitive
            // dependency. The global load order must not silently select either Common.dll.
            string diamondOneDirectory = Path.Combine(workDirectory, "diamond-one");
            string diamondTwoDirectory = Path.Combine(workDirectory, "diamond-two");
            string diamondOutputDirectory = Path.Combine(workDirectory, "diamond-output");
            Directory.CreateDirectory(diamondOneDirectory);
            Directory.CreateDirectory(diamondTwoDirectory);
            CopyFixture(versionOnePath,
                Path.Combine(diamondOneDirectory, FixtureAssemblyName), identicalTime);
            CopyFixture(dependencyVersionOnePath,
                Path.Combine(diamondOneDirectory, DependencyAssemblyName), identicalTime);
            CopyFixture(commonVersionOneSourcePath,
                Path.Combine(diamondOneDirectory, CommonAssemblyName), identicalTime);
            CopyFixture(leafVersionOneSourcePath,
                Path.Combine(diamondOneDirectory, LeafAssemblyName), identicalTime);
            CopyFixture(secondFixtureVersionTwoSourcePath,
                Path.Combine(diamondTwoDirectory, SecondFixtureAssemblyName), identicalTime);
            CopyFixture(secondDependencySourcePath,
                Path.Combine(diamondTwoDirectory, SecondDependencyAssemblyName), identicalTime);
            CopyFixture(commonVersionTwoSourcePath,
                Path.Combine(diamondTwoDirectory, CommonAssemblyName), identicalTime);
            CopyFixture(leafVersionOneSourcePath,
                Path.Combine(diamondTwoDirectory, LeafAssemblyName), identicalTime);
            sourcePath = Path.Combine(workDirectory, "diamond_dependency_conflict.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = "{$reference '" + Path.Combine(diamondOneDirectory,
                FixtureAssemblyName) + "'}" + Environment.NewLine +
                "{$reference '" + Path.Combine(diamondTwoDirectory,
                SecondFixtureAssemblyName) + "'}" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture, ReflectionCacheUnloadSecondFixture;" +
                Environment.NewLine +
                "begin var a := new FixtureType; var b := new SecondFixtureType; Println(1); end.";
            await Compile(expectSuccess: false, outputDirectory: diamondOutputDirectory,
                rejectInternalError: true);

            sourcePath = Path.Combine(workDirectory, "after_diamond_conflict.pas");
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            sourceText = "{$reference '" + Path.Combine(identicalOneDirectory,
                FixtureAssemblyName) + "'}" + Environment.NewLine +
                "uses ReflectionCacheUnloadFixture;" + Environment.NewLine +
                "begin Println((new FixtureType).Echo(5)); end.";
            expectedRuntimeOutput = "5";
            await Compile(outputDirectory: identicalOutputDirectory);

            Send(process, ConsoleCompilerConstants.CommandExit);
            if ((await ReadAndHandle()).Command != ConsoleCompilerConstants.CommandExit)
                throw new Exception("Missing shutdown acknowledgement.");
            await process.WaitForExitAsync().WaitAsync(Timeout);
            string stderr = await errors;
            if (process.ExitCode != 0 || stderr.Length != 0)
                throw new Exception($"Compiler exited with {process.ExitCode}: {stderr}");

            Console.WriteLine(
                "PASS: compiled programs ran with every selected DLL generation; old contexts were unloaded.");
        }
        finally
        {
            if (errors != null && !process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                await process.WaitForExitAsync();
            }
            try
            {
                Directory.Delete(workDirectory, recursive: true);
            }
            catch (IOException)
            {
            }
        }
    }

    private static string CreateSource(string fixtureReference, string secondFixtureReference,
        string methodName, int expectedValue,
        string dependencyMethodName, int dependencyExpectedValue,
        string commonMethodName, int commonExpectedValue,
        string leafMethodName, int leafExpectedValue) =>
        "{$reference '" + fixtureReference + "'}" + Environment.NewLine +
        "{$reference '" + secondFixtureReference + "'}" + Environment.NewLine +
        "uses ReflectionCacheUnloadFixture, ReflectionCacheUnloadSecondFixture;" + Environment.NewLine +
        "begin" + Environment.NewLine +
        "  var x := new FixtureType;" + Environment.NewLine +
        "  assert(x." + methodName + " = " + expectedValue + ");" + Environment.NewLine +
        "  assert(x.Dependency." + dependencyMethodName + " = " + dependencyExpectedValue + ");" + Environment.NewLine +
        "  assert((new SecondFixtureType).Dependency.Value = 303);" + Environment.NewLine +
        "  assert(x.Common." + commonMethodName + " = " + commonExpectedValue + ");" + Environment.NewLine +
        "  assert((new SecondFixtureType).Common." + commonMethodName + " = " + commonExpectedValue + ");" + Environment.NewLine +
        "  assert(x.Common.Leaf." + leafMethodName + " = " + leafExpectedValue + ");" + Environment.NewLine +
        "  assert((new SecondFixtureType).Common.Leaf." + leafMethodName + " = " + leafExpectedValue + ");" + Environment.NewLine +
        "  Println(x." + methodName + ");" + Environment.NewLine +
        "  Println(x.Dependency." + dependencyMethodName + ");" + Environment.NewLine +
        "  Println((new SecondFixtureType).Dependency.Value);" + Environment.NewLine +
        "  Println(x.Common." + commonMethodName + ");" + Environment.NewLine +
        "  Println((new SecondFixtureType).Common." + commonMethodName + ");" + Environment.NewLine +
        "  Println(x.Common.Leaf." + leafMethodName + ");" + Environment.NewLine +
        "  Println((new SecondFixtureType).Common.Leaf." + leafMethodName + ");" + Environment.NewLine +
        "end.";

    private static string CreateInvalidSource(string fixtureReference, string secondFixtureReference) =>
        "{$reference '" + fixtureReference + "'}" + Environment.NewLine +
        "{$reference '" + secondFixtureReference + "'}" + Environment.NewLine +
        "uses ReflectionCacheUnloadFixture, ReflectionCacheUnloadSecondFixture;" + Environment.NewLine +
        "begin" + Environment.NewLine +
        "  var x := new FixtureType;" + Environment.NewLine +
        "  x.MethodThatDoesNotExist;" + Environment.NewLine +
        "end.";

    private static void CopyFixture(string source, string destination, DateTime writeTimeUtc)
    {
        File.Copy(source, destination, overwrite: true);
        File.SetLastWriteTimeUtc(destination, writeTimeUtc);
    }

    private static void StageRuntimeDependencies(string outputDirectory, string dependency,
        string secondDependency, string common, string leaf, DateTime writeTimeUtc)
    {
        Directory.CreateDirectory(outputDirectory);
        CopyFixture(dependency, Path.Combine(outputDirectory, DependencyAssemblyName), writeTimeUtc);
        CopyFixture(secondDependency, Path.Combine(outputDirectory, SecondDependencyAssemblyName), writeTimeUtc);
        CopyFixture(common, Path.Combine(outputDirectory, CommonAssemblyName), writeTimeUtc);
        CopyFixture(leaf, Path.Combine(outputDirectory, LeafAssemblyName), writeTimeUtc);
    }

    private static void Send(Process process, int command, object? argument = null)
    {
        process.StandardInput.WriteLine(argument == null ? command.ToString() : $"{command} {argument}");
        process.StandardInput.Flush();
    }

    private static void SendText(Process process, int command, string text)
    {
        Send(process, command, text.Length);
        process.StandardInput.Write(text);
        process.StandardInput.Flush();
    }

    private static async Task<string> ReadFrame(StreamReader reader)
    {
        var text = new StringBuilder();
        char[] character = new char[1];
        using var cancellation = new CancellationTokenSource(Timeout);
        while (true)
        {
            if (await reader.ReadAsync(character.AsMemory(), cancellation.Token) == 0)
                throw new EndOfStreamException("Compiler closed the protocol stream: " + text);
            text.Append(character[0]);
            if (text.Length >= ConsoleCompilerConstants.DataSeparator.Length &&
                text.ToString(text.Length - ConsoleCompilerConstants.DataSeparator.Length,
                    ConsoleCompilerConstants.DataSeparator.Length) == ConsoleCompilerConstants.DataSeparator)
                return text.ToString(0, text.Length - ConsoleCompilerConstants.DataSeparator.Length);
        }
    }
}
