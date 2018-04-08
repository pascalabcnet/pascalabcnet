uses Graph3D;

begin
  var b := Box(0,0,0,3,1,2,Colors.Blue);
  var b1 := b.Clone.RotateAt(OrtZ,90,P3D(1.5,0,0)).MoveOnX(-0.5);
  b1 := b.Clone.RotateAt(OrtZ,-90,P3D(-1.5,0,0)).MoveOnX(0.5);
  View3D.BackgroundColor := Colors.Black;
  View3D.ShowGridLines := False;
end.