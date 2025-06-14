function IsPrime(n: integer): boolean;
begin
  for var i:=2 to n.Sqrt.Round do
    if n.Divs(i) then
      exit(False);
  exit(True);
end;

begin
  IsPrime(17).Print;
end.