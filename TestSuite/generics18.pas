function f1 := 1;
function f2 := 2;
begin
  var a := Arr&<function: integer>(f1, f2);
  var a2 := Arr&<function: integer>(f1, f2);
  assert(a[0] = 1);
  assert(a[1]() = 2);
  assert(a2[0] = 1);
  assert(a2[1]() = 2);
end.