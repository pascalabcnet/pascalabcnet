type
  // Не обязательно auto класс, я добавил только ради конструктора
  t0 = auto class
    f: byte;
  end;
  
procedure p1<T>(o: T); where T: t0;
begin
  assert(o.f = 123);
  
  var p := procedure->
  begin
    var o2 := o;
  end;
  
end;

begin
  p1(new t0(123));
end.