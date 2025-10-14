// Алгоритм вычисления значения многочлена в точке x по схеме Горнера

const n = 5;

begin
  var x := ReadReal('Введите значение x:');
  Print($'Введите коэффициенты многочлена ({n + 1} штук):');
  var a := ReadReal;
  var s := a;
  for var i := 1 to n do
  begin
    a := ReadReal;
    s := s * x + a;
  end;
  Println('Значение многочлена в точке', x, 'равно', s);
end.
