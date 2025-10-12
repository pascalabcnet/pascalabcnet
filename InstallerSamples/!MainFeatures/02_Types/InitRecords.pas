// Инициализаторы полей записи
type 
  Frac = record
    num: integer := 0;
    denom := 1; // автоопределеине типа - denom: integer
  end;
  

begin
  var f: Frac;  
  var f1: Frac := (num: 2; denom: 3);
  Println(f.num,'/',f.denom);
  Println(f1.num,'/',f1.denom);
end.  