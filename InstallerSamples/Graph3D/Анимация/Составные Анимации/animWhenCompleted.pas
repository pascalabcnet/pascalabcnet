uses Graph3D;

begin
  var t := Text3D(0,-7,1,'Начало анимации...',2);
  var s := Box(0,0,0,3,1,2,Colors.Blue);
  var an := s.AnimMoveTo(P3D(5,0,0),2).WhenCompleted(procedure -> t.Text := 'Конец!');
  an.Begin
end.