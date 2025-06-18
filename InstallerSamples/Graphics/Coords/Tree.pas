uses Turtle;

procedure DrawTree(length: real; depth: integer);
begin
  if depth > 0 then
  begin
    forw(length);
    turn(-35);
    DrawTree(length*0.65, depth-1);
    turn(70);
    DrawTree(length*0.65, depth-1);
    turn(-35);
    forw(-length);
  end;
end;

begin
  toPoint(0, -8);
  down;
  
  DrawTree(6, 11);
end.