function Test: sequence of integer->integer;
begin
  yield nil;
end;

begin
  assert(Test.First = nil);
end.