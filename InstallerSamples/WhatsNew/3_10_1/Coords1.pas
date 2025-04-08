uses Coords;

function RandomPoint: Point := Pnt(Random(-13,13),Random(-10,10));

begin
  DrawPoints(ArrGen(10,i -> RandomPoint),PointRadius := 4);
  DrawPoints(ArrGen(10,i -> RandomPoint),PointRadius := 6);
  DrawPoint(2,3,Colors.Red);
  DrawCircle(1,1,1,Colors.LightBlue);
  DrawRectangle(3,2,2,1);
  DrawText(3,2,'Hello');
  DrawTextUnscaled(0,0,'Текст не масштабируется', Size := 20, Color := Colors.Red);
  DrawText(-4,7,'Текст масштабируется', FontName := 'Courier New', Size := 34);
end.