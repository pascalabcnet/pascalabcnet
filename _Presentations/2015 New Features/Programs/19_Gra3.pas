uses GraphABC;

procedure MouseMove(x,y,mb: integer);
begin
  if mb=1 then
    Circle(x,y,Random(5,25))
end;

begin
  OnMouseMove := MouseMove;
end.