uses Graph3D;

begin
  View3D.ShowViewCube := False;
  var s := Box(0,0,0,3,1,2,Colors.Blue);
  OnKeyDown := k -> 
    case k of
    Key.Left: s.MoveBy(1,0,0);
    Key.Right: s.MoveBy(-1,0,0);
    Key.Up: s.MoveBy(0,-1,0);
    Key.Down: s.MoveBy(0,1,0);
    Key.X: s.Rotate(OrtX,10);
    Key.Y: s.Rotate(OrtY,10);
    Key.Z: s.Rotate(OrtZ,10);
    end;
end.