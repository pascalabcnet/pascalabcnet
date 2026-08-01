// NET10-TESTFIX regression: PersistedAssemblyBuilder must emit foreach over T[].
procedure CheckRows<T>(a: array of array of T);
begin
  var count := 0;
  foreach var row in a do
    count += row.Length;
  assert(count = 4);
end;

begin
  var a := Arr(Arr(1, 2), Arr(3, 4));
  CheckRows(a);
end.
