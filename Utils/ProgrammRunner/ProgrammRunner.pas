// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
begin
  var args := System.Environment.GetCommandLineArgs();
  var args2 := new string[args.Length > 3?args.Length-3:0];
  for var i := 3 to args.Length - 1 do
    args2[i-3] := args[i];
  var psi := new System.Diagnostics.ProcessStartInfo();
  psi.FileName := args[1];
  psi.Arguments := string.Join(' ', args2);
  //psi.CreateNoWindow := true;
  psi.UseShellExecute := false;
  var p := new System.Diagnostics.Process();
  p.StartInfo := psi;
  p.Start();
  p.WaitForExit();
  var msg := 'Программа завершена, нажмите любую клавишу . . .';
  if args[2] = 'en-US' then
    msg := 'Program is finished, press any key to continue . . .';
  System.Console.WriteLine(msg);
  System.Console.ReadKey(true);
end.