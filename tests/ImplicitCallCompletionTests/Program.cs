using System;
using System.IO;
using System.Linq;
using CodeCompletion;
using Languages.Pascal;
using PascalABCCompiler.Contracts.Parsers;

internal static class Program
{
    private static void Main()
    {
        var language = PascalLanguageRegistration.RegisterPascalLanguage();
        CodeCompletionBootstrap.Initialize("en");
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string library = Path.Combine(repository, "bin", "Lib");
        Check(File.Exists(Path.Combine(library, "PABCSystem.pas")), "standard library sources exist");
        CodeCompletionController.comp.CompilerOptions.SystemDirectory = Path.Combine(repository, "bin");
        CodeCompletionController.comp.CompilerOptions.StandardDirectories["%PABCSYSTEM%"] = Path.Combine(repository, "bin");
        const string fileName = "ImplicitCallCompletion.pas";
        string declarations = string.Join("\n", new[] {
            "function Required(x: integer): string := x.ToString;",
            "function NoArgs: string := 'abc';",
            "function Optional(x: integer := 1): string := x.ToString;",
            "function Variadic(params xs: array of integer): string := 'abc';",
            "function Choice(x: integer): integer; overload; begin Result := x; end;",
            "function Choice: string; overload; begin Result := 'abc'; end;"
        });
        foreach (var item in new[] {
            (Expression: "Int", Member: (string)null),
            (Expression: "Int(2.5)", Member: "CompareTo"),
            (Expression: "Int.CompareTo", Member: (string)null),
            (Expression: "Int(2.5).ToString", Member: "Substring"),
            (Expression: "(Int)", Member: (string)null),
            (Expression: "PABCSystem.Int", Member: (string)null),
            (Expression: "PABCSystem.Int(2.5)", Member: "CompareTo"),
            (Expression: "Required", Member: (string)null),
            (Expression: "Required(2)", Member: "Substring"),
            (Expression: "NoArgs", Member: "Substring"),
            (Expression: "NoArgs()", Member: "Substring"),
            (Expression: "(NoArgs)", Member: "Substring"),
            (Expression: "Optional", Member: "Substring"),
            (Expression: "Variadic", Member: "Substring"),
            (Expression: "Choice", Member: "Substring"),
            (Expression: "Choice(2)", Member: "CompareTo"),
            (Expression: "'abc'.Substring", Member: (string)null),
            (Expression: "'abc'.Substring(1)", Member: "Substring"),
            (Expression: "'abc'.ToUpper", Member: "Substring"),
            (Expression: "System.Guid.NewGuid", Member: "CompareTo")
        })
        {
            // Parse a valid surrounding file, then ask the same completion services
            // the IDE uses with an unfinished dot expression at the caret.
            string source = declarations + "\nbegin\n  Print(" + item.Expression + ".ToString);\nend.";
            var converter = new CodeCompletionController().Compile(fileName, source);
            Check(converter.is_compiled && converter.LastConversionError == null,
                "fixture builds for " + item.Expression + ": " + converter.LastConversionError);
            int offset = source.IndexOf("Print(", StringComparison.Ordinal) + "Print(".Length + item.Expression.Length + 1;
            int line = source.Take(offset).Count(ch => ch == '\n');
            int column = offset - source.LastIndexOf('\n', offset - 1) - 1;
            var context = new CompletionTriggerContext { DotPressed = true };
            var expression = CompletionExpressionService.AnalyzeAtCaret(offset, source.Substring(0, offset),
                line, column, language.LanguageIntellisenseSupport, context, KeywordKind.None);
            var parsed = CompletionExpressionService.Parse(language.Parser, fileName, expression.ExpressionText, context);
            Check(!parsed.ShouldAbortCompletion && parsed.Expression != null, "expression parses: " + item.Expression);
            var result = CompletionSymbolService.GetSymbols(converter, line, column, context,
                language.CaseSensitive, expression.ExpressionText, expression.CtrlOrShiftSpaceAfterDot,
                expression.InsidePatternWithDots, expression.Pattern, parsed.Expression, expression.Keyword, true, 0);
            var symbols = result.Symbols;
            Check(item.Member == null ? symbols == null || symbols.Length == 0
                : symbols != null && symbols.Any(s => s != null && s.name == item.Member),
                "dot completion for " + item.Expression + ": " + (item.Member ?? "no result members"));
        }
        Console.WriteLine("PASS: no result completion for uncalled required-argument functions; explicit, zero-argument, optional, params and overloaded calls preserved.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }
}
