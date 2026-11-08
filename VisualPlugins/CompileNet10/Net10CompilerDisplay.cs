using System;
using System.IO;

namespace VisualPascalABCPlugins
{
    // Uses the legacy IDE's resource keys, but never its compiler instance/state.
    internal sealed class Net10CompilerDisplay
    {
        private readonly Func<string, string> resource;
        private bool compiling;
        public bool HasResult { get; private set; }
        public Net10CompilerDisplay(Func<string, string> resource) { this.resource = resource; }

        public void Handle(Net10CompilerEvent value, Action<string> message, Action<string> status)
        {
            if (value.@event == "compilerMessage")
            {
                message("[.NET 10] " + value.message);
                return;
            }
            if (value.state == "NuGetRestoreProgress" || value.state == "NuGetRestoreStarting")
            {
                // The status strip owns delayed progress and cancellation. Starting
                // is bookkeeping, not proof of a download: cached checks stay silent.
                return;
            }
            if (value.state == "NuGetPackagesInstalled")
            {
                message("[.NET 10] " + string.Format(resource("STATE_NUGETPACKAGESINSTALLED{0}"), value.fileName));
                return;
            }
            if (value.state == "NuGetPackageInstalled")
            {
                message("[.NET 10] " + string.Format(resource("STATE_NUGETPACKAGEINSTALLED{0}"), value.fileName));
                return;
            }
            string key = null;
            switch (value.state)
            {
                case "CompilationStarting": compiling = true; HasResult = false; key = "STATE_START_COMPILING_ASSEMBLY{0}"; break;
                case "BeginCompileFile": key = "STATE_BEGINCOMPILEFILE{0}"; break;
                case "CompileInterface": key = "STATE_COMPILEINTERFACE{0}"; break;
                case "CompileImplementation": key = "STATE_COMPILEIMPLEMENTATION{0}"; break;
                case "EndCompileFile": key = "STATE_ENDCOMPILEFILE{0}"; break;
                case "CodeGeneration": key = "STATE_CODEGENERATION{0}"; break;
                case "ReadDLL": key = "STATE_READDLL{0}"; break;
                case "SavePCUFile": key = "STATE_SAVEPCUFILE{0}"; break;
                case "ReadPCUFile": key = "STATE_READPCUFILE{0}"; break;
                case "PCUReadingError": key = "STATE_PCUREADINGERROR{0}"; break;
                case "PCUWritingError": key = "STATE_PCUWRITINGERROR{0}"; break;
                case "SemanticTreeConversion": key = "STATE_SEMANTICTREECONVERSION{0}"; break;
                case "SemanticTreeConverterConnected": key = "STATE_SEMANTICTREECONVERTERCONNECTED{0}"; break;
                case "SyntaxTreeConversion": key = "STATE_SYNTAXTREECONVERSION{0}"; break;
                case "SyntaxTreeConverterConnected": key = "STATE_SYNTAXTREECONVERTERCONNECTED{0}"; break;
                case "Reloading": compiling = false; HasResult = false; key = "STATE_RELOADING"; break;
                case "Ready": key = "STATE_READY"; break;
                case "CompilationFinished":
                    message(value.errorCount == 0
                        ? string.Format(resource("CM_OK_{0}MS"), value.elapsedMilliseconds)
                        : string.Format(resource("CM_{0}_ERROS"), value.errorCount));
                    key = "STATE_COMPILATIONFINISHED{0}";
                    break;
            }
            string file = string.IsNullOrEmpty(value.fileName) ? "" : Path.GetFileName(value.fileName);
            if (key != null) message("[.NET 10]" + string.Format(resource(key), file));
            switch (value.state)
            {
                case "BeginCompileFile": status(".NET 10: " + string.Format(resource("STATETEXT_BEGINCOMPILEFILE{0}"), file)); break;
                case "CodeGeneration": status(".NET 10: " + string.Format(resource("STATETEXT_CODEGENERATION{0}"), file)); break;
                case "Reloading": status(".NET 10: " + resource("STATETEXT_RELOADING")); break;
                case "Ready":
                    // Initial/reload Ready is not a compilation result. Keep the
                    // platform or previous result until a real phase arrives.
                    if (!compiling) break;
                    string text = value.errorCount > 0
                        ? string.Format(resource("STATETEXT_{0}_ERROS"), value.errorCount)
                        : value.warningCount > 0
                            ? string.Format(resource("STATETEXT_COMPILATION_SUCCESS_{0}LINES_{1}WARNINGS"), value.linesCompiled, value.warningCount)
                            : string.Format(resource("STATETEXT_COMPILATION_SUCCESS_{0}LINES"), value.linesCompiled);
                    status(".NET 10: " + text);
                    HasResult = compiling;
                    compiling = false;
                    break;
            }
        }
    }
}
