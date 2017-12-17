uses Graph3D;

begin
  var ss := Cube(0,0,0,1.5,Colors.Red);
  var anim := ss.AnimMoveOnX(10).AutoReverse.Forever.AccelerationRatio(0.5,0.5);
  //var anim := ss.AnimScale(3).Forever.AutoReverse;
  //var anim := ss.AnimRotate(OrtZ,360).Forever.AutoReverse;
  //var anim := ss.AnimRotateAt(OrtZ,360,P3D(3,0,0),2).Forever;
  anim.Begin;
end.