uses Graph3D;

begin
  Window.Title := '3D-примитивы';
  
  //var a := Any(0,0,0,Colors.Red);
  //a.MoveOn(-5,2,0);
  
  var rr := Rectangle3D(-3.5,-2,0,3,2,Colors.MediumPurple);
  rr.Rotate(v3d(0,1,0),30);
  
  var стрелки := CoordinateSystem(2);
  
  var Ar := Arrow(3,0,0,0,0,2,Colors.Tan);

  var p := P3D(5,-5,0);
  var t := TruncatedCone(p,2,0.9,0.7,False,Colors.Red);
  p := p.MoveX(-2);
  TruncatedCone(p,2,0.9,0.7,Colors.BlueViolet);
  p := p.MoveX(-2);
  Cylinder(p,2,0.9,False,Colors.Crimson);
  p := p.MoveX(-2);
  var cc := Cylinder(p,2,0.9,Colors.Chocolate);
  p := p.MoveX(-2);
  var конус := Cone(p,2.5,0.9,Colors.DarkGreen);
  p := p.MoveX(-2);
  var c := Sphere(p.MoveZ(0.7),1,Colors.Crimson);
  Text3D(p.MoveZ(2.3),'—фера',0.7);
  
  p := P3D(5,-2,0);
  var чайник := Teapot(p.MoveZ(0.9),Colors.ForestGreen);
  //чайник.Rotate(V3D(0,1,0),45);
  var bb := BillboardText(p.Move(2,0,0.7),'Billboard'+NewLine + '"„айник"',12);
  p := p.MoveX(-2.75);
  var cb := Cube(p.MoveZ(0.75),1.5,Colors.DodgerBlue);
  cb.Scale(1.2);
  cb.Rotate(V3D(0,0,1),45);
  p := p.MoveX(-2.75);
  var b := Box(p,Sz3D(2,1,1),Colors.PaleGreen);
  b.Rotate(V3D(0,1,0),-30);
  p := P3D(5,1,0);
  var эллипсоид := Ellipsoid(p.MoveZ(0.6),1.3,0.8,0.6,Colors.Violet);
  p := p.MoveX(-7);
  var pp := Tube(p,2,0.7,0.5,Colors.DodgerBlue);
  //View3D.Save('a2.jpg');
  
  {while True do
  begin
    чайник.Rotate(V3D(0,0,1),10.8);
    эллипсоид.Rotate(V3D(0,0,1),-5.8);
    стрелки.Rotate(V3D(0,0,1),1);
    Sleep(50);
  end;}
end.