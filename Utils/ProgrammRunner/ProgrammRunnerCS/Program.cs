using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammRunnerCS
{
    class Program
    {
        static void Main(string[] args)
        {
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
        }
    }
}
