uses Graph3D;

begin
  Window.Title := 'Перемещение шара - анимация на основе кадра'; 
  View3D.Title := 'Используйте клавиши:';
  View3D.SubTitle := 'W,A,S,D,Стрелки - перемещение';
  var c := Sphere(0,0,0,1,Colors.SeaGreen);
  c.Direction := V3D(-1,0,0);
  c.Velocity := 5;
  
  var kl,kr,ku,kd: boolean;
  
  BeginFrameBasedAnimationTime(dt -> begin
    if kr then 
      c.Direction := V3D(-1,c.Direction.Y,0)
    else if kl then 
      c.Direction := V3D(1,c.Direction.Y,0)
    else c.Direction := V3D(0,c.Direction.Y,0);  
    if ku then 
      c.Direction := V3D(c.Direction.X,-1,0)
    else if kd then 
      c.Direction := V3D(c.Direction.X,1,0)
    else c.Direction := V3D(c.Direction.X,0,0);
    c.MoveTime(dt);
  end);
  
  OnKeyDown := k ->
  begin
    case k of
      Key.w,Key.Up:    begin ku := true; kd := false; end; 
      Key.s,Key.Down:  begin kd := true; ku := false; end;
      Key.a,Key.Left:  begin kl := true; kr := false; end;
      Key.d,Key.Right: begin kr := true; kl := false; end;
    end;  
  end;
  OnKeyUp := k ->
  begin
    case k of
      Key.w,Key.Up:    ku := false;
      Key.s,Key.Down:  kd := false;
      Key.a,Key.Left:  kl := false;
      Key.d,Key.Right: kr := false;
    end;  
  end;

end.