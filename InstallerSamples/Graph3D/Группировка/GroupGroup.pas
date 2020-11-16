uses Graph3D;

begin
  var b := Box(0,0,0,3,1,2,Colors.Blue);
  var s := Sphere(0,0,2,1,Colors.Green);
 
  var g := Group(b,s);
  g.MoveByY(-4);
  var g1 := g.Clone;
  g.MoveByY(3);
  var gg := Group(g,g1);
  gg.MoveByX(3);
end.