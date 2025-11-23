function f: procedure := ()->write(1);

function f1: integer->integer := x->x;

begin
  f()();
  Print(f1()(2));
end.