// Стандартные функции UpperCase LowerCase
var c: char;

begin
  c := 'д';
  writelnFormat('Символ {0} в верхнем регистре: {1}',c,UpperCase(c));
  c := 'F';
  writelnFormat('Символ {0} в нижнем регистре: {1}',c,LowerCase(c));
end.