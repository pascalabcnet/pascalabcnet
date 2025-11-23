function f1 := 1;
function f2 := 0;
begin
  var i := true ? f1 : f2;
  assert(i = 1);
end.