type
  t1 = static class
    static o := 1;
  end;
  
var x := t1.o;

begin
  assert(x = 1);
  x := 5;
  assert(x = 5);
  assert(t1.o = 1);
  assert(x = 5);
end.