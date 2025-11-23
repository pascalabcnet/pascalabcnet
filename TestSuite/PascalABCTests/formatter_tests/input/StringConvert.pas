var 
  s: string;
  i: integer;
  r: real;

begin
  // Преобразование строки в число
  s := '123,3443'; // Дробная часть отделяется запятой - настройки Windows
  if not real.TryParse(s, r) then
    Writeln('Строка s не является строковым представлением вещественного числа')
  else Writeln(r);
     
  if not integer.TryParse(s, i) then
    Writeln('Строка s не является строковым представлением целого числа');
     
  // Преобразование числа в строку
  i := 10;
  s := i.ToString;
  Writeln(s);
  
  s := '';
  for i:=1 to 9 do
    s += i.ToString;
  Writeln(s);
end.