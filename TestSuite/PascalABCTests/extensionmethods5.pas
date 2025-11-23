uses System, Arrays;

function &Array.MySum<T>(): T;
begin
  Result := default(T);
end;

function &Array.MySum2: integer := 5;

begin
  var a: array of integer := (1,2,3);
  assert(a.MySum&<integer>() =0);
  assert(a.MySum2 = 5);
  var sum := 0;
  sum := a.Select(x->x*x).ToArray.Sum;
  assert(sum = 14);
  a.Select(x->x*x).ToArray.Writeln;
  
end.