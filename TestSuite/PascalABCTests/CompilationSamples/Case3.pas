// Оператор выбора
var c: char;

begin
  writeln('Введите символ: ');
  readln(c);
  case c of
    '0'..'9': writeln('Это цифра');
    'a'..'z','A'..'Z': writeln('Это английская буква');
    'а'..'я','А'..'Я': writeln('Это русская буква');
  end;
end.