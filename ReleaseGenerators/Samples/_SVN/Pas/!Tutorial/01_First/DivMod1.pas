// Операции div и mod
var a: integer;

begin 
  write('Введите a: ');
  readln(a);
  writeln('Последняя цифра числа: ',a mod 10);
  writeln('Число без последней цифры: ',a div 10);
  writeln('Если число a четно, то 0: ',a mod 2);
end.