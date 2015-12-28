var ss1,ss2: set of string;
  
begin
  ss1 := ['планшет','смартфон','ноутбук'];
  ss2 := ['смартфон','компьютер','планшет'];
  writeln('Объединение: ',ss1+ss2);
  writeln('Пересечение: ',ss1*ss2);
  writeln('Разность:    ',ss1-ss2);
end.