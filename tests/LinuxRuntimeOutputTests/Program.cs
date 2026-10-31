using System;
using System.Text;
using VisualPascalABC;

internal static class Program
{
    private const string Warning = "Could not set X locale modifiers";
    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            string[] inputs = {
                Warning + "\n", Warning + "\r\n", Warning + "\r",
                "Ошибка программы\n" + Warning + "\n[READLNSIGNAL][EXCEPTION]System.Exception[END]",
                "\n" + Warning + "\r\n\n" + Warning + "\nХвост",
                "prefix " + Warning + "\n", Warning + " extra\n",
                Warning, "Could not set X", "[READLNSIGNAL]", "Обычный stderr без перевода строки"
            };
            foreach (string input in inputs)
            {
                string expected = input;
                if (input == Warning + "\n" || input == Warning + "\r\n" || input == Warning + "\r") expected = "";
                if (input.StartsWith("Ошибка программы")) expected = "Ошибка программы\n[READLNSIGNAL][EXCEPTION]System.Exception[END]";
                if (input.StartsWith("\n")) expected = "\n\nХвост";
                for (int split = 0; split <= input.Length; split++)
                {
                    var filter = new MonoX11WarningFilter();
                    string actual = filter.Feed(input.Substring(0, split)) + filter.Feed(input.Substring(split)) + filter.Complete();
                    if (actual != expected) throw new Exception("Wrong output at split " + split + ": " + input);
                }
                var fragmented = new MonoX11WarningFilter();
                var output = new StringBuilder();
                foreach (char ch in input) output.Append(fragmented.Feed(ch.ToString()));
                output.Append(fragmented.Complete());
                if (output.ToString() != expected) throw new Exception("Wrong character-by-character output: " + input);
            }
            var immediate = new MonoX11WarningFilter();
            if (immediate.Feed("[READLNSIGNAL]") != "[READLNSIGNAL]" || immediate.Feed("Error!") != "Error!")
                throw new Exception("Ordinary output or input request was buffered.");
            Console.WriteLine("PASS exact Mono warning suppressed across every chunk boundary and line ending.");
            Console.WriteLine("PASS other errors, Unicode, similar messages, partial lines and runtime signals preserved.");
            if (args.Length != 0)
            {
                var assembly = System.Reflection.Assembly.LoadFrom(System.IO.Path.GetFullPath(args[0]));
                var type = assembly.GetType("VisualPascalABC.MonoX11WarningFilter", true);
                var instance = Activator.CreateInstance(type, true);
                var feed = type.GetMethod("Feed");
                var complete = type.GetMethod("Complete");
                string actual = (string)feed.Invoke(instance, new object[] { "Could not set X " }) +
                    (string)feed.Invoke(instance, new object[] { "locale modifiers\r" }) +
                    (string)feed.Invoke(instance, new object[] { "\nОшибка программы\n[READLNSIGNAL]" }) +
                    (string)complete.Invoke(instance, null);
                if (actual != "Ошибка программы\n[READLNSIGNAL]") throw new Exception("Packaged IDE filter failed.");
                Console.WriteLine("PASS actual Linux IDE contains the working filter: " + args[0]);
                OutputColorTests.Run(assembly);
            }
            return 0;
        }
        catch (Exception e) { Console.Error.WriteLine(e); return 1; }
    }
}
