type
  t0 = auto class
    f: byte;
  end;
  
procedure p1<T>(oo: T); where T: t0;
var o2: T;
begin
  var o := oo;
  assert(o.f = 123);
  o2 := oo;
  assert(o2.f = 123);
end;

begin
  p1(new t0(123));
end.