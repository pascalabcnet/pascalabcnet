// Процедуры Polyline, Polygon, Curve, ClosedCurve
// Перенос начала системы координат
uses GraphABC;

var a: array of Point := (new Point(0,0), new Point(50,170), new Point(100,100), new Point(150,170), new Point(200,0));

begin
  Window.Title := 'Рисование по массиву точек';
  Brush.Color := Color.Beige;
  Coordinate.SetOrigin(60,30);
  Polyline(a);
  Coordinate.OriginX := 360;
  Polygon(a);
  Coordinate.Origin := new Point(60,250);
  Curve(a);
  Coordinate.OriginX := 360;
  ClosedCurve(a);
end.  
