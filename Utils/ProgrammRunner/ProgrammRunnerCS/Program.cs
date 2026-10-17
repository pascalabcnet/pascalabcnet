using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammRunnerCS
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length >= 4 && args[0] == "--net10")
            {
                // Keep the console open exactly as for the legacy Run mode, but
                // launch the .NET 10 program through dotnet without redirecting I/O.
                var net10StartInfo = new System.Diagnostics.ProcessStartInfo();
                net10StartInfo.FileName = args[1];
                net10StartInfo.Arguments = string.Join(" ", new[] { QuoteArgument(args[2]) }
                    .Concat(args.Skip(4).Select(QuoteArgument)));
                net10StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                net10StartInfo.UseShellExecute = false;
                int exitCode;
                using (var program = System.Diagnostics.Process.Start(net10StartInfo))
                {
                    program.WaitForExit();
                    exitCode = program.ExitCode;
                }
                Console.WriteLine(args[3] == "en-US"
                    ? "Program is finished, press any key to continue . . ."
                    : "Программа завершена, нажмите любую клавишу . . .");
                Console.ReadKey(true);
                return exitCode;
            }
            var n = 2;
            var args2 = new string[args.Length > n ? args.Length - n : 0];
            for (var i = n; i < args.Length; i++)
                args2[i - n] = args[i];
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = args[n-2];
            psi.Arguments = string.Join(" ", args2);
            //psi.CreateNoWindow := true;
            psi.UseShellExecute = false;
            var p = new System.Diagnostics.Process();
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit();
            var msg = "Программа завершена, нажмите любую клавишу . . .";
            if (args[n-1] == "en-US")
                msg = "Program is finished, press any key to continue . . .";
            System.Console.WriteLine(msg);
            System.Console.ReadKey(true);
            return 0;
        }

        private static string QuoteArgument(string value)
        {
            var result = new StringBuilder("\"");
            int backslashes = 0;
            foreach (char ch in value)
            {
                if (ch == '\\') { backslashes++; continue; }
                if (ch == '"')
                {
                    result.Append('\\', backslashes * 2 + 1);
                    result.Append('"');
                    backslashes = 0;
                    continue;
                }
                result.Append('\\', backslashes);
                backslashes = 0;
                result.Append(ch);
            }
            result.Append('\\', backslashes * 2);
            result.Append('"');
            return result.ToString();
        }
    }
}
