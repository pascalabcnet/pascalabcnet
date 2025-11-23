function f1(b: byte) := b;
function f2 := 2;
type fnc = function(b: byte): byte;
begin
  var o1: object := f1;
  var f: fnc := fnc(o1);
  assert(f(2) = 2);
  var o2: object := f2;
  assert(integer(o2) = 2);
  o1 := f1;
  f := fnc(o1);
  assert(f(2) = 2);
end.