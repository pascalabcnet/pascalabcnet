// Стандартные функции Ord Chr
var 
  i: integer;
  c: char;

begin
  c := 'z';
  i := Ord(c);
  writelnFormat('Код символа {0} в кодировке Windows равен {1}',c,i);
  i := 193;
  c := Chr(i);
  writelnFormat('Символ с кодом {0} в кодировке Windows - это {1}',i,c);
end.