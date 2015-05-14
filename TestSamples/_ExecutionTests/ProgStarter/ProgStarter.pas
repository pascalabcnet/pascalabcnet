uses System, System.IO, CRT;

begin
  Writeln('Program starter');
  var dirs := Directory.GetFiles(Directory.GetParent(Environment.CurrentDirectory).FullName);
  foreach dir:string in dirs do
    if Path.GetExtension(dir) = '.exe' then begin
      var psi := new System.Diagnostics.ProcessStartInfo(dir);
      psi.CreateNoWindow := true;
		  psi.UseShellExecute := false;
	    Console.Write(psi.FileName);
		  var p := System.Diagnostics.Process.Start(psi);
		  while not p.HasExited do 
		    Sleep(10);
		  ClearLine;
    end;
  Writeln;
	Write('Press any key to exit . . . ');
	Readkey;
end.