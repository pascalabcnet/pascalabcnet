// Операции div и mod
var a: integer;

begin 
  Write('Введите a: ');
  Readln(a);
  Writeln('Последняя цифра числа: ',a mod 10);
  Writeln('Число без последней цифры: ',a div 10);
  Writeln('Если число a четно, то 0: ',a mod 2);
end.