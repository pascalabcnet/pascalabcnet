var s: string;
type
  // не обязательно auto класс, я добавил только ради конструктора
  t0 = auto class
    f: byte;
  end;
  
procedure p1<T>(o: T); where T: t0;
begin
  s := o.f.ToString; // Выводит 0 вместо 123
  
  // Обязательно захватить параметр и локальную переменную лямбдой
  var x := 0;
  var p := procedure->
  begin
    o := o;
    x := x;
  end;
  
end;

begin
  p1(new t0(byte(123)));
  assert(s = '123');
end.