var i: integer;
procedure p := Inc(i);

begin
  var q := 3*p;
  var q1 := p*3;
  q;
  assert(i=3);
  q1;
  assert(i=6);
end.