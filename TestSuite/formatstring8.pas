function f1(b: byte) := 0;

begin
  var s := $'{f1} {0}';
  assert(s = 'formatstring8.$delegate0 0');
end.