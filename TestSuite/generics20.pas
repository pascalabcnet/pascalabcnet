function Deconstruct<K, V>(d: Dictionary<K, V>): (K, V);
begin
  var kk := d.First.Key;
  var vv := d.First.Value;
  Result := (kk, vv);
end;

begin
  var d := new Dictionary<integer,integer>;
  d[1] := 2;
  var t := Deconstruct(d);
  assert(t.Item1 = 1);
  assert(t.Item2 = 2);
end.