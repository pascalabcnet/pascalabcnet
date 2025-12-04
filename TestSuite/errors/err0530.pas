//!Данное имя не может быть захвачено лямбда-выражением
begin
  var a: array[0..0] of byte := (0);
  var p := procedure->(a := a);
end.