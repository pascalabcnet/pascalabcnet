uses System, System.Collections.Generic;

procedure p<X,Y,Z>(a:IEnumerable<X>; b: Func<Y, IEnumerable<Z>>; c: Func<X, IEnumerable<Y>>);
begin
  writeln(a.GetType());
  writeln(b.GetType());
  writeln(c.GetType());
end;

begin
  var a := new List<string>;
  p(a, x -> x.Split(), x -> x.Split());
end.