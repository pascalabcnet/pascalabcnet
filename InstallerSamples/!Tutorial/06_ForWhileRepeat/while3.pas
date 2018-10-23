// Цикл while. Сумма цифр положительного числа

begin
  var m := ReadlnInteger('Введите положительное число: ');

  Print('Цифры числа в обратном порядке:');
  var s := 0;
  while m>0 do
  begin
    var digit := m mod 10;
    Print(digit);
    s += digit;
    m := m div 10;
  end;
  
  Println;
  Println('Сумма цифр =',s);
end.