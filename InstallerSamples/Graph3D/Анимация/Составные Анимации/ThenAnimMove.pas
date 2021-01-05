uses Graph3D;

begin
  var s := Sphere(Origin,1,Colors.Green);
  s.AnimMoveByX(5).WhenCompleted(procedure->s.AnimMoveByY(5).Begin).Begin;
  var s1 := s.Clone;
  s1.Color := Colors.Blue;
  s1.AnimMoveByX(-5).WhenCompleted(procedure->s1.AnimMoveByY(-5).Begin).Begin;
end.