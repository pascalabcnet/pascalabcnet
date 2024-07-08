procedure test(params a: array of System.Nullable<integer>);
begin
  assert(a[0] = nil);
  assert(a[1] = 1);
end;
begin  
  test(nil, 1);
end.