// Нетипизированные файлы
// Запись в файл 10 чисел, при следующем запуске - чтение этих чисел и удаление файла
const filename = 'u.dat';

var 
  f: file;
  r: real;
  s: string[10];
  k: integer;
  c: char;

begin
  if not FileExists(filename) then
  begin
    assign(f,filename);
    rewrite(f);
    writeln('Запись в файл ',filename);
    
    r := random*10; 
    for var i:=0 to 10 do
      s := s + Chr(Ord('A')+Random(26));
    c := Chr(Ord('A')+Random(26));
    k := Random(100);
    
    writeln(r);
//    writeln(s);
    writeln(c);
    writeln(k);
    // Запись в файл данных разных типов
    write(f,r);
//    writeln(f,s);
    write(f,c);
    write(f,k);
    close(f);  
  end
  else 
  begin
    assign(f,filename);
    reset(f);
    writeln('Чтение из файла ',filename);

    // Чтение из файла данных разных типов
    read(f,r,{s,}c,k);
    writeln(r);
//    writeln(s);
    writeln(c);
    writeln(k);

    close(f);  
    erase(f);
  end;  
end.