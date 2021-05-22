uses WPFObjects,Timers;

type 
  BulletWPF = class(CircleWPF) end;
  MonsterWPF = class(SquareWPF) end;
  PlayerWPF = class(EllipseWPF) end;

begin
  var Player := new PlayerWPF(GraphWindow.Center, 30, 50, RandomColor);
  Player.Velocity := 100;
  Player.Number := 0;
  
  loop 5 do
  begin
    var m := new MonsterWPF(750,Random(10,550),30,RandomColor);
    m.Velocity := 50;
  end;  
  
  OnMouseMove := (x,y,mb) -> Player.RotateToPoint(x,y);
  
  OnMouseDown := (x,y,mb) -> begin
    var cc := new BulletWPF(Player.CenterTop,5,Colors.Red);
    cc.Direction := (x-Player.Center.X,y-Player.Center.Y);
    cc.Velocity := 300;
  end;
  
  var kl,kr,ku,kd: boolean;

  OnDrawFrame := dt -> begin
    Window.Title := 'Количество объектов: '+Objects.Count;
    // Перемещение игрока
    if kr then 
      Player.Dx := 1
    else if kl then 
      Player.Dx := -1
    else Player.Dx := 0;  
    if ku then 
      Player.Dy := -1
    else if kd then 
      Player.Dy := 1
    else Player.Dy := 0; 
    
    for var i:=Objects.Count-1 downto 0 do // все перемещаются в своём направлении со своей скоростью
    begin
      var o := Objects[i];
      if o is MonsterWPF then 
        o.Direction := (Player.Center.X - o.Center.X,Player.Center.Y-o.Center.Y);
      o.MoveTime(dt);
    end;
    
    if Player.IntersectionList.Any(o -> o is MonsterWPF) then
    begin
      // Конец игры
      
    end;
  end;
  
  CreateTimerAndStart(100,procedure -> // Таймер убивания монстров и умирания объектов за пределами экрана
  begin
    for var i:=Objects.Count-1 downto 0 do
    begin
      var o := Objects[i];
      if o.OutOfGraphWindow and not (o is PlayerWPF) then 
        o.Destroy;   
      if o is BulletWPF then
        foreach var x in o.IntersectionList do
          if x is MonsterWPF then
          begin
            x.Destroy;
            o.Destroy;
            Player.Number += 1;
            break;
          end;
    end;
  end);
  
  CreateTimerAndStart(1000,procedure -> // Таймер рождения монстров
  begin
    var x := if Random(2)=0 then 750 else 50;
    var m := new MonsterWPF(x,Random(10,550),30,RandomColor);
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