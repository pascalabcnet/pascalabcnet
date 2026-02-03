function FastPower(a: real; n: integer): real;
begin
  if n = 0 then exit(1); // a^0 = 1
  if n = 1 then exit(a); // a^1 = a
  if n < 0 then exit(1 / FastPower(a, -n)); // отрицательная степень
  
  var half := FastPower(a, n div 2);
  if n mod 2 = 0 then exit(half * half); // чётная степень
  exit(half * half * a); // нечётная
end;

begin
  Writeln('2^10 = ', FastPower(2, 10)); // 1024
  Writeln('3^-2 = ', FastPower(3, -2)); // 0.111...
end.