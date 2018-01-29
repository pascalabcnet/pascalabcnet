uses Graph3D;

begin
  var s := Sphere(0,0,0,1,Colors.Red);
  var c := Cube(0,0,1.5,1,Colors.Blue);
  var p := Pyramid(0,0,0.5,4,0.5,1,Colors.LightGreen);
  s.AddChild(c);
  c.AddChild(p);
  var s1 := s.Clone;
  s1.MoveOnX(2);
  var g := Group(s,s1);
  var g1 := g.Clone;
  g.MoveOnY(-4);
end.