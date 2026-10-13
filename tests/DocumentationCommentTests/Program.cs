using System;
using System.Collections.Generic;
using System.Linq;
using CodeCompletion;
using Languages.Pascal;
using PascalABCCompiler;
using PascalABCCompiler.Errors;
using PascalABCCompiler.Parsers;

internal static class Program
{
    private static void Main()
    {
        var language = PascalLanguageRegistration.RegisterPascalLanguage();
        CodeCompletionBootstrap.Initialize("ru");
        CheckTypeAndConstructor(language);
        foreach (string ending in new[] { "CRLF", "LF", "mixed", "mixed-reverse" })
        foreach (bool period in new[] { false, true })
        {
            string lastLine = "так как преобразования будут выполнены ещё раз" + (period ? "." : "");
            string[] lines =
            {
                "unit DocTest;", "interface", "type Pipeline = class",
                "  /// Применяет только обученные шаги и возвращает результат.",
                "  ///", "  /// Метод предназначен для анализа промежуточного",
                "  /// результата предобработки.", "  ///",
                "  /// Полученный результат не следует повторно подавать в pipeline,",
                "  /// " + lastLine,
                "  function Transform(value: integer): integer;",
                "  /// Описание соседнего метода.",
                "  function Next(value: integer): integer;",
                "end;", "implementation", "end."
            };
            string source = string.Concat(lines.Select((sourceLine, index) => sourceLine +
                (ending == "CRLF" || ending == "mixed" && index % 2 == 0 ||
                 ending == "mixed-reverse" && index % 2 != 0 ? "\r\n" : "\n")));
            string expected = "Применяет только обученные шаги и возвращает результат.\r\n" +
                "\r\n Метод предназначен для анализа промежуточного\r\n результата предобработки.\r\n" +
                "\r\n Полученный результат не следует повторно подавать в pipeline,\r\n " + lastLine;
            string name = ending + (period ? ", trailing period" : ", no trailing period");
            var sections = language.DocParser.BuildTree(source);
            Check(sections.sections.Count == 2, name + ": consecutive /// lines form one block");
            var section = sections.sections[0];
            Check(section.text.Trim() == expected, name + ": full text and empty doc lines retained");
            Check(section.source_context.begin_position.line_num == 4 &&
                  section.source_context.end_position.line_num == 10,
                name + ": source line range preserved");
            var errors = new List<Error>();
            var tree = language.Parser.GetCompilationUnit("DocTest.pas", source, errors,
                new List<CompilerWarning>(), ParseMode.Normal, false);
            Check(tree != null && errors.Count == 0, name + ": fixture parses");
            var docs = new DocumentationConstructor().Construct(tree, sections);
            Check(docs.Values.Contains(expected) && docs.Values.Contains("Описание соседнего метода."),
                name + ": documentation attaches to the intended methods");
            var converter = new CodeCompletionController().Compile("DocTest.pas", source);
            Check(converter.is_compiled, name + ": IntelliSense model builds");
            int offset = source.IndexOf("Transform", StringComparison.Ordinal) + 2;
            int line = source.Substring(0, offset).Count(ch => ch == '\n');
            int column = offset - source.LastIndexOf('\n', offset - 1) - 1;
            string hover = HoverService.GetDescription(language, converter, "DocTest.pas", source,
                offset, line, column);
            Check(hover != null && hover.Contains(expected) && !hover.Contains("Описание соседнего метода."),
                name + ": actual hover contains the entire documentation, not only the last line");
        }
        foreach (string ending in new[] { "\r\n", "\n" })
        {
            var separate = language.DocParser.BuildTree("/// First" + ending + ending + "/// Second" + ending + "procedure p;");
            Check(separate.sections.Count == 2, "a non-doc blank line separates documentation blocks");
        }
        Console.WriteLine("All documentation comment checks passed.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
        Console.WriteLine("PASS " + message);
    }

    private static void CheckTypeAndConstructor(Languages.Facade.ILanguage language)
    {
        foreach (string ending in new[] { "CRLF", "LF", "mixed", "mixed-reverse" })
        foreach (bool period in new[] { false, true })
        {
            string suffix = period ? "." : "";
            string source = "unit CtorDoc;\r\ninterface\r\ntype\r\n" +
                "  /// Масштабирует признаки в заданный диапазон.\r\n" +
                "  /// Используется без центрирования" + suffix + "\r\n" +
                "  MinMaxScaler = class\r\npublic\r\n" +
                "  /// Создаёт MinMaxScaler с диапазоном [rangeMin, rangeMax].\r\n" +
                "  /// По умолчанию масштабирование выполняется к [0, 1]" + suffix + "\r\n" +
                "  constructor Create(rangeMin: real := 0.0; rangeMax: real := 1.0);\r\n" +
                "  /// Отдельная перегрузка конструктора.\r\n" +
                "  constructor Create(count: integer);\r\n" +
                "end;\r\nimplementation\r\nend.\r\n";
            source = string.Concat(source.Split(new[] { "\r\n" }, StringSplitOptions.None)
                .Select((sourceLine, index) => sourceLine +
                    (ending == "CRLF" || ending == "mixed" && index % 2 == 0 ||
                     ending == "mixed-reverse" && index % 2 != 0 ? "\r\n" : "\n")));
            var converter = new CodeCompletionController().Compile("CtorDoc.pas", source);
            Check(converter.is_compiled, "constructor fixture builds");
            foreach (string marker in new[] { "MinMaxScaler = class", "Create(rangeMin", "Create(count" })
            {
                int offset = source.IndexOf(marker, StringComparison.Ordinal) + 2;
                int line = source.Substring(0, offset).Count(ch => ch == '\n');
                int column = offset - source.LastIndexOf('\n', offset - 1) - 1;
                PascalABCCompiler.Parsers.KeywordKind keyword;
                string shortExpression;
                string expression = language.LanguageIntellisenseSupport.FindExpressionFromAnyPosition(
                    offset, source, line, column, out keyword, out shortExpression);
                Check(expression != null && !expression.Contains("центрирования") &&
                      !expression.Contains("[0, 1]"), "hover expression does not include preceding documentation");
                string hover = HoverService.GetDescription(language, converter, "CtorDoc.pas", source, offset, line, column);
                string expected = marker.StartsWith("Create(count") ? "Отдельная перегрузка конструктора." :
                    marker.StartsWith("Create") ? "Создаёт MinMaxScaler с диапазоном [rangeMin, rangeMax].\r\n " +
                        "По умолчанию масштабирование выполняется к [0, 1]" + suffix :
                    "Масштабирует признаки в заданный диапазон.\r\n Используется без центрирования" + suffix;
                Check(hover != null && hover.Contains(expected),
                    ending + ", period=" + period + ": type/constructor documentation: " + marker);
                if (marker.StartsWith("Create(rangeMin"))
                    Check(keyword == PascalABCCompiler.Parsers.KeywordKind.Constructor &&
                          hover.Contains("rangeMin") && hover.Contains("rangeMax"),
                        "constructor header hover selects the declared constructor, not a synthetic default");
                if (marker.StartsWith("Create(count"))
                    Check(hover.Contains("count") && !hover.Contains("По умолчанию масштабирование"),
                        "constructor overloads keep their own signature and documentation");
            }
        }
        foreach (string ending in new[] { "\r\n", "\n" })
        foreach (string prefix in new[] { "value.", "value. // Comment.", "value. // 'Quoted' comment." })
        {
            string text = prefix + ending + "  Member";
            string shortExpression;
            PascalABCCompiler.Parsers.KeywordKind keyword;
            string expression = language.LanguageIntellisenseSupport.FindExpressionFromAnyPosition(
                text.Length - 2, text, 1, 4, out keyword, out shortExpression);
            Check(expression != null && expression.Replace("\r", "").Replace("\n", "").Replace(" ", "") == "value.Member",
                "multiline member access remains intact with and without line comments");
        }
    }
}
