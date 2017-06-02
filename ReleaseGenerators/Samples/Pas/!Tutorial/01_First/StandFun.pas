// Стандартные функции
var x: real;

begin 
  write('Введите x: ');
  readln(x);
  writeln('Квадрат ',x,' равен ',sqr(x));
  writeln(x,' в степени 5 равно ',power(x,5));
  writeln('Квадратный корень из ',x,' равен ',sqrt(x));
  writeln('Модуль ',x,' равен ',abs(x));
  writeln('Натуральный логарифм ',x,' равен ',ln(x));
  writeln('Синус ',x,' равен ',sin(x));
  writeln('Косинус ',x,' равен ',cos(x));
end.