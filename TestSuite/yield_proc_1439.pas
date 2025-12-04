procedure p0 := Print(1);

function f1: sequence of integer->integer;
begin
  var ff: integer->integer := x->x*x;
  yield ff;
end;

begin 
  var a := f1.First;
  Assert(a(2)=4);
end.