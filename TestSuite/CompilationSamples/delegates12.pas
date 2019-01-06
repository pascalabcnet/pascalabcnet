function f(x: real) := x*2;

function g(x: real) := x+1;

begin
  var fg: real -> real := f*g;
  assert(abs(fg(1)-4) < 0.00000001);
end.