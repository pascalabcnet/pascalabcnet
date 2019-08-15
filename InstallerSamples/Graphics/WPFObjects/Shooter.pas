uses WPFObjects,Timers;

type 
  BulletWPF = class(CircleWPF) end;
  MonsterWPF = class(SquareWPF) end;

begin
  var c := new EllipseWPF(200, 300, 30, 50, RandomColor);
  c.Velocity := 100;
  c.Number := 0;
  
  loop 5 do
  begin
    var m := new MonsterWPF(750,Random(10,550),30,RandomColor);
    m.Velocity := 50;
  end;  
  
  OnMouseMove := (x,y,mb) -> c.RotateToPoint(x,y);
  
  OnMouseDown := (x,y,mb) -> begin
    var cc := new BulletWPF(c.CenterTop,5,Colors.Red);
    cc.Direction := (x-c.Center.X,y-c.Center.Y);
    cc.Velocity := 300;
  end;
  
  var kl,kr,ku,kd: boolean;

  BeginFrameBasedAnimationTime(dt->begin
    Window.Title := 'Количество объектов: '+Objects.Count;
    if kr then 
      c.Dx := 1
    else if kl then 
      c.Dx := -1
    else c.Dx := 0;  
    if ku then 
      c.Dy := -1
    else if kd then 
      c.Dy := 1
    else c.Dy := 0; 
    
    for var i:=Objects.Count-1 downto 0 do // все перемещаются в своём направлении со своей скоростью
    begin
      var o := Objects[i];
      if o is MonsterWPF then 
        o.Direction := (c.Center.X - o.Center.X,c.Center.Y-o.Center.Y);
      o.Move(dt);
    end;  
  end);
  
  CreateTimer(100,procedure -> // Таймер убивания монстров и умирания объектов за пределами экрана
  begin
    for var i:=Objects.Count-1 downto 0 do
    begin
      var o := Objects[i];
      if (o.Center.X < 0) or (o.Center.X > Window.Width) or
         (o.Center.Y < 0) or (o.Center.Y > Window.Height) then
        o.Destroy;   
      if o is BulletWPF then
        foreach var x in o.IntersectionList do
          if x is MonsterWPF then
          begin
            x.Destroy;
            o.Destroy;
            c.Number += 1;
            break;
          end;
    end;
  end);
  
  CreateTimer(500,procedure -> // Таймер рождения монстров
  begin
    var m := new MonsterWPF(750,Random(10,550),30,RandomColor);
    m.Velocity := 50;
  end);
  

  OnKeyDown := k ->
  begin
    case k of
      Key.w,Key.Up: begin ku := true; kd := false; end; 
      Key.s,Key.Down: begin kd := true; ku := false; end;
      Key.a,Key.Left: begin kl := true; kr := false; end;
      Key.d,Key.Right: begin kr := true; kl := false end;
    end;  
  end;
  OnKeyUp := k ->
  begin
    case k of
      Key.w,Key.Up: ku := false;
      Key.s,Key.Down: kd := false;
      Key.a,Key.Left: kl := false;
      Key.d,Key.Right: kr := false;
    end;  
  end;
end.