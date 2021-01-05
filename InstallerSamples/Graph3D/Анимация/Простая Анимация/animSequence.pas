uses Graph3D;

begin
  var s := Box(0,0,0,3,1,2,Colors.Blue);
  var an := Animate.Sequence(
    s.AnimMoveByY(5),
    s.AnimMoveByX(5)
  );
  an.Begin;
end.