function operator=<T>(a,b: array of T): boolean; extensionmethod :=
(a.Length=b.Length) and a.SequenceEqual(b);

begin
  var a := Arr(1,2,3);
  var b := Arr(1,2,3);
  Assert(not object.ReferenceEquals(a,b));
  Assert(a = b);
end.