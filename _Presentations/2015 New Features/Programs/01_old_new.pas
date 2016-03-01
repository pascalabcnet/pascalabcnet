function fact(n: integer): integer;
begin
  var p := 1;
  for var i:=1 to n do
    p *= i;
  result := p;
end;

begin
  var n := ReadInteger('Введите n:');
  writeln('n!=',fact(n));  
end.