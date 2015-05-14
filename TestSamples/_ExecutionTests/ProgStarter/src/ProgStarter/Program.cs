/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Pavel
 * Datum: 13.01.2008
 * Zeit: 17:48
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
using System;
using System.IO;

namespace ProgStarter
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Program Starter");
			
			// TODO: Implement Functionality Here
			string[] dirs = Directory.GetFiles(Directory.GetParent(Environment.CurrentDirectory).FullName);
			for (int i=0; i<dirs.Length; i++)
			if (Path.GetExtension(dirs[i]) == ".exe")
			{
				System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(dirs[i]);
				psi.CreateNoWindow = true;
				psi.UseShellExecute = false;
				Console.Write(psi.FileName);
				System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);
				while (!p.HasExited);
				ClearLine();
			}
			Console.WriteLine();
			Console.Write("Press any key to exit . . . ");
			Console.ReadKey(true);
		}
		
		public static void ClearLine()
        {
            Console.CursorLeft = 0;
            Console.Write("                                                        ");
            Console.CursorLeft = 0;
        }
	}
}