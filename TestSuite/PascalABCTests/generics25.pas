function f1 := 1;
function f2(params a: array of integer) := 2;

begin
  var a := Arr(f1, f2);
  assert(a[0] = 1);
  assert(a[1] = 2);
end.