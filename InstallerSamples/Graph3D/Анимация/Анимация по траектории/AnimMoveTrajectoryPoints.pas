uses Graph3D;

begin
  var b := Sphere(Origin,1,Colors.Blue);
  
  var (p1,p2,p3,p4) := (P3D(3,0,0),P3D(3,3,0),P3D(0,3,0),P3D(0,0,0));
  
  b.MoveTo(p1);
  b.AnimMoveTrajectory(Arr(p2,p3,p4,p1),3).Forever.Begin;
end.