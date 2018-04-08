type CharSet = set of char;
function Test: sequence of CharSet;
begin
  var set1 := ['a','b'];
  yield set1;
end;

begin
  assert(test().First = ['a','b']);
end.