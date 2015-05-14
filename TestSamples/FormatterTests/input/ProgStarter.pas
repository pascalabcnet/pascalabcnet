uses System, System.IO;
procedure ClearLine;
begin
   Console.CursorLeft := 0;
   Console.Write('                                                        ');
   Console.CursorLeft := 0;
end;

begin
  Console.WriteLine('Program starter');
  var dirs := Directory.GetFiles(Directory.GetParent(Environment.CurrentDirectory).FullName);
  for var i := 0 to dirs.Length-1 do
  if Path.GetExtension(dirs[i]) = '.exe' then
  begin
    var psi := new System.Diagnostics.ProcessStartInfo(dirs[i]);
    psi.CreateNoWindow := true;
		psi.UseShellExecute := false;
	  Console.Write(psi.FileName);
		var p := System.Diagnostics.Process.Start(psi);
		while not p.HasExited do;
		ClearLine;
  end;
  Console.WriteLine;
	Console.Write('Press any key to exit . . . ');
	Console.ReadKey(true);

end.