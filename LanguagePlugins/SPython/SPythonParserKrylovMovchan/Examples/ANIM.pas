uses Graph3D;

begin
  var b := Graph3D.Box(0, 0, 0, 6, 1, 2, Graph3D.Colors.Blue);
  var s := Graph3D.Sphere(0, 0, 2, 1, Graph3D.Colors.Green);

  var g := Graph3D.Group(b, s);
  g.AnimRotateAt(Graph3D.OrtZ, 360, Graph3D.P3D(3, 0, 0), 2).Begin();
end.