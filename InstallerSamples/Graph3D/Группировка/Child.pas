uses Graph3D;

begin
  var s := Sphere(0,1,2,2,Colors.Orchid);
  s.AddChild(Sphere(0,2,1,0.5,Colors.White));
  s.Rotate(OrtZ,90);
end.