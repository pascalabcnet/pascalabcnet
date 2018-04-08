uses Graph3D;

begin
  var s := Sphere(Origin,1,Colors.Green);
  s.AnimMoveOnX(5).WhenCompleted(procedure->s.AnimMoveOnY(5).Begin).Begin;
  var s1 := s.Clone;
  s1.Color := Colors.Blue;
  s1.AnimMoveOnX(-5).WhenCompleted(procedure->s1.AnimMoveOnY(-5).Begin).Begin;
end.