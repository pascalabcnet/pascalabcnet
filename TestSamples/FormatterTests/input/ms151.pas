// массив файлов не инициализируется
var ff: array [1..4] of file of integer;

begin
  assign(ff[1],'a.dat');
end.