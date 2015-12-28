// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
begin
  var args := System.Environment.GetCommandLineArgs();
  var args2 := new string[args.Length > 2?args.Length-2:0];
  for var i := 2 to args.Length - 1 do
    args2[i-2] := args[i];
  var psi := new System.Diagnostics.ProcessStartInfo();
  psi.FileName := args[1];
  psi.Arguments := string.Join(' ',args2);
  //psi.CreateNoWindow := true;
  psi.UseShellExecute := false;
  var p := new System.Diagnostics.Process();
  p.StartInfo := psi;
  p.Start();
  p.WaitForExit();
  System.Console.WriteLine('Программа завершена, нажмите любую клавишу . . .');
  System.Console.ReadKey(true);
end.