//ä
function Test<T>(a, b: T): integer; where T:System.IComparable<T>;
begin
  var x: T;
  x.CompareTo{@function IComparable<>.CompareTo(other: T): integer; virtual;@}(a);
end;

procedure Test2<T>(a, b: T); where T:System.IComparable<real>;
begin
  var x: T;
  x.CompareTo{@function IComparable<>.CompareTo(other: real): integer; virtual;@}(a);
end;

begin
end.