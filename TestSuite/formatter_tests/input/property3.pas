type
  t1 = class
  x: integer;
  property p1: real read x+1 write begin x:=1; end;
  property p2:real read x+3;
  end;

begin end.
