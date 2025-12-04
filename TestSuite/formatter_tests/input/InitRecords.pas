// Инициализаторы полей записи
type 
  Frac = record
    num: integer := 0;
    denom := 1; // автоопределеине типа - denom: integer
  end;
  
var 
  f: Frac;  
  f1: Frac := (num: 2; denom: 3);

begin
  writeln(f.num,'/',f.denom);
  writeln(f1.num,'/',f1.denom);
end.  