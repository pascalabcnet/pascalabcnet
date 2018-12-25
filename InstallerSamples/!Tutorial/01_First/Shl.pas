// Демонстрация операции shl
begin
  Writeln('Степени двойки');
  Writeln(' n         2^n');
  for var i:=0 to 30 do
    Writeln(i:2,(1 shl i):12);
end.

