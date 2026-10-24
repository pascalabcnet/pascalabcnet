var calls := 0;
function Next: integer;
begin
  calls += 1;
  Result := calls;
end;

begin
  var ok1 := KV(1, (2,3));
  var ok2 := Pair(1, (2,3));
  var p := 1 to (2,3);
  assert(p.Key = 1);
  assert(p.Value.Item1 = 2);
  assert(p.Value.Item2 = 3);
  assert(p.GetType = ok1.GetType);
  assert(p.GetType = ok2.GetType);

  var k := (1,2) to 3;
  assert(k.Key.Item1 = 1);
  assert(k.Key.Item2 = 2);
  assert(k.Value = 3);
  var both := (1,2) to (3,4);
  assert(both.Key.Item2 = 2);
  assert(both.Value.Item1 = 3);

  var nested := 'outer' to (1 to (2,3));
  assert(nested.Value.Key = 1);
  assert(nested.Value.Value.Item2 = 3);
  var t := 1 to ((2,3),4);
  assert(t.Value.Item1.Item2 = 3);
  assert(t.Value.Item2 = 4);
  var grouped := (1 to (2,3));
  assert(grouped.Value.Item2 = 3);
  var scalar := 1 to 2;
  assert((scalar.Key = 1) and (scalar.Value = 2));
  var ordered := Next to (Next, Next);
  assert(ordered.Key = 1);
  assert(ordered.Value.Item1 = 2);
  assert(ordered.Value.Item2 = 3);
  assert(calls = 3);
end.
