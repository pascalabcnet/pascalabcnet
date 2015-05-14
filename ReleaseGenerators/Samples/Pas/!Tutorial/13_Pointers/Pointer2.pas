// Ѕестиповые указатели
// »м можно присваивать любой адрес, но дл€ использовани€ необходимо 
// €вное приведение к типизированному указателю
var
  i: integer := 5;
  r: real := 3.14;
  p: pointer;
  
type 
  pinteger = ^integer;
  preal = ^real;
  
begin
  writeln('»сходные значени€ переменных i и r: ');
  writeln(i,' ',r);
  p := @i; 
  pinteger(p)^ := 8;
  p := @r;
  preal(p)^ += 1;
  writeln('Ќовые значени€ переменных i и r: ');
  writeln(i,' ',r);
end.  