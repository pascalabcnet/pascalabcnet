type
  t1 = class
    o: integer;
  end;
  
begin
  var sq: sequence of t1 := Arr(new t1);
  var i: integer := sq.Max(a->a.o); //Ошибка : Нельзя преобразовать тип real к integer
end.