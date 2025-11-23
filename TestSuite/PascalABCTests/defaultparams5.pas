var obj: object;
procedure p1<T>(o: T := default(T));
begin
  obj := o;
end;
begin
  p1&<BigInteger>;
  assert(BigInteger(obj) = default(BigInteger));
end.
