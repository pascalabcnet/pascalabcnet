// Строки. Строка может иметь произвольную длину
begin
  var s := 'abcdefghijklmnopqrstuvwxyz';
  s := s + UpperCase(s);
  s += s;
  s += s;
  s += s;
  Println('Длина строки =', s.Length);
  Println('Строка:', s);
end.