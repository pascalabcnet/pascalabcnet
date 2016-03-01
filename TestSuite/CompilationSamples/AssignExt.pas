// Присваивания += -= *= /=
var 
  i: integer;
  r: real;

begin
  i := 1;
  writeln('i := 1; i = ',i);
  i += 2; // Увеличение на 2
  writeln('i += 2; i = ',i);
  i *= 3; // Увеличение в 3 раза
  writeln('i *= 3; i = ',i);
  writeln;
  r := 6;
  writeln('r := 6; r = ',r);
  r /= 2;
  writeln('r /= 2; r = ',r);
end.