// #1416
function f1 := 5;

begin
  var o := new class(a := f1);
  Assert(o.a = 5);
end.