function SquareElems(a: array of integer): array of integer;
begin
  SetLength(Result,a.Length);
  for var i:=0 to a.Length-1 do
    Result[i] := sqr(a[i]);
end;

var a: array of integer;

begin
  a := Seq(2,5,7,10);
  a := SquareElems(a);
  foreach var x in a do
    Print(x);
end.