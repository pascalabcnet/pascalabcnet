function f: sequence of object;
begin
  yield nil;
end;

begin
  var o: object := nil;
  Assert(f.SequenceEqual(Seq(o)));
end.