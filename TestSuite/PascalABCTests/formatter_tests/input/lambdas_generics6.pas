uses System, System.Collections.Generic;

procedure p<X, Y> (b: Func<IEnumerable<Y>, X>; c: Y);
begin
  writeln(b.GetType());
end;

begin
  p(x -> x, 5);
end.