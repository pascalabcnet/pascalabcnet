procedure WriteArray<T>(a: array of T);
begin
  foreach x: T in a do
    writeln(x);
end;

begin
  var a := Arr(2,3,5);
  WriteArray(a);
  var a2 := new integer[2,2]((3,3),(3,3));
  foreach x: integer in a2 do
    assert(x = 3);
  var a3 := new integer[2,2]((3,3),(3,3));
  foreach var x in a3 do
    assert(x=3);
end.