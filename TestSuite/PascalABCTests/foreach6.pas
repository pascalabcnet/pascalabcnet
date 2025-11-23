function test<T>(a: T): sequence of T;
begin
  var lst: List<List<T>> := new List<List<T>>;
  lst.Add(new List<T>(Arr(a, a)));
  foreach var x in lst do
  begin
    yield x[0];
  end;
end;
begin
  assert(test(2).ToArray[0] = 2);
end.