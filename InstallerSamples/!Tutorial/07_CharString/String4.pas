// Строки фиксированной длины (устарели). Длина не может быть больше 255

begin
  var s: string[9];
  var s1: shortstring; // shortstring = string[255]
  var slong := 'PascalABC.NET';
  Println('Строка произвольной длины:', slong);
  s := slong;
  Println('Строка фиксированной длины обрезается:', s);
end.