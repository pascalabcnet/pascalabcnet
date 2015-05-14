function IsPrime(x: integer): boolean;
begin
  var sqx := Round(Sqrt(x));
  var i := 2;
  while (i <= sqx) and (x mod i <> 0) do
    i += 1;
  Result := i > sqx; 
end;

var n := 3000000;

begin
  writeln(Range(2,n).Where(IsPrime).Count);
  writeln(Milliseconds);
  writeln(Range(2,n).AsParallel.Where(IsPrime).Count);
  writeln(MillisecondsDelta);
end.

