function f1(b: byte) := 0;

begin
  var s := $'{f1} {0}';
  assert(s = 'function f1(b: byte): integer 0');
end.