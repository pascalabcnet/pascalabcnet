uses System;

var TReal: &Type;
    TInteger: System.Type;
    &if: integer;

begin
  TInteger := typeof(integer);
  Writeln(TInteger);
  TReal := typeof(real);
  Writeln(TReal);
  &if := &if + 1;
  Writeln(&if);
  Readln;
end.