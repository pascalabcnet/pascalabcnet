{$apptype windows}

var p: System.Diagnostics.Process;
    i: integer;
    
begin
try
  p := new System.Diagnostics.Process();
  p.StartInfo.UseShellExecute := false;
  p.StartInfo.CreateNoWindow := true;
  p.StartInfo.FileName := CommandLineArgs[0];
  p.StartInfo.Arguments := '"'+CommandLineArgs[1]+'"';
  for i:=2 to CommandLineArgs.Length-1 do
    p.StartInfo.Arguments := p.StartInfo.Arguments + ' ' + '"'+CommandLineArgs[i]+'"';
  p.Start;
  p.WaitForExit;
except
end;
end.