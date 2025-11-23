// Стандартные функции OrdUnicode ChrUnicode
var 
  i: integer;
  c: char;

begin
  c := 'Д';
  i := OrdUnicode(c);
  writelnFormat('Код символа {0} в кодировке Unicode равен {1}',c,i);
  i := 1046;
  c := ChrUnicode(i);
  writelnFormat('Символ с кодом {0} в кодировке Unicode - это {1}',i,c);
end.