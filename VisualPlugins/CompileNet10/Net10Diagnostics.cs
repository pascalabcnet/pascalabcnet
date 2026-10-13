using System;
using System.Collections.Generic;
using System.IO;
using PascalABCCompiler.Errors;

namespace VisualPascalABCPlugins
{
    internal static class Net10Diagnostics
    {
        public static List<Error> Create(Net10CompileResponse response, string sourceFileName)
        {
            var errors = new List<Error>();
            var warnings = new List<Error>();
            if (response.diagnostics != null)
                foreach (var diagnostic in response.diagnostics)
                {
                    string fileName = string.IsNullOrWhiteSpace(diagnostic.fileName)
                        ? sourceFileName : diagnostic.fileName;
                    if (!Path.IsPathRooted(fileName))
                        fileName = Path.Combine(Path.GetDirectoryName(sourceFileName), fileName);
                    fileName = Path.GetFullPath(fileName);
                    int line = Math.Max(1, diagnostic.line), column = Math.Max(1, diagnostic.column);
                    if (string.Equals(diagnostic.severity, "warning", StringComparison.OrdinalIgnoreCase))
                        warnings.Add(new CommonWarning(diagnostic.message ?? "Предупреждение .NET 10", fileName, line, column));
                    else
                        errors.Add(new CommonCompilerError(diagnostic.message ?? "Ошибка компиляции .NET 10", fileName, line, column));
                }
            if (!response.success && errors.Count == 0)
                errors.Add(new Error(string.IsNullOrWhiteSpace(response.message)
                    ? "Компилятор .NET 10 не создал выходной файл." : response.message));
            // Like the legacy IDE: errors first, so warnings never hide error navigation.
            errors.AddRange(warnings);
            return errors;
        }

        public static bool ChangeViewTab(Net10CompileResponse response, bool run) => !response.success || !run;
    }
}
