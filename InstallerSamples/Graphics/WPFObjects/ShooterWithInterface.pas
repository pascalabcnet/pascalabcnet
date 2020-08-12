uses WPFObjects, Timers, Controls;

type
  BulletWPF = class(CircleWPF) end;
  MonsterWPF = class(SquareWPF) end;
  PlayerWPF = class(EllipseWPF) end;
  TGameState = (Paused, Started, EndOfGame);

var
  // Состояние игры
  GameState: TGameState := Paused;
  // Глобальные интерфейсные объекты
  sb: StatusBarWPF;
  bstart, bstop, bnewgame: ButtonWPF;
  // Глобальные игровые объекты
  Player: PlayerWPF;
  // Клавиатура  
  kl, kr, ku, kd: boolean;


procedure InitGame;
begin
  Player := new PlayerWPF(GraphWindow.Center, 30, 50, RandomColor);
  Player.Velocity := 100;
  Player.Number := 0;
  
  loop 10 do
  begin
    var m := new MonsterWPF(750, Random(10, 550), 30, RandomColor);
    m.Velocity := 50;
  end;
end;

procedure NewGame;
begin
  sb.Text := '';
  Objects.Clear;
  InitGame;
end;

// Конец игры - игрок погиб
procedure GameOver;
begin
  sb.Text := 'Игрок погиб';
  GameState := EndOfGame;
  bstart.Enabled := False;
  bstop.Enabled := False;
  bnewgame.Enabled := True;
  Player.Color := Colors.Black;
end;

// Конец игры - победа!
procedure GameOverWin;
begin
  sb.Text := 'Победа!';
  GameState := EndOfGame;
  bstart.Enabled := False;
  bstop.Enabled := False;
  bnewgame.Enabled := True;
  Player.Color := Colors.Yellow;
end;

procedure InitInterface;
begin
  Window.SetSize(1000, 600);
  LeftPanel(200, Colors.Orange);
  bstart := Button('Start');
  bstop := Button('Stop');
  bnewgame := Button('Новая игра');
  bstop.Enabled := False;
  bstart.Enabled := False;
  bstart.Click := procedure → begin
    GameState := Started;
    bstart.Enabled := False;
    bstop.Enabled := True;
  end;
  bstop.Click := procedure → begin
    GameState := Paused;
    bstart.Enabled := True;
    bstop.Enabled := False;
  end;
  bNewGame.Click := procedure → begin
    GameState := Started;
    bnewgame.Enabled := False;
    bstart.Enabled := False;
    bstop.Enabled := True;
    NewGame;
  end;
  sb := StatusBar;
end;

// Обработчик таймера убивания монстров и умирания объектов за пределами экрана
procedure KillMonstersHandler;
begin
  if GameState <> Started then 
    exit;
  // Удалить пули, вылетевшие за экран
  Objects.DestroyAll(o → o.OutOfGraphWindow and not (o is PlayerWPF));
  // Удалить монстра, убитого пулей, вместе с пулей
  foreach var o in Objects do
  begin
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
end;

// Обработчик перерисовки экрана
procedure DrawFrame(dt: real);
begin
  if GameState <> Started then exit;
  Window.Title := 'Количество объектов: ' + Objects.Count;
  
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
  
// Перемещение остальных объектов
  foreach var o in Objects do // все перемещаются в своём направлении со своей скоростью
  begin
    if o is MonsterWPF then 
      o.Direction := (Player.Center.X - o.Center.X, Player.Center.Y - o.Center.Y);
    o.MoveTime(dt);
  end;
  
// Проверка гибели игрока
  if Player.IntersectionList.Any(o → o is MonsterWPF) then
    GameOver;

// Проверка выигрыша игрока
  if Player.Number >= 10 then
    GameOverWin;
end;

/// Обработчик таймера рождения монстров
procedure CreateMonstersHandler;
begin
  if GameState <> Started then exit;
  var x := if Random(2) = 0 then 750 else 50;
  var m := new MonsterWPF(x, Random(10, 550), 30, RandomColor);
  m.Velocity := 50;
end;

begin
  InitInterface;
 
  CreateTimerAndStart(100, KillMonstersHandler);
  CreateTimerAndStart(1000, CreateMonstersHandler);
  
// Обработчик перерисовки кадров
  OnDrawFrame := DrawFrame;

// Обработчики мыши и клавиатуры  
  OnMouseMove := (x, y, mb) → begin
    if GameState <> Started then exit;
    Player.RotateToPoint(x, y);
  end;  
  
  OnMouseDown := (x, y, mb) → begin
    if GameState <> Started then exit;
    var cc := new BulletWPF(Player.CenterTop, 5, Colors.Red);
    cc.Direction := (x - Player.Center.X, y - Player.Center.Y);
    cc.Velocity := 300;
  end;
  
  OnKeyDown := k → begin
    case k of
      Key.w, Key.Up: begin ku := true; kd := false; end; 
      Key.s, Key.Down: begin kd := true; ku := false; end;
      Key.a, Key.Left: begin kl := true; kr := false; end;
      Key.d, Key.Right: begin kr := true; kl := false end;
    end;  
  end;
  
  OnKeyUp := k → begin
    case k of
      Key.w, Key.Up: ku := false;
      Key.s, Key.Down: kd := false;
      Key.a, Key.Left: kl := false;
      Key.d, Key.Right: kr := false;
    end;  
  end;
end.