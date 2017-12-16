uses Graph3D;

function ParametricTrajectory(a,b: real; N: integer; fun: real->Point3D) := Partition(a,b,N).Select(fun);

begin
  var tr := ParametricTrajectory(0,2*Pi,100,t->P3D(8*cos(t),4*sin(t),0));
  Polyline3D(tr);
  var b := Sphere(tr.First,1,Colors.Blue);
  b.AnimMoveTrajectory(tr.Skip(1),5).Forever.Begin;
end.