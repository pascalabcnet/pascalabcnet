function IsPrime(x: integer): boolean;
begin
  Result := Range(2,Round(sqrt(x))).All(i->x mod i <> 0)
end;

begin
  Range(2,1000).Where(IsPrime).Print;
end.