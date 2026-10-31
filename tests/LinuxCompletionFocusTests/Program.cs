using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

internal static class Program
{
    private static void Pump()
    {
        var watch = Stopwatch.StartNew();
        while (watch.ElapsedMilliseconds < 180) { Application.DoEvents(); Thread.Sleep(5); }
    }

    private static void Check(bool value, string message)
    {
        if (!value) throw new Exception(message);
        Console.WriteLine("PASS " + message);
    }

    [STAThread]
    private static int Main(string[] args)
    {
        int result = 1;
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
        EventHandler run = null;
        run = (sender, eventArgs) => {
            Application.Idle -= run;
            try
            {
                if (args.Length > 1 && args[1] == "mouse")
                {
                    TextAreaMouseStateTests.Run(System.Reflection.Assembly.LoadFrom(args[0]), Check);
                    TextAreaHoverTimerTests.Run(System.Reflection.Assembly.LoadFrom(args[0]), Check);
                    TextAreaToolTipFontTests.Run(System.Reflection.Assembly.LoadFrom(args[0]), Check);
                    Console.WriteLine("All mouse state checks passed.");
                    result = 0;
                    return;
                }
                bool listOnly = args.Length > 1 && args[1] == "list";
                if (!listOnly) DocumentationTests.Run(Check, Pump);
                if (args.Length >= 1) CompletionInsertionTests.Run(args[0], Check, Pump, listOnly);
                Console.WriteLine("All Linux completion checks passed. OS: " + Environment.OSVersion);
                result = 0;
            }
            catch (Exception error) { Console.Error.WriteLine(error); }
            finally { Application.ExitThread(); }
        };
        Application.Idle += run;
        Application.Run();
        return result;
    }
}
