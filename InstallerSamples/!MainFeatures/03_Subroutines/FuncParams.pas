// Переменное число параметров 
function Sum(params arg: array of integer): integer;
begin
  Result := 0;
  foreach var x in arg do
    Result += x;
end;

begin
  writeln(Sum(1,2,3));
  writeln(Sum(4,5,6,7));
end.  