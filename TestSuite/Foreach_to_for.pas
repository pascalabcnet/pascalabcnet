type Person = class

end;

procedure P(a: array of Person);
begin
  var s: Person;
  foreach var x in a do
    s := x;
end;

procedure P<T>(a: array of T);
begin
  var s: T;
  foreach var x in a do
    s := x;
end;

begin
  var n := 5000;
  var a := ArrFill(n,1);
  var s := 0;
  foreach var x in a do
    s += x;
  Assert(s = n);
end.