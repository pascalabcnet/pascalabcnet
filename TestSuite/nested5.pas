var i: integer;
type
  t1 = class
    
    static procedure p1;
    
  end;

static procedure t1.p1;
  function f(b: byte) := b;
begin
  f(0);
  i := 1;
end;

begin
  t1.p1;
  assert(i = 1);
end.