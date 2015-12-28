function fact(n: integer): integer;
begin
  result := 1;
  for var i:=1 to n do
    result *= i;
end;

begin
  var n := ReadInteger('Введите n:');
  writeln('n!=',fact(n));  
end.