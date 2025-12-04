begin
  var a := |default(integer?),nil,1|;
  assert(a[1] = nil);
  assert(a[0] = nil);
  assert(a[2] = 1);
end.