// Строки. Строка может иметь произвольную длину
var s: string;
    
begin
  s := 'abcdefghijklmnopqrstuvwxyz';
  s := s + Uppercase(s);
  s += s; 
  s += s; 
  s += s;
  writeln('Длина строки = ',s.Length);
  writeln('Cтрока: ',s);
end.