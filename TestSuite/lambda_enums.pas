procedure test;
begin
  var o: (a, b) := a;
  assert(o = a);
  var p: ()->() := ()->exit();
end;

begin
  var o: (a, b) := a;
  assert(o = a);
  var p: ()->() := ()->begin o := b end;
  p;
  assert(o = b);
  test;
end.