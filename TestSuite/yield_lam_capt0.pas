// #2085
function f1: sequence of integer;
begin
  var b: byte := 5;
  yield Arr(3).Select(b->b).First;
end;

begin
  Assert(f1.First = 3);
end.