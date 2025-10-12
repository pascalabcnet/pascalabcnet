// Бестиповые файлы

begin
  var f: file := CreateBinary('a.dat');
  // Записываем в файл данные любых типов
  f.Write(1,2.5,'Hello');  
  f.Close;
  f.Reset;
  // Считываем эти данные из файла
  var i: integer;
  var r: real;
  var s: string;
  Read(f,i,r,s);
  Print(i,r,s);
  f.Close;
end.