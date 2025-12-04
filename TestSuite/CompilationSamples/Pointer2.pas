// Бестиповые указатели
// Им можно присваивать любой адрес, но для использования необходимо 
// явное приведение к типизированному указателю
var
  i: integer := 5;
  r: real := 3.14;
  p: pointer;
  
type 
  pinteger = ^integer;
  preal = ^real;
  
begin
  writeln('Исходные значения переменных i и r: ');
  writeln(i,' ',r);
  p := @i; 
  pinteger(p)^ := 8;
  p := @r;
  preal(p)^ += 1;
  writeln('Новые значения переменных i и r: ');
  writeln(i,' ',r);
end.  