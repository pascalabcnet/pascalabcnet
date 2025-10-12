// Все типы кроме указателей - производные от Object
var 
  i: integer;
  r: real;
  o: object;

begin
  o := i;
  Println(o.GetType,TypeName(o));
  o := r;
  Println(o.GetType,TypeName(o));
  if o.GetType = typeof(real) then
    Println('В переменной o - вещественный тип');
end.