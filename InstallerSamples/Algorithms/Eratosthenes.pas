// "Решето Эратосфена" - вычисление простых чисел
const n = 100000;

begin
  var primes := HSet(2..n);
  
  for var i:=2 to Round(Sqrt(n)) do
  begin
    if not (i in primes) then
      continue;
    var x := i*i;
    while x<=n do
    begin
      primes -= x;
      x += i;
    end;
  end;

  Writeln('Простые числа < ',n,':');
  Writeln(primes);
  Writeln;
  Writeln('Время вычисления: ',Milliseconds/1000);
end.

