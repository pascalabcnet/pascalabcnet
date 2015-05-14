function Test<T>(a, b: T): integer; where T:System.IComparable<T>;
var y: T;
begin
  var x: T;
  x.CompareTo(a);
  y.CompareTo(a);
  Result := a.CompareTo(b);
end;

begin
  assert(Test(4,3)=1);
  assert(Test(2,5)=-1);
end.