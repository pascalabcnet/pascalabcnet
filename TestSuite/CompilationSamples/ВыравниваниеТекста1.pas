uses GraphWPF;

begin
  Window.Title := 'Выравнивание шрифта';
  Font.Size := 20;
  var (x,y) := (200,200);
  var (w,h) := (400,200);
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
  TextOut(150,100,'PointRightBottom',Alignment.RightBottom);
  TextOut(150,100,'PointRightTop',Alignment.RightTop);
  TextOut(150,100,'PointLeftTop',Alignment.LeftTop);
  TextOut(150,100,'PointLeftBottom',Alignment.LeftBottom);
  FillCircle(150,100,5,Colors.Red);
  TextOut(600,100,'PointCenterTop',Alignment.CenterTop);
  TextOut(600,100,'PointCenterBottom',Alignment.CenterBottom);
  FillCircle(600,100,5,Colors.Red);
  TextOut(400,500,'PointLeftCenter',Alignment.LeftCenter);
  TextOut(400,500,'PointRightCenter',Alignment.RightCenter);
  FillCircle(400,500,5,Colors.Red);
end.
