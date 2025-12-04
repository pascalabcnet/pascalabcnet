type
  t0 = class
    v := 123;
  end;
  
procedure p1<T>(o: T);
where T: t0, constructor;
begin
  assert(t0(o).v = 123);
  var p := procedure -> o := o;
end;

begin
  p1(new t0);
end.