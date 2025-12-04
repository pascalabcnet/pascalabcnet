begin
  var o: object;
  var b := true;
  o := b ? 1 : 2;
  assert(integer(o) = 1);
  b := false;
  o := b ? 1 : 2;
  assert(integer(o) = 2);
end.