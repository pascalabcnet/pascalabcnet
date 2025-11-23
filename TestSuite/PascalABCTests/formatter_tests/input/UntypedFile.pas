// Бестиповые файлы
var 
  f: file;
  i: integer;
  r: real;
  s: string;

begin
  assign(f,'a.dat');
  rewrite(f);
  // Записываем в файл данные любых типов
  write(f,1,2.5,'Hello');  
  close(f);
  reset(f);
  // Считываем эти данные из файла
  read(f,i,r,s);
  write(i,' ',r,' ',s);
  close(f);
end.