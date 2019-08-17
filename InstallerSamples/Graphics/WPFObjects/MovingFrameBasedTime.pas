uses WPFObjects;

begin
  var c := new CircleWPF(Window.Center, 20, Colors.Green);
  
  BeginFrameBasedAnimationTime(dt->begin
    c.MoveTime(dt);    
  end);
  
  OnKeyDown := k -> begin
    case k of
      Key.Left: c.Direction := (-5, 0);
      Key.Right: c.Direction := (5, 0);
      Key.Up: c.Direction := (0, -5);
      Key.Down: c.Direction := (0, 5);
    end;
  end;
  
  OnKeyUp := k -> begin
    c.Direction := (0, 0);
  end;
end.