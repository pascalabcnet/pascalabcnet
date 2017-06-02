// Передача массива как параметра подпрограмм
const n=10;

type RealArr = array [1..n] of real;

procedure FillArrByRandom(var a: RealArr; n: integer);
begin
  for var i:=1 to n do
    a[i] := Random*10;
end;

procedure PrintArr(const a: RealArr; n: integer);
begin
  foreach x: real in a do
    write(x:4:2,'  ');
end;

var a: RealArr;

begin
  FillArrByRandom(a,n);
  writeln('Содержимое случайного массива вещественных: ');  
  PrintArr(a,n);
end.