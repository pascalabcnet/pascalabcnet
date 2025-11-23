function IsPrime(x: integer): boolean;
begin
  Result := Range(2,Round(sqrt(x))).All(i->x mod i <> 0)
end;

begin
  assert(IsPrime(4) = false);
  assert(IsPrime(1) = true);
  assert(IsPrime(7) = true);
  assert(IsPrime(13) = true);
  assert(IsPrime(6) = false);
end.