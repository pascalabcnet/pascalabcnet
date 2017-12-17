uses Graph3D;

begin
  var ss := Cube(0,0,0,1.5,Colors.Red);
  var anim := ss.AnimRotate(OrtZ,360).Forever.AutoReverse;
  anim.Begin;
end.