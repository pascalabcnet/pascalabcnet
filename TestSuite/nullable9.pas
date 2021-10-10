
begin
  var n1, n2: System.TimeSpan?;
  assert(n1=n2);
  assert(n1=nil);
  n1 := System.TimeSpan.Zero;
  n2 := System.TimeSpan.Zero;
  assert(n1=n2);
  assert(not (n1<>n2));
  n2 := nil;
  assert(n1<>n2);
  assert(not (n1=n2));
end.