// Присваивания += -= *= /=
var 
  i: integer;
  r: real;

begin
  i := 1;
  Writeln('i := 1; i = ',i);
  i += 2; // Увеличение на 2
  Writeln('i += 2; i = ',i);
  i *= 3; // Увеличение в 3 раза
  Writeln('i *= 3; i = ',i);
  Writeln;
  r := 6;
  Writeln('r := 6; r = ',r);
  r /= 2;
  Writeln('r /= 2; r = ',r);
end.