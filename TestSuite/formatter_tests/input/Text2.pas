// Программа, выводящая текст в файл a.txt
var 
  f: Text;
  a: array of string := ('Каждый','охотник','желает','знать','где','сидит','фазан');

begin
  assign(f,'a.txt');
  rewrite(f);
  for var i:=0 to a.Length-1 do 
    writeln(f,a[i]);
  close(f);
  writeln('Текст записан в файл a.txt');
end.