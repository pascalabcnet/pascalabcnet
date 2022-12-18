//!Нельзя преобразовать тип Func<integer> к integer
procedure p1<T>(a: array of ()->T; v: T) :=
a[0] := ()->v;
begin
  var a := new Func0<integer>[1];
  p1(a, 5);

  //Ошибка: Нельзя преобразовать тип integer к Func<integer>
  foreach var x: integer in a do
    Writeln(x);
  
  var p := procedure->exit();
end.
