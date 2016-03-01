// Строки фиксированной длины. Длина не может быть больше 255
var 
  s: string[9];
  s1: shortstring; // shortstring = string[255]
  slong: string;
    
begin
  slong := 'PascalABC.NET';
  writeln('Cтрока произвольной длины: ',slong);
  s := slong;
  writeln('Cтрока фиксированной длины обрезается: ',s);
end.