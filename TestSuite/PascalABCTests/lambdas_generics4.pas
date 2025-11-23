uses System, System.Collections.Generic;

procedure p<X, Y> (a: X; b: Action<IEnumerable<Dictionary<X, Y>>>; c: Y);
begin
  assert(b.GetType().ToString() = 'System.Action`1[System.Collections.Generic.IEnumerable`1[System.Collections.Generic.Dictionary`2[System.Int32,System.Int32]]]');
end;

begin
  p(4, procedure(x) -> begin end, 5);
end.