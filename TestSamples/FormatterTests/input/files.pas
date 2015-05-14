//Работа с текстовыми файлами
var f : Text;
    s : string;
begin
  AssignFile(f,'output.txt');
  Rewrite(f);
  writeln(f,'Это текстовый файл ');
  writeln(f,235);
  writeln(f,3.14);
  writeln(f,'string');
  CloseFile(f);
  AssignFile(f,'output.txt');
  Reset(f);
  while not Eof(f) do begin
    readln(f,s);
    writeln(s);
  end;
  CloseFile(f);
  readln;
end.