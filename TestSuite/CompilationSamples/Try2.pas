// Обработка нескольких исключений
var x: integer;

begin
  try 
    writeln('Введите число (1 - ошибка деления на 0): ');
    readln(x);
    x := 10 div (x-1);
  except
    on System.FormatException do
      writeln('Ошибка ввода');
    on System.DivideByZeroException do
      writeln('Деление на 0');  
  end;
end.
