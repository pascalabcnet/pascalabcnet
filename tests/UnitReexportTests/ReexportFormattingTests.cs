using System;
using System.Collections.Generic;
using System.Linq;
using CodeFormatters;
using Languages.Facade;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTree;

internal static class ReexportFormattingTests
{
    public static void Run(ILanguage language)
    {
        foreach (string newline in new[] { "\n", "\r\n" })
        {
            string source = string.Join(newline, new[]
            {
                "unit FormatReexport;",
                "interface",
                "uses ReexportOrigin;",
                "EXPORT    ReexportOrigin . F1   ,   ReexportOrigin . F2   AS   FFF2  ;",
                "export ReexportOrigin(*keep dot comment*).Hidden {keep alias comment} as Kept, // keep list comment",
                "    ReexportOrigin . F1 as Again ;",
                "implementation",
                "end."
            });
            var original = Parse(language, source);
            string formatted = new CodeFormatter(2).FormatTree(source, original, 1, 1);
            Check(formatted.Contains("export ReexportOrigin.F1, ReexportOrigin.F2 as FFF2;"),
                "normalize keyword, dot, comma, alias and semicolon: " + formatted);
            Check(formatted.Contains("ReexportOrigin(*keep dot comment*).Hidden {keep alias comment} as Kept, // keep list comment"),
                "preserve comments in the qualified name, alias and list");
            Check(formatted.Contains("\n  ReexportOrigin.F1 as Again;"), "indent the continuation of the list");

            var formattedTree = Parse(language, formatted);
            Check(Exports(original).SequenceEqual(Exports(formattedTree)), "preserve re-export declarations");

            // The existing formatter may mix LF and CRLF in the gap after the
            // unit header. Normalize line endings before checking idempotence.
            string canonical = formatted.Replace("\r\n", "\n").Replace("\n", "\r\n");
            string secondPass = new CodeFormatter(2).FormatTree(canonical, Parse(language, canonical), 1, 1);
            Check(secondPass == canonical, "formatting is stable on the second pass. First:\n" +
                canonical + "\nSecond:\n" + secondPass);
        }
        Console.WriteLine("PASS re-export formatting (LF and CRLF)");
    }

    private static unit_module Parse(ILanguage language, string source)
    {
        var errors = new List<Error>();
        var result = language.Parser.GetCompilationUnitForFormatter("FormatReexport.pas", source,
            errors, new List<CompilerWarning>()) as unit_module;
        Check(result != null && errors.Count == 0, "parse unit for formatting: " + string.Join("; ", errors));
        return result;
    }

    private static IEnumerable<(string Unit, string Original, string Alias)> Exports(unit_module module) =>
        module.interface_part.interface_definitions.defs.OfType<unit_reexport>()
            .Select(entry => (entry.source_unit.name, entry.source_name.name, entry.export_name?.name));

    private static void Check(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }
}
