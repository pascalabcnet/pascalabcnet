// Вывод результатов вычислений. Используются переменные и процедура ввода

begin
  var (a,b) := ReadlnInteger2('Введите a и b:');
  Writeln;
  Writeln(a,' + ',b,' = ',a+b);
  Writeln(a,' - ',b,' = ',a-b);
  Writeln(a,' * ',b,' = ',a*b);
  Writeln(a,' / ',b,' = ',a/b);
end.