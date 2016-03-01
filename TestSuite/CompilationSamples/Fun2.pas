// Функция Power

function Power(x: real; n: integer): real;
begin
  Result := 1;
  for var i:=1 to n do
    Result *= x;
end;

var 
  x: real; 
  n: integer;

begin
  x := 2; n := 5;  
  writelnFormat('{0} в степени {1} = {2}',x,n,Power(x,n));
end.  