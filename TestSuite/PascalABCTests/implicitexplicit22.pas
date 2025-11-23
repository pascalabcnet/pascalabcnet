var i: integer;

type
  t0<T> = class
    static function operator implicit(o: T): t0<T> := nil;
  end;

procedure p1<T>(o: T);
begin
  i := 1;
end;

procedure p1<T>(o: t0<T>);
begin
  i := 2;
end;

begin
  p1(5);
  assert(i = 1);
  p1(new t0<byte>);
  assert(i = 2);
  p1&<integer>(5);
  assert(i = 1);
end.