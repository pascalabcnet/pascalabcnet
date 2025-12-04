var a: integer;
    b: integer;
    
procedure p;
begin
  a := 1;
end;

procedure q;
begin
  b := 1;
end;

begin
  var t := (p, q);
  t[0];
  t[1];
  assert(a = 1);
  assert(b = 1);
end.