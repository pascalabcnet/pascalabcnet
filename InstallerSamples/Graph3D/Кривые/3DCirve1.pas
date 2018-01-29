uses Graph3D;

function ParametricTrajectory(a,b: real; N: integer; fun: real->Point3D) := Partition(a,b,N).Select(fun);

function ParametricCirve3D(a,b: real; fun: real->Point3D; N: integer := 200): SegmentsT;
begin
  var tr := ParametricTrajectory(a,b,N,fun);
  Result := Polyline3D(tr);
end;

begin
  //View3d.ShowGridLines := False;
  ParametricCirve3D(0,20*Pi,t->P3D(0.04*t*cos(t),0.04*t*sin(t),0.1*t),1000);
  //var p := ParametricCirve3D(0,20*Pi,t->P3D(3*cos(t),4*sin(1.4*t),4*cos(1.7*t)),1000);
end.