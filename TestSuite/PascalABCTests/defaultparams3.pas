function f1(x: byte := 1) := x;

begin
  var s := $'{f1}';
  assert(s = '1');
end.