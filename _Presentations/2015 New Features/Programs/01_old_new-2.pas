function fact(n: integer): integer;
var p: integer;
    i: integer;
begin
  p := 1;
  for i:=1 to n do
    p := p * i;
  fact := p;  
end;

var 
  n: integer;
  p: integer;    

begin
  read(n);
  writeln('n!=',fact(n));
end.