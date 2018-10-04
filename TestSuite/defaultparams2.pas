function f(x: byte := default(byte)) := x;

begin
  assert(f = 0);
end.