uses Coords;

begin
  Globals.PointRadius := 2.3;
  Globals.LineWidth := 1.0;
  var p1 := Pnt(2,3);
  var p2 := Pnt(-4,7.3);
  var p3 := Pnt(-5,-3);
  DrawPoint(p1); 
  DrawPoint(p2);
  DrawPoint(p3);
  DrawText(p1,'A', Align := Alignment.LeftBottom, Size := 18);
  DrawText(p2,'B', Align := Alignment.LeftBottom, Size := 18);
  DrawText(p3,'C', Align := Alignment.RightTop, Size := 18);
  DrawLine(p1,p2);
  DrawLine(p1,p3);
  DrawLine(p2,p3);
  DrawText(Middle(p1,p2), p1.Distance(p2).ToString(2), Align := Alignment.Center, BackgroundColor := Colors.LightBlue);
  DrawText(Middle(p2,p3), p2.Distance(p3).ToString(2), Align := Alignment.Center,
    BackgroundColor := Colors.White, BorderWidth := 0.5);
  DrawText(Middle(p1,p3), p1.Distance(p3).ToString(2), Align := Alignment.Center, BackgroundColor := Colors.LightGreen);
end.