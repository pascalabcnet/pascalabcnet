﻿uses Graph3D;
// ошибка!
begin
  var b := Box(0,0,0,3,1,2,Colors.Blue);
  var s := Sphere(0,0,2,1,Colors.Green);
  var b1 := b.Clone;
  var s1 := s.Clone;
 
  var g := Group(b,s);
  var g1 := Group(b1,s1);
  g1.MoveByY(3);
  g.AddChild(g1[0]);// он должен отсоединиться от g!
  b1.MoveByY(3);
end.