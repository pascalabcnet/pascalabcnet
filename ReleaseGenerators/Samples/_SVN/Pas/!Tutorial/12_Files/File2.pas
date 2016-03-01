// Файлы строк фиксированной длины
type Str20 = string[20];

var 
  s: Str20;
  f: file of Str20; // file of string определить нельзя, поскольку string имеет переменную длину!
  
begin
  assign(f,'str.dat');
  rewrite(f);
  write(f,'Иванов Иван Иванович');
  write(f,'Петров Петр Петрович');
  write(f,'Сидоров Александр Евг');
  close(f);
  reset(f);
  writeln('Содержимое файла строк: ');
  for var i := 1 to FileSize(f) do
  begin
    read(f,s);
    writeln(s);
  end;
  close(f);
end.