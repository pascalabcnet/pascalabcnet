// Демонстрация операции shl
begin
  writeln('Степени двойки');
  writeln(' n         2^n');
  for var i:=0 to 30 do
    writeln(i:2,(1 shl i):12);
end.

