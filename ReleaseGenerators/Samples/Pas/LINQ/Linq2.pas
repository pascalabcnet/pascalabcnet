function IsPrime(x: integer): boolean;
begin
  var sqx := Round(Sqrt(x));
  var i := 2;
  while (i <= sqx) and (x mod i <> 0) do
    i += 1;
  Result := i > sqx; 
end;

begin
  Range(2,1000).Where(IsPrime).Print;
end.

