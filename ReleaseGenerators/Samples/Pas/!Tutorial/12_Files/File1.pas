// Типизированные файлы
// Запись в файл 10 чисел, при следующем запуске - чтение этих чисел и удаление файла
const filename = 'a.dat';

var f: file of integer;

begin
  if not FileExists(filename) then
  begin
    assign(f,filename);
    rewrite(f);
    writeln('Запись в файл ',filename);
    for var i:=1 to 10 do
    begin
      var x := random(100); 
      write(x,' ');
      write(f,x);
    end;  
    close(f);  
  end
  else 
  begin
    assign(f,filename);
    reset(f);
    writeln('Чтение из файла ',filename);
    for var i:=1 to 10 do
    begin
      var x: integer;
      read(f,x);
      write(x,' ');
    end;  
    close(f);  
    erase(f);
  end;  
end.