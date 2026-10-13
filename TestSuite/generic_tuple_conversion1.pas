function Partition1<T>(p: T): (sequence of T, sequence of T);
begin
  var left := new List<T>;
  var right := new List<T>;
  Result := (left, right);
end;

begin
  var parts := Partition1(1);
  assert(parts[0].Count = 0);
  assert(parts[1].Count = 0);
end.
