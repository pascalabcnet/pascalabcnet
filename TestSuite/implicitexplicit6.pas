type
  t0<T> = class
    static function operator implicit(o: T): t0<T> := nil;
  end;
  
  t1 = class(t0<byte>) end;
  
procedure p1<T>(q: t0<T>);
begin
  assert(q <> nil);
end;

begin
  var prog_Q: t1 := new t1;
  p1(prog_Q);
end.