// Строки string, string[n], shortstring
var 
  s: string; // память, занимаемая s, зависит от ее длины
  s10: string[10]; // память под ss фиксирована
  ss: shortstring := s;
  f: file of string[10];
  // f: file of string; - ошибка

begin
  s := '12345678901234567890';
  s10 := s; // обрезание
  writeln(s10);
  s += s; s += s;
  s += s; s += s;
  writeln(s);
  writeln('Длина строки = ',s.Length);
end.