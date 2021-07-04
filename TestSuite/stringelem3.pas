procedure p1(var ch: char) :=
ch := '_';

begin
  var s := '123';
  var s1 := s;
  var s2 := '123';
  p1(s1[2]);
  Assert(s<>s1);
  Assert(s2<>s1);
  assert(s1='1_3');
end.