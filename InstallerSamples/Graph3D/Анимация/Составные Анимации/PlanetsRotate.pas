uses Graph3D;

begin
  Window.Title := 'Вращение планет';
  View3D.ShowCoordinateSystem := False;
  View3D.ShowGridLines := False;
  View3D.ShowViewCube := False;
  View3D.BackgroundColor := Colors.Black;

  var s := Sphere(2,0,0,0.5,Colors.Red);
  var ss := Sphere(0,0,0,1,Colors.Blue);
  Sphere(0,0,0,2,DiffuseMaterial(Colors.Yellow)+SpecularMaterial(32));
  var g := Group(s,ss);
  g.MoveOn(-7,0,0);
  var anim := s.AnimRotateAt(OrtZ,360*100,P3D(-2,0,0),2*100)
    * g.AnimRotateAt(OrtZ,360*100,P3D(7,0,0),10*100)
  ;
   
  anim.Begin;
end.