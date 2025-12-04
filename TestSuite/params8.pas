function Test<T>(a: T): sequence of T;
begin
  Result := Seq(a);
end;

begin
  assert(Test(2).ToArray()[0]=2);
end.