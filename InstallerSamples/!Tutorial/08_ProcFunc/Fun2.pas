// Функция Power

function Power(x: real; n: integer): real;
begin
  Result := 1;
  for var i:=1 to n do
    Result *= x;
end;

begin
  var x: real := 2; 
  var n: integer := 5;
  Println($'{x} в степени {n} = {Power(x,n)}');
end.  