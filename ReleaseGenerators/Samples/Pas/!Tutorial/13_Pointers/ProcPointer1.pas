// Процедурные переменные

function add(a,b: integer): integer;
begin
  Result := a + b;
end;

function mult(a,b: integer): integer;
begin
  Result := a * b;
end;

var p: function (a,b: integer): integer;
  
  
begin
  p := add;
  writeln('Сумма 2 и 3 равна ',p(2,3));
  p := mult;
  writeln('Произведение 2 и 3 равно ',p(2,3));
end.  