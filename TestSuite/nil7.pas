type
  t0 = class end;
  
  t1 = class
    static function operator implicit(p: t0): t1 := new t1;
  end;

var obj: t1;
  
procedure p1(o: t1);
begin
  obj := o;
end;

begin
  p1(t0(nil));
  assert(obj <> nil);
end.