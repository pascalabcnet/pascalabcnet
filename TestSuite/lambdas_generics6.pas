uses System, System.Collections.Generic;

procedure p<X, Y> (b: Func<IEnumerable<Y>, X>; c: Y);
begin
  assert(b.GetType().ToString() = 'System.Func`2[System.Collections.Generic.IEnumerable`1[System.Int32],System.Collections.Generic.IEnumerable`1[System.Int32]]');
end;

begin
  p(x -> x, 5);
end.