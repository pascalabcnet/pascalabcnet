function f(x: real) := exp(x) - 4; // Короткое определение функции

function BisectionMethod(a, b, eps: real): real;
begin
  var fa := f(a); // Вычисляем f(a) один раз в начале
  while abs(b - a) > eps do
  begin
    var c := (a + b) / 2;
    var fc := f(c); // Вычисляем функцию только один раз на итерацию
    if fc = 0 then
      break; // Выход из цикла, если найден точный корень
    if fa * fc < 0 then
      b := c
    else
      (a, fa) := (c, fc); // Кортежное присваивание
  end;
  Result := (a + b) / 2; // Возвращаем среднюю точку как приближенный корень
end;

begin
  var (a, b) := (0.0, 3.0); // Множественное присваивание для a и b
  var eps := 0.000001;
  
  var root := BisectionMethod(a, b, eps);
  
  Println('Корень уравнения:', root);
end.