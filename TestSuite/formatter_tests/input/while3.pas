// Цикл while. Сумма цифр положительного числа
var m: integer;

begin
  writeln('Введите положительное число: ');
  read(m);
  write('Цифры числа в обратном порядке: ');
  var s := 0;
  while m>0 do
  begin
    var digit := m mod 10;
    write(digit,' ');
    s += digit;
    m := m div 10;
  end;
  writeln;
  writeln('Сумма цифр = ',s);
end.