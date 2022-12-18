//!Нельзя преобразовать тип Func<integer> к integer
procedure p1<T>(a: array of ()->T; v: T) :=
a[0] := ()->v;
begin
  var a := new Func0<integer>[1];
  p1(a, 5);

  foreach var x: integer in a do
    Writeln(x);
end.
