// ÷икл for. a в степени n
var 
  a: real;
  n: integer;

begin
  write('¬ведите a,n: ');
  readln(a,n);
  var p: real := 1;
  for var i:=1 to n do
    p := p * a; // можно p *= a
  writelnFormat('{0} в степени {1} = {2}',a,n,p);  
end.