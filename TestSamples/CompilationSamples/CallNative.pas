// Вызов функции из обычной dll
function add(a,b: integer): integer; external 'NativeDll.dll' name 'add'; // объявление внешней функции

begin // основная программа
  writeln(add(2,3));
end.
