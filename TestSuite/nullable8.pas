type
  r1 = record end;
  
begin
  var n1, n2: r1?;
  assert(n1 = n2);
  assert(n1 = nil);
  assert(not (n1<>n2));
  n1 := new r1;
  assert(n1 = n1);
  n2 := new r1;
  assert(n1 = n2);
  n2 := nil;
  assert(n1 <> n2);
end.