// ÷икл while. —умма цифр положительного числа
var m: integer;

begin
  writeln('¬ведите положительное число: ');
  read(m);
  write('÷ифры числа в обратном пор¤дке: ');
  var s := 0;
  while m>0 do
  begin
    var digit := m mod 10;
    write(digit,' ');
    s += digit;
    m := m div 10;
  end;
  writeln;
  writeln('—умма цифр = ',s);
end.