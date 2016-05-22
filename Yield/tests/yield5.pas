var a: real := 555.0;

function Gen(n: integer): sequence of real;
var j,k: real;
begin
  j := 777.0;
  yield n;
  yield j;
  yield a;
end;

begin
  foreach var x in Gen(5) do
    Print(x);
end.