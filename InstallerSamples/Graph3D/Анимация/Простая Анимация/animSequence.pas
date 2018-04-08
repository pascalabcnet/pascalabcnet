uses Graph3D;

begin
  var s := Box(0,0,0,3,1,2,Colors.Blue);
  var an := Animate.Sequence(
    s.AnimMoveOnY(5),
    s.AnimMoveOnX(5)
  );
  an.Begin;
end.