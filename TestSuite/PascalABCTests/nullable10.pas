begin
  var t := ( true ? 1 : nil, 2);
  var t2 := ( true ? nil : 1, 2);
  assert(t.Item1 = 1);
  assert(t2.Item1 = nil);
end.