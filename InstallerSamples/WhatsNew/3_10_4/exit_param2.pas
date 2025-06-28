function IsPrime(n: integer): boolean;
begin
  if n < 2 then exit(False);
  for var i:=2 to n.Sqrt.Trunc do
    if n.Divs(i) then
      exit(False);
  exit(True);
end;

begin
  IsPrime(17).Print;
  IsPrime(1).Print;
end.
