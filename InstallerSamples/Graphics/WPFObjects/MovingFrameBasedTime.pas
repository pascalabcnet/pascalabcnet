uses WPFObjects;

begin
  var c := new CircleWPF(Window.Center, 20, Colors.Green);
  
  OnDrawFrame := dt -> begin
    c.MoveTime(dt);    
  end;
  
  OnKeyDown := k -> begin
    case k of
      Key.Left: c.Direction := Direction.Left;
      Key.Right: c.Direction := Direction.Right;
      Key.Up: c.Direction := Direction.Up;
      Key.Down: c.Direction := Direction.Down;
    end;
  end;
  
  OnKeyUp := k -> begin
    c.Direction := Direction.Zero;
  end;
end.