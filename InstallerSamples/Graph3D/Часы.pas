uses Graph3D;

begin
  View3D.ShowGridLines := False;
  var Циферблат := Cylinder(0,0,-0.4,0.2,6,Colors.DeepPink);
  var Сек := Arrow(0,0,0,0,-5.5,0,0.2,Colors.Yellow);
  var Мин := Arrow(0,0,0,0,-6.0,0,0.3,Colors.Red);
  
  var a := 0;
  var r := 5.8;
  loop 60 do
  begin
    Sphere(r*cos(a*Pi/180),r*sin(a*Pi/180),-0.2,0.1,Colors.White);
    a += 6;
  end;
  
  while True do
  begin
    Sleep(10);
    Сек.Rotate(v3D(0,0,1),-6);
    Мин.Rotate(v3D(0,0,1),-6/60);
  end;
end.