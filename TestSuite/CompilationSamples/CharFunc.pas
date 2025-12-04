// Стандартные функции Ord, Chr, OrdUnicode, ChrUnicode
var 
  c: char;
  i: integer;

begin
  writeln('sizeof(char) = ',sizeof(char));
  writeln;
  c := 'Ж';
  i := Ord(c);
  writelnFormat('Код символа {0} в кодировке Windows равен {1}',c,i);
  c := Chr(i);
  writelnFormat('Символ с кодом {0} в кодировке Windows - это {1}',i,c);
  writeln;
  i := OrdUnicode(c);
  writelnFormat('Код символа {0} в кодировке Unicode равен {1}',c,i);
  c := ChrUnicode(i);
  writelnFormat('Символ с кодом {0} в кодировке Unicode - это {1}',i,c);
end.