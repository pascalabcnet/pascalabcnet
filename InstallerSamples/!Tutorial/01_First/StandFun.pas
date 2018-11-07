// Стандартные функции
var x: real;

begin 
  write('Введите x: ');
  Readln(x);
  Writeln('Квадрат ',x,' равен ',Sqr(x));
  Writeln(x,' в степени 5 равно ',Power(x,5));
  Writeln('Квадратный корень из ',x,' равен ',Sqrt(x));
  Writeln('Модуль ',x,' равен ',Abs(x));
  Writeln('Натуральный логарифм ',x,' равен ',Ln(x));
  Writeln('Синус ',x,' равен ',Sin(x));
  Writeln('Косинус ',x,' равен ',Cos(x));
end.