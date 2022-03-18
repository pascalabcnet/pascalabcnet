type
  t1<T> = class
    static function operator implicit(o: T): t1<T>; begin Assert(2=1); Result := nil; end;
  end;
  t2<T> = class(t1<T>) end;

var i: integer;
procedure p<T>(r: t1<T>);
begin
  r := new t2<T>;
  i := 1;
end;

begin 
  p(new t1<integer>);
  assert(i = 1);
end.