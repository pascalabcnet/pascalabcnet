var x := 1;

type
  t1 = static class
    static o := x;
  end;
  
begin
  assert(x = 1);
  x := 5;
  assert(x = 5);
  assert(t1.o = 5);
  assert(x = 5);
end.