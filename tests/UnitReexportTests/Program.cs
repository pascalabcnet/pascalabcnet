using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeCompletion;
using Languages.Facade;
using Languages.Pascal;
using PascalABCCompiler.Contracts.Parsers;
using PascalABCCompiler.Errors;

internal static class Program
{
    private static void Main(string[] args)
    {
        var language = PascalLanguageRegistration.RegisterPascalLanguage();
        CodeCompletionBootstrap.Initialize("en");
        Check(args.Length == 2, "pass directories with the source and PCU-only fixtures");
        CheckClient(language, Path.GetFullPath(args[0]), false);
        CheckClient(language, Path.GetFullPath(args[1]), true);
    }

    private static void CheckClient(ILanguage language, string directory, bool pcuOnly)
    {
        var fileName = Path.Combine(directory, "ReexportClient.pas");
        string source = File.ReadAllText(fileName);
        Check(File.Exists(Path.Combine(directory, "ReexportFacade.pas")) != pcuOnly,
            "fixture uses the intended module format");
        var converter = new CodeCompletionController().Compile(fileName, source);
        Check(converter.is_compiled && converter.LastConversionError == null,
            "client builds its IntelliSense model: " + converter.LastConversionError);

        int qualified = After(source, "  ReexportFacade.");
        var (qualifiedLine, qualifiedColumn) = Position(source, qualified);
        var context = new CompletionTriggerContext { DotPressed = true };
        var expression = CompletionExpressionService.AnalyzeAtCaret(qualified, source.Substring(0, qualified),
            qualifiedLine, qualifiedColumn, language.LanguageIntellisenseSupport, context, KeywordKind.None);
        var parsed = CompletionExpressionService.Parse(language.Parser, fileName, expression.ExpressionText, context);
        Check(!parsed.ShouldAbortCompletion && parsed.Expression != null, "parse completion expression");
        var completion = CompletionSymbolService.GetSymbols(converter, qualifiedLine, qualifiedColumn, context,
            language.CaseSensitive, expression.ExpressionText, expression.CtrlOrShiftSpaceAfterDot,
            expression.InsidePatternWithDots, expression.Pattern, parsed.Expression, expression.Keyword, true, 0);
        Check(completion.Symbols != null, "qualified completion has members");
        var names = completion.Symbols.Select(s => s.name).ToArray();
        Check(names.Contains("FFF2") && names.Contains("F1") && !names.Contains("F2") && !names.Contains("Hidden"),
            "qualified completion shows only the exported routines: " + string.Join(", ", names));

        int call = After(source, "  FFF2(");
        var (line, column) = Position(source, call);
        Check(converter.GetNameByPattern("FFF", line, column, true, 0)?.Any(s => s.name == "FFF2") == true,
            "unqualified completion offers the alias");

        foreach (string marker in new[] { "  FFF2", "  ReexportFacade.FFF2" })
        {
            int offset = After(source, marker) - 2;
            var (hoverLine, hoverColumn) = Position(source, offset);
            string hover = HoverService.GetDescription(language, converter, fileName, source, offset, hoverLine, hoverColumn);
            Check(hover != null && hover.Contains("FFF2"), "hover uses the alias: " + hover);
            if (!pcuOnly)
            {
                string candidate = language.LanguageIntellisenseSupport.FindExpressionFromAnyPosition(offset,
                    source, hoverLine, hoverColumn, out var keyword, out _);
                var identifier = language.Parser.GetExpression(fileName, candidate, new List<Error>(), new List<CompilerWarning>());
                Check(identifier != null && converter.GetDefinition(identifier, hoverLine, hoverColumn, keyword, false)
                    .Any(d => Path.GetFileName(d.file_name) == "ReexportOrigin.pas" && d.line == 7),
                    "navigation reaches the original procedure declaration");
            }
        }

        foreach (string marker in new[] { "if (F1(", "(ReexportFacade.F1(" })
        {
            int offset = After(source, marker);
            var (signatureLine, signatureColumn) = Position(source, offset);
            var help = SignatureHelpService.GetSignatureHelpAtCaret(language, converter, fileName,
                source.Substring(0, offset), offset, signatureLine, signatureColumn, '(', 1, 1);
            Check(help?.Signatures?.Length == 2 && help.Signatures.Any(s => s.Contains("integer")) &&
                help.Signatures.Any(s => s.Contains("string")), "signature help keeps both overloads: " + marker);
        }
        Console.WriteLine("PASS re-export IntelliSense: " + (pcuOnly ? "PCU" : "source"));
    }

    private static int After(string source, string marker)
    {
        int offset = source.IndexOf(marker, StringComparison.Ordinal);
        Check(offset >= 0, "missing marker " + marker);
        return offset + marker.Length;
    }

    private static (int line, int column) Position(string source, int offset) =>
        (source.Take(offset).Count(c => c == '\n'), offset - source.LastIndexOf('\n', offset - 1) - 1);

    private static void Check(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }
}
