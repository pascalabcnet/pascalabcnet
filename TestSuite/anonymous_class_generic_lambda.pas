function MyPairwise<T>(Self: sequence of T); extensionmethod
  := Self.Select(x -> new class(x, f := x));

function ProjectArray<T>(items: array of T) := new class(values := items, count := items.Length);

procedure CheckProjection(projection: integer -> integer);
begin
  assert(false);
end;

procedure CheckProjection(projection: string -> integer);
begin
  assert(projection('abc') = 3);
end;

begin
  var ints := Arr(1, 2).MyPairwise.ToArray;
  assert((ints[0].x = 1) and (ints[1].f = 2));
  var strings := Arr('one', 'two').MyPairwise.ToArray;
  assert((strings[0].x = 'one') and (strings[1].f = 'two'));
  var projected := ProjectArray(Arr(10, 20));
  assert((projected.values[1] = 20) and (projected.count = 2));
  CheckProjection(x -> (new class(value := x)).value.Length);
end.
