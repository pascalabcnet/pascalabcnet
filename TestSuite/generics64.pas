type
  // не обязательно auto класс, я добавил только ради конструктора
  t0 = auto class
    f: byte;
  end;
  
procedure p1<T>(o: T); where T: t0;
begin
  assert(o.f.ToString.Println = '123');
  assert(o.f.ToString = '123');
  assert(o.f = 123);
  // Обязательно захватить o
  var p := procedure->(o := o);
end;

begin
  p1(new t0(123));
end.