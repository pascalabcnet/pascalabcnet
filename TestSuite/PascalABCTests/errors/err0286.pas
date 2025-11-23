const
  TempFile: string = System.IO.Path.GetTempFileName();

begin
  writeln(TempFile);
  writeln(TempFile);
end.