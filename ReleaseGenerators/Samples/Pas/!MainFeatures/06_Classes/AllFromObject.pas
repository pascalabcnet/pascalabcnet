// Все типы кроме указателей - производные от Object
var 
  i: integer;
  r: real;
  o: object;

begin
  o := i;
  writeln(o.GetType);
  o := r;
  writeln(o.GetType);
  if o.GetType=typeof(real) then
    writeln('В переменной o - вещественный тип');
end.