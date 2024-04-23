var i: integer;

procedure p0(p: byte->());
begin
  p(1);
  Inc(i);
end;

type
  t1 = class
    static procedure p1<T>(o: T) := exit;
    procedure p2<T>(o: T) := exit;
  end;
  
begin
  // OK
  p0(t1.p1&<byte>);
  p0(t1.Create.p2&<byte>);
  // А эти 2 падают
  p0(t1.p1);
  p0(t1.Create.p2);
  assert(i = 4);
end.