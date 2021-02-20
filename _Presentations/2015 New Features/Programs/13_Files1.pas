var 
  f: Text;
  s: string;
begin
  Assign(f,'13_Files1.pas');
  Reset(f);
  while not eof(f) do
  begin
    readln(f,s);
    writeln(s);
  end;
  Close(f);
end.