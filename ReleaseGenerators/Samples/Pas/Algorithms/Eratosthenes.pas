// "Решето Эратосфена" - вычисление простых чисел
const n = 100000;

var primes: set of integer;

begin
  primes := [2..n];
  
  for var i:=2 to round(sqrt(n)) do
  begin
    if not (i in primes) then
      continue;
    var x := i*i;
    while x<=n do
    begin
      Exclude(primes,x);
      x += i;
    end;
  end;

  writeln('Простые числа < ',n,':');
  writeln(primes);
  writeln;
  writeln('Время вычисления: ',Milliseconds/1000);
end.

