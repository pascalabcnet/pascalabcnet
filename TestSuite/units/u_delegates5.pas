unit u_delegates5;
interface

implementation

type
  d<T> = T->();

procedure Test;
begin
  var i := 0;
  var f: d<integer> := x -> begin i := x end;
  f(2);
  assert(i = 2);
end;      
begin
  Test;
end.