uses System, System.Collections.Generic;

procedure p<X, Y> (a: X; b: Action<IEnumerable<Dictionary<X, Y>>>; c: Y);
begin
  writeln(b.GetType());
end;

begin
  p(4, procedure(x)->begin end, 5);
end.