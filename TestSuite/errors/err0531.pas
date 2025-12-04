//!Данное имя не может быть захвачено лямбда-выражением
procedure p;
var a: array[0..0] of byte := (0);
begin
   var p := procedure->(a := a);
end;
begin
  
 
end.