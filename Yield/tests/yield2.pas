function Gen(n: integer): sequence of integer;
var j,k: real;
begin
  var i := 1;
  j := 5;
  while i<j do
  begin
    yield i*i;
    i += 1;
  end;
end;

begin
  foreach var x in Gen(5) do
    Print(x);
end.