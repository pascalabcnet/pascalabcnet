uses Graph3D;

begin
  var b := Box(0,0,0,6,1,2,Colors.Blue);
  var s := Sphere(0,0,2,1,Colors.Green);

  var g := Group(b,s);
  g.AnimRotateAt(OrtZ,360,P3D(3,0,0),2).Begin;
end.