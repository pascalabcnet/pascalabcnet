uses System;

begin
  var t1 := new ValueTuple<integer>(1);
  var t2 := new ValueTuple<integer>(1);
  assert(t1 = t2);
end.