begin
  var i: integer?;
  var x := i ?? 0;
  assert(x.Value = 0);
  i := 1;
  x := i ?? 0;
  assert(x.Value = 1);
end.