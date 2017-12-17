uses Graph3D;

begin
  Cylinder(Origin,6,0.3,Colors.Green);
  var ss := Cube(-3,0,0,1.5,Colors.Red);
  var anim := ss.AnimRotateAt(OrtZ,360,P3D(3,0,0),2).Forever;
  anim.Begin;
end.