using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Languages.Pascal;
using PascalABCCompiler.Errors;
using PascalABCCompiler.Parsers;

internal static class Program
{
    private const string Package = "{$package Example.Package 1.2.3}";

    private static int Main()
    {
        var language = PascalLanguageRegistration.RegisterPascalLanguage();
        string directory = Path.Combine(Path.GetTempPath(), "pabc-package-placement-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        int checks = 0;
        try
        {
            Action<string, string, bool, int> parse = (name, source, late, expectedLine) =>
            {
                var errors = new List<Error>();
                var tree = language.Parser.GetCompilationUnit(Path.Combine(directory, "Sample.pas"), source,
                    errors, new List<CompilerWarning>(), ParseMode.Normal, false);
                bool mustReject = false;
#if NET10_0_OR_GREATER
                mustReject = late;
#endif
                if (mustReject)
                {
                    if (tree != null || errors.Count != 1 ||
                        !(errors[0].Message.Contains("PACKAGE_DIRECTIVE_MUST_PRECEDE_CODE") ||
                          errors[0].Message.Contains("{$package}")))
                        throw new Exception(name + ": missing placement diagnostic: " + string.Join("; ", errors));
                    if (((LocatedError)errors[0]).SourceLocation.BeginPosition.Line != expectedLine)
                        throw new Exception(name + ": incorrect diagnostic line");
                    string expectedFile = name == "package in late include" ? "header.inc" : "Sample.pas";
                    if (Path.GetFileName(((LocatedError)errors[0]).FileName) != expectedFile)
                        throw new Exception(name + ": incorrect diagnostic file");
                }
                else if (tree == null || errors.Count != 0)
                    throw new Exception(name + ": unexpected parse failure: " + string.Join("; ", errors));
                checks++;
            };

            foreach (string newline in new[] { "\n", "\r\n" })
            {
                Func<string, string> lines = text => text.Replace("\n", newline);
                parse("before begin", lines(Package + "\nbegin end."), false, 0);
                parse("before program", lines(Package + "\nprogram Sample; begin end."), false, 0);
                parse("before unit", lines(Package + "\nunit Sample; interface implementation end."), false, 0);
                parse("before uses", lines(Package + "\nuses System; begin end."), false, 0);
                parse("before short program", lines(Package + "\n## Println(1);"), false, 0);
                parse("comments and directives", lines("// uses Fake;\n(* program Fake; *)\n{$define HEADER}\n" + Package + "\nbegin end."), false, 0);
                parse("inactive header code", lines("{$ifdef NEVER_DEFINED}\nuses Fake;\n{$endif}\n" + Package + "\nbegin end."), false, 0);
                parse("duplicate packages", lines(Package + "\n" + Package + "\nbegin end."), false, 0);
                parse("after uses", lines("uses System;\n" + Package + "\nbegin end."), true, 2);
                parse("after program", lines("program Sample;\n" + Package + "\nbegin end."), true, 2);
                parse("after unit", lines("unit Sample;\n" + Package + "\ninterface implementation end."), true, 2);
                parse("after interface", lines("unit Sample; interface\n" + Package + "\nimplementation end."), true, 2);
                parse("after implementation", lines("unit Sample; interface implementation\n" + Package + "\nend."), true, 2);
                parse("after declaration", lines("var x: integer;\n" + Package + "\nbegin end."), true, 2);
                parse("inside body", lines("begin\n" + Package + "\nend."), true, 2);
                parse("after short marker", lines("##\n" + Package + "\nPrintln(1);"), true, 2);
                parse("inactive late package", lines("begin\n{$ifdef NEVER_DEFINED}\n" + Package + "\n{$endif}\nend."), false, 0);
                parse("comment and string contents", lines("(* " + Package + " *)\nbegin var s := '" + Package + " uses Fake;'; end."), false, 0);
                parse("interpolated string", lines(Package + "\nbegin var n := 2; var s := $'{n}..{n+1}'; end."), false, 0);
                parse("language text in interpolation", lines("begin var s := $'{1} uses Fake;'; end."), false, 0);

                File.WriteAllText(Path.Combine(directory, "header.inc"), lines(Package + "\n"));
                parse("package in header include", lines("{$include 'header.inc'}\nbegin end."), false, 0);
                parse("package in late include", lines("uses System;\n{$include 'header.inc'}\nbegin end."), true, 1);
                File.WriteAllText(Path.Combine(directory, "code.inc"), lines("uses System;\n"));
                parse("code in include before package", lines("{$include 'code.inc'}\n" + Package + "\nbegin end."), true, 2);
            }
            Console.WriteLine("PASS: " + checks + " package-placement checks; modern header-only, legacy unchanged; no restore/network.");
            return 0;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error);
            return 1;
        }
        finally
        {
            foreach (string file in Directory.GetFiles(directory)) File.Delete(file);
            Directory.Delete(directory);
        }
    }
}
