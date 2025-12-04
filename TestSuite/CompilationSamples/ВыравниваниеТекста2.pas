uses GraphWPF;

begin
  Window.Title := 'Выравнивание шрифта';
  Font.Size := 20;
  
  var sc := 40;
  SetMathematicCoords;
  
  var (x,y) := (-5,-2);
  var (w,h) := (10,4);
  DrawRectangle(x,y,w,h);
  DrawText(x,y,w,h,'LeftTop',Alignment.LeftTop);
  DrawText(x,y,w,h,'LeftCenter',Alignment.LeftCenter);
  DrawText(x,y,w,h,'LeftBottom',Alignment.LeftBottom);
  DrawText(x,y,w,h,'CenterTop',Alignment.CenterTop);
  DrawText(x,y,w,h,'Center');
  DrawText(x,y,w,h,'CenterBottom',Alignment.CenterBottom);
  DrawText(x,y,w,h,'RightTop',Alignment.RightTop);
  DrawText(x,y,w,h,'RightCenter',Alignment.RightCenter);
  DrawText(x,y,w,h,'RightBottom',Alignment.RightBottom);
  // Выравнивание относительно точки
  TextOut(-5,5,'PointRightBottom',Alignment.RightBottom);
  TextOut(-5,5,'PointRightTop',Alignment.RightTop);
  TextOut(-5,5,'PointLeftTop',Alignment.LeftTop);
  TextOut(-5,5,'PointLeftBottom',Alignment.LeftBottom);
  FillCircle(-5,5,0.1,Colors.Red);
  TextOut(5,5,'PointCenterTop',Alignment.CenterTop);
  TextOut(5,5,'PointCenterBottom',Alignment.CenterBottom);
  FillCircle(5,5,0.1,Colors.Red);
  TextOut(5,-5,'PointLeftCenter',Alignment.LeftCenter);
  TextOut(5,-5,'PointRightCenter',Alignment.RightCenter);
  FillCircle(5,-5,0.1,Colors.Red);
end.
