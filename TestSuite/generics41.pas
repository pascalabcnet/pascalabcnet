function p1<T>(a, b: T): integer;
  where T: System.IComparable<T>;
begin
  Result := a.CompareTo(b);
end;

function p2<T>(a, b: T): integer;
  where T: record, System.IComparable<T>;
begin
  Result := a.CompareTo(b);
end;

begin
  assert(p1(2,1) = 1);
  assert(p2(2,1) = 1);
end.