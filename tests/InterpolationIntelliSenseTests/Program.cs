using System;
using System.Collections.Generic;
using System.Linq;
using CodeCompletion;
using Languages.Facade;
using Languages.Pascal;
using PascalABCCompiler.Contracts.Parsers;
using PascalABCCompiler.Errors;

internal static class Program
{
    private static void Main()
    {
        var language = PascalLanguageRegistration.RegisterPascalLanguage();
        CodeCompletionBootstrap.Initialize("en");
        CheckInterpolation(language, "\n");
        CheckInterpolation(language, "\r\n");
        Console.WriteLine("PASS interpolation IntelliSense (LF and CRLF)");
    }

    private static void CheckInterpolation(ILanguage language, string newline)
    {
        const string fileName = "InterpolationIntelliSense.pas";
        string source = string.Join(newline, new[]
        {
            "begin",
            "  var name := 'Ivan';",
            "  var age := 18;",
            "  var ordinary := $'Hello {name}';",
            "  // $''' is not a literal here",
            "  (* $'''", "     still a comment *)",
            "  var text := $'''",
            "    It's {name}.",
            "    Age: {age + 1}.",
            "    Escaped: {{name}}. Next: {name.Substring(1, 2)}.",
            "    ''';",
            "  var plain := '''",
            "    Plain: {name}.",
            "    ''';",
            "  Println(name);",
            "end."
        });

        var converter = new CodeCompletionController().Compile(fileName, source);
        Check(converter.is_compiled && converter.LastConversionError == null, "fixture builds");

        foreach (var item in new[]
        {
            (marker: "Hello {name", symbol: "name"),
            (marker: "It's {name", symbol: "name"),
            (marker: "Age: {age", symbol: "age"),
            (marker: "Println(name", symbol: "name")
        })
        {
            int offset = After(source, item.marker) - item.symbol.Length;
            var (line, column) = Position(source, offset);
            var hover = HoverService.GetDescription(language, converter, fileName, source, offset, line, column);
            Check(hover != null && hover.IndexOf("var " + item.symbol + ":", StringComparison.OrdinalIgnoreCase) >= 0,
                "hover resolves " + item.marker + ": " + hover);
        }

        foreach (string marker in new[] { "Escaped: {{name", "Plain: {name" })
        {
            int offset = After(source, marker) - 1;
            var (line, column) = Position(source, offset);
            Check(HoverService.GetDescription(language, converter, fileName, source, offset, line, column) == null,
                "no symbol hover in literal text: " + marker);
        }

        int memberOffset = After(source, "Next: {name.");
        var (memberLine, memberColumn) = Position(source, memberOffset);
        var context = new CompletionTriggerContext { DotPressed = true };
        var expression = CompletionExpressionService.AnalyzeAtCaret(memberOffset, source.Substring(0, memberOffset),
            memberLine, memberColumn, language.LanguageIntellisenseSupport, context, KeywordKind.None);
        var parsed = CompletionExpressionService.Parse(language.Parser, fileName, expression.ExpressionText, context);
        Check(!parsed.ShouldAbortCompletion && parsed.Expression != null,
            "member expression inside unfinished multiline string: " + expression.ExpressionText);
        var symbols = CompletionSymbolService.GetSymbols(converter, memberLine, memberColumn, context,
            language.CaseSensitive, expression.ExpressionText, expression.CtrlOrShiftSpaceAfterDot,
            expression.InsidePatternWithDots, expression.Pattern, parsed.Expression, expression.Keyword, true, 0);
        Check(symbols.Symbols?.Any(symbol => symbol != null && symbol.name == "Substring") == true,
            "member completion inside multiline interpolation");

        foreach (var trigger in new[] { (marker: "Substring(", key: '('), (marker: "Substring(1,", key: ',') })
        {
            int offset = After(source, trigger.marker);
            var (line, column) = Position(source, offset);
            var help = SignatureHelpService.GetSignatureHelpAtCaret(language, converter, fileName,
                source.Substring(0, offset), offset, line, column, trigger.key, 1, 1);
            Check(help?.Signatures?.Any(signature => signature.Contains("Substring")) == true,
                "signature help inside multiline interpolation after " + trigger.key);
        }

        int navigationOffset = After(source, "It's {name") - 1;
        var (navigationLine, navigationColumn) = Position(source, navigationOffset);
        string candidate = language.LanguageIntellisenseSupport.FindExpressionFromAnyPosition(navigationOffset,
            source, navigationLine, navigationColumn, out var keyword, out _);
        var identifier = language.Parser.GetExpression(fileName, candidate, new List<Error>(), new List<CompilerWarning>());
        Check(identifier != null && converter.GetDefinition(identifier, navigationLine, navigationColumn, keyword, false)
            .Any(definition => definition.line == 2), "navigation goes to the declaration of name");
    }

    private static int After(string source, string marker)
    {
        int offset = source.IndexOf(marker, StringComparison.Ordinal);
        Check(offset >= 0, "missing marker " + marker);
        return offset + marker.Length;
    }

    private static (int line, int column) Position(string source, int offset) =>
        (source.Take(offset).Count(ch => ch == '\n'), offset - source.LastIndexOf('\n', offset - 1) - 1);

    private static void Check(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }
}
