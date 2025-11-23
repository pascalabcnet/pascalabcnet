begin
  var s1 := ['a'];
  var s := SetOf('a','b','c')-s1;
  assert('b' in s);
end.