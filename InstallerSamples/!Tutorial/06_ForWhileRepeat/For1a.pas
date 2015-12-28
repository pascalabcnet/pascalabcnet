// Цикл for. Шаг цикла 2
const n = 25;

var i: integer;

begin
  writeln('Четные числа от 1 до ',2*n);
  for i:=1 to n do
    write(2*i,' ');
  writeln;
  writeln;

  writeln('Четные числа от 1 до ',2*n);
  var x := 2;
  for i:=1 to n do
  begin
    write(x,' ');
    x += 2;
  end;  
  writeln;
  writeln;

  writeln('Нечетные числа от 1 до ',2*n);
  for i:=1 to n do
    write(2*i-1,' ');
  writeln;
  writeln;

  writeln('Нечетные числа от 1 до ',2*n);
  x := 1;
  for i:=1 to n do
  begin
    write(x,' ');
    x += 2;
  end;  
  writeln;
  writeln;
end.