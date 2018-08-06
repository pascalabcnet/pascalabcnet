var j: int64;

procedure test(i: int64; proc:procedure);
begin
  j := i;
end;

procedure test(r: real; proc:procedure);
begin
  test(System.Convert.ToInt64(r), proc);
end;
begin
  test(2.3, nil);
  assert(j = 2);
end.