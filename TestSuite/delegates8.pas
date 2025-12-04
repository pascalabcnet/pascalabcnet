var x: integer;
procedure pp;
begin
  x := 1;
end;

type proc = procedure;
begin
  var p: procedure;
  var o: object := p;
  o := pp;
  p := proc(o);
  p;
  assert(x = 1);
end.