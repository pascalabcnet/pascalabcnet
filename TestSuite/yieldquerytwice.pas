function f<T>(a: sequence of T): sequence of T;
begin
  foreach var x in a do
    yield x;
end;

begin
  var q := f(Range(1,4));

  var sq := Seq(1,2,3,4);  
  Assert(q.Println.SequenceEqual(sq));
  Assert(q.Println.SequenceEqual(sq));
end.