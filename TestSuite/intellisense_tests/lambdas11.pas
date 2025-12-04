procedure p1(p: procedure(pp: Action0)) := exit;

procedure p2(f: real->real) := exit;

begin
  p1(p->
  begin
    var a: real;
    p2(x->x*x{@parameter x: real;@});
    a{@var a: real;@} := 2;
  end);
end.