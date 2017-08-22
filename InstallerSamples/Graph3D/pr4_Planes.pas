uses Graph3D;

begin
  View3D.ShowGridLines := False;
  
  Camera.Position := P3D(12,16,24);
  Camera.LookDirection := Camera.Position.Multiply(-1).ToVector3D;

  var sz := 12;
  var alpha := 100;
  var planeXZ := Rectangle3D(0,0,0,sz,sz,V3D(0,1,0),Colors.Green.ChangeAlpha(alpha));
  var planeXY := Rectangle3D(0,0,0,sz,sz,V3D(0,0,1),Colors.Blue.ChangeAlpha(alpha));
  var planeYZ := Rectangle3D(0,0,0,sz,sz,V3D(1,0,0),V3D(0,1,0),Colors.Red.ChangeAlpha(alpha));
  BillboardText(sz/2,sz/2,0,'XY',20);
  BillboardText(0,sz/2,sz/2,'YZ',20);
  BillboardText(sz/2,0,sz/2,'XZ',20);
  var len := 8;
  CoordinateSystem(len,0.3);
  BillboardText(len+0.5,0,0,'X',20);
  BillboardText(0,len+0.5,0,'Y',20);
  BillboardText(0,0,len+0.5,'Z',20);
end.