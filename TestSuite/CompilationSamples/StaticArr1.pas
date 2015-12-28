// Использование массива
const n=10;

var a: array [1..n] of real;

begin
  for var i:=1 to n do
    a[i] := Random*10;
  writeln('Содержимое случайного массива вещественных: ');  
  foreach x: real in a do
    write(x:4:2,'  ');
end.