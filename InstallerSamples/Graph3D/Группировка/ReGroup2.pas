uses Graph3D;
// ошибка!
begin
  var b := Box(0,0,0,3,1,2,Colors.Blue);
  var s := Sphere(0,0,2,1,Colors.Green);
  var b1 := Box(0,-3,0,3,1,2,Colors.Blue);
  var s1 := Sphere(0,-3,2,1,Colors.Green);
 
  var g := Group(b,s);
  var g1 := Group(b1,s1);
  g.AddChild(g1[0]);// он должен отсоединиться от g!
  Sleep(1000);
  g.MoveOnX(3);
  Sleep(1000);
  g1.MoveOnY(4);
end.