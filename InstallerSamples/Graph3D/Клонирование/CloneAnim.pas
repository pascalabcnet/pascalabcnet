// Анимация клона

uses Graph3D;
begin
  var b := Box(0,0,1,1,3,2,RandomColor);
  var b1 := b.Clone;
  b1.MoveOn(3,0,0);
  b1.AnimRotate(OrtZ,90).Begin;
end.