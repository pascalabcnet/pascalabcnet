unit u_defaultparams2;
function f(x: byte := default(byte)) := x;

begin
  assert(f = 0);
end.