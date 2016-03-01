procedure wr<T>(a: array of T);
begin
  assert(a[0].ToString()='5');
end;

begin
var a := Arr(1,3,5);
wr(a.Reverse.ToArray);
end.
