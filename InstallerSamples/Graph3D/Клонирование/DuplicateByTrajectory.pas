uses Graph3D;

function ParametricTrajectory(a,b: real; fun: real->Point3D; N: integer) := Partition(a,b,N).Select(fun);

function ParametricCirve3D(a,b: real; fun: real->Point3D; N: integer := 200): SegmentsT;
begin
  var tr := ParametricTrajectory(a,b,fun,N);
  Result := Polyline3D(tr);
end;

procedure DuplicateByTrajectory(c: Object3D; a,b: real; fun: real->Point3D; N: integer);
begin
  c.MoveTo(fun(a));
  foreach var p in Partition(a,b,N).Skip(1).Select(fun) do
  begin
    var c1 := c.Clone.MoveTo(p);
    c1.Rotate(OrtZ,3);
    (c1 as BoxT).Color := RandomColor;
    c := c1;
  end;
end;

begin
  var c := Cube(0,0,0,0.7,Colors.Green);
  DuplicateByTrajectory(c,0,4*Pi,t->P3D(4*cos(t),4*sin(t),0.5*t),40);
end.