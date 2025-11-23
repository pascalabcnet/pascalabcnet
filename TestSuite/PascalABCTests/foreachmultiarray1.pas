type
  T = class 
  end;

var o: T;

procedure p(a: array of array of T);
begin
  foreach var x in a do
    foreach var y in x do
    begin
      o := y;
    end;
end;

begin
  var a: array of array of T;
  SetLength(a, 2);
  SetLength(a[0], 2);
  SetLength(a[1], 2);
  a[1][1] := new T;
  p(a);
  assert(a[1][1] = o);
end.