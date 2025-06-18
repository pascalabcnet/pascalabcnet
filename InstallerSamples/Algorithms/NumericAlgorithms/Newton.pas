function f(x: real) := exp(x) - 4; // Функция f(x) = exp(x) - 4
function df(x: real) := exp(x);    // Производная f'(x) = exp(x)

function NewtonMethod(x, eps: real): real;
begin
  while abs(f(x)) > eps do
    x := x - f(x) / df(x); 
  Result := x;
end;

begin
  var x := 0.5;       // Начальное приближение
  var eps := 0.00001;  // Заданная точность

  var root := NewtonMethod(x, eps);
  
  Println('Корень уравнения: ', root);
end.