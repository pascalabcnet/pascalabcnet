function Gen(a,b: integer): sequence of integer;
var j,k: real;
begin
  var i := 1;
  j := 2;
  while i<5 do
  begin
    yield i*i;
    i += 1;
  end;
end;

begin
  foreach var x in Gen(1,2) do
    Print(x);
end.