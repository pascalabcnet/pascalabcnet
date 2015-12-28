// Программа, выводящая текст из своего файла
var 
  f: Text;

begin
  assign(f,'Text1.pas');
  reset(f);
  while not eof(f) do
  begin
    var s: string;
    readln(f,s);
    writeln(s);
  end;
  close(f);
end.