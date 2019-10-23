type
  t1 = class end;

procedure p2<T>(a: T); where T:class;
begin
  assert(a = nil);
end;

procedure p1<T>; where T: t1;
begin
  var a: T;
  var b: t1;
  a := b as T;
  p2&<t1>(b);
  p2&<T>(a);
  assert(a = nil);
end;

begin 
  p1&<t1>;
end.