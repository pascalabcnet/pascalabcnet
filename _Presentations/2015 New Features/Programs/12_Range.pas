function IsPrime(n: integer): boolean;
begin
  Result := Range(2,Round(sqrt(n))).All(i->n mod i <> 0)
end;

begin
  Range(2,1000).Where(IsPrime).Print;
end.