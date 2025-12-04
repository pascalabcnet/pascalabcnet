function f<T>(source: sequence of T): T;
begin
  var sourceIterator := source.GetEnumerator();
  sourceIterator.MoveNext();
  var min: T := sourceIterator.Current;
  Result := min;
end;

begin
assert(f(Arr(2,3,4)) = 2);
end.