var i: integer;
type
  t1 = class
    // Обязательно НЕ виртуальный метод
    procedure p1(x: integer);
    begin
      i := x;
    end;
  end;
  
procedure p0<T>(a: T);
where T: t1;
begin
  a.p1(1);
end;

begin
  p0(new t1);
  assert(i = 1);
end.