uses Graph3D;

begin
  View3D.Title := 'Двигайте чайник при помощи стрелок';
  var t := Teapot(0, 0, 1, Colors.Green);
  OnKeyDown := k -> begin
    case k of
   Key.Left: t.X += 1; 
   Key.Right: t.X -= 1; 
   Key.Down: t.Y += 1; 
   Key.Up: t.Y -= 1; 
   Key.PageUp: t.Z += 1; 
   Key.PageDown: t.Z -= 1; 
    end;
  end;
end.