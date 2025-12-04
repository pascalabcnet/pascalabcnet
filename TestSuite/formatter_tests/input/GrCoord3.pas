uses GraphABC;

begin
  Coordinate.Origin := Window.Center;
  Coordinate.SetMathematic;
  while True do
  begin
    LockDrawing;
    ClearWindow;
    Ellipse(-120,-70,120,70);
    Line(0,0,200,0);
    Line(0,0,0,200);
    Redraw;
    Coordinate.Angle := Coordinate.Angle + 1;
    Sleep(100);
  end;
end.
