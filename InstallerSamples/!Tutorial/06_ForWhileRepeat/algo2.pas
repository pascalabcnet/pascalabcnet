// Алгоритм вычисления значения многочлена в точке x по схеме Горнера
const n=5;

var 
  x,a: real;

begin
  writeln('Введите значение x: ');
  readln(x);

  writeln('Введите коэффициенты многочлена (',n+1,' штук): ');
  read(a);
  var s := a;
  for var i:=1 to n do
  begin
    read(a);
    s := s*x + a;
  end;

  writeln('Значение многочлена в точке ',x,' равно ',s);
end.