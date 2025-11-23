begin
  var a := new PInteger[2];
  var i := 1;
  a[0] := @i;
  SetLength(a, 1);
  assert(a[0] = @i);
  SetLength(a, 3);
  assert(a[0] = @i);
end.