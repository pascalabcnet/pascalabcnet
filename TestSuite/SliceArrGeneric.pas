procedure insert<T>(var a: array of T);
begin
  a := a[0:2];
end;

begin
  var a := Arr(1,2,3,4,5);
  Insert(a);
  Assert(a.SequenceEqual(Arr(1,2)));
end.