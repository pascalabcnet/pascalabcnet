uses ABCObjects,GraphABC,Timers;

const 
  clPlayer = Color.BurlyWood;

var
  kLeftKey,kRightKey: boolean;
  kSpaceKey: integer;
  /// Игрок
  Player: RectangleABC;
  /// Таймер движения врагов
  t: Timer;
  /// Флаг конца игры
  EndOfGame: boolean;
  /// Количество неигровых объектов
  StaticObjectsCount: integer;
  /// Счетчик выигрышей
  Wins: integer;
  /// Счетчик проигрышей
  Falls: integer;
  /// Информационная строка
  InfoString: RectangleABC;
  /// Сообщение в начале игры
  NewGame: RoundRectABC;

type
  KeysType = (kLeft,kRight);
  
  /// Класс пули
  Pulya = class(CircleABC)
  public
    constructor Create(x,y: integer);
    procedure Move; override;
  end;
  
  /// Класс врага
  Enemy = class(RectangleABC)
  public
    constructor Create(x,y,w: integer);
    procedure Move; override;
  end;

constructor Pulya.Create(x,y: integer);
begin
  inherited Create(x,y,5,clRed);
  dx := 0; 
  dy := -5;
end;

procedure Pulya.Move; 
begin
  inherited Move; 
  if Top<0 then
    Visible := False;
  for var j:=StaticObjectsCount to Objects.Count-1 do
    // При столкновении пуля и объект становятся невидимыми
    if (Objects[j]<>Self) and Intersect(Objects[j]) then
    begin
      Objects[j].Visible := False;
      Visible := False;
    end;
end;

constructor Enemy.Create(x,y,w: integer);
begin
  inherited Create(x,y,w,20,clRandom);
  if Random(2)=0 then
    dx := 5
  else dx := -5;
  dy := 0;
end;

procedure Enemy.Move; 
begin
  if Random(2)<>0 then 
    Exit;
  if Random(10)=0 then 
    dy := 5;
  if (Left<0) or (Left+Width>Window.Width) or (Random(30)=0) then
    dx := -dx;
  inherited Move;
  if dy<>0 then 
    dy := 0;
  if Top>Window.Height-50 then
    EndOfGame := True;
end;

/// Количество врагов
function NumberOfEnemies: integer;
begin
  Result := 0;
  for var i:=0 to Objects.Count-1 do
    if Objects[i] is Enemy then
      Result += 1;
end;

/// Создание игрока и врагов
procedure CreateObjects;
begin
  Player := new RectangleABC(280,WindowHeight-30,100,20,clPlayer);
  for var i:=1 to 100 do
  begin
    var r1 := new Enemy(Random(WindowWidth-50),40+Random(10),50);
    r1.TextVisible := True;
    r1.Number := i;
  end;
end;

/// Разрушение игрока и врагов
procedure DestroyObjects;
begin
  for var i:=Objects.Count-1 downto StaticObjectsCount do
    Objects[i].Destroy;
end;

/// Перемещение врагов
procedure MoveObjects;
begin
  for var i:=StaticObjectsCount+1 to Objects.Count-1 do
    Objects[i].Move;
end;

/// Удаление уничтоженных объектов
procedure DestroyKilledObjects;
begin
  for var i:=ObjectsCount-1 downto StaticObjectsCount+1 do
    if not Objects[i].Visible then
      Objects[i].Destroy;
end;

/// Обработчик нажатия клавиши
procedure KeyDown(Key: integer);
begin
  case Key of
vk_Left:  kLeftKey := True;
vk_Right: kRightKey := True;
vk_Space: if kSpaceKey=2 then kSpaceKey := 1;
  end;
end;

/// Обработчик отжатия клавиши
procedure KeyUp(Key: integer);
begin
  case Key of
vk_Left:  kLeftKey := False;
vk_Right: kRightKey := False;
vk_Space: kSpaceKey := 2;
  end;
end;

/// Изменение информационной строки
procedure ChangeInfoString;
begin
  InfoString.Text := 'Врагов: '+IntToStr(NumberOfEnemies)+'      Побед: '+IntToStr(Wins)+'      Поражений: '+IntToStr(Falls);
end;

/// Обработчик нажатия символьной клавиши
procedure KeyPress(Key: char);
begin
  if (Key in ['G','П','g','п']) and EndOfGame then
  begin
    NewGame.Visible := False;
    EndOfGame := False;
    t.Start;
    CreateObjects;
    kSpaceKey := 2;
    kLeftKey := False;
    kRightKey := False;
  end;
end;

/// Обработчик отжатия мыши
procedure MouseUp(x,y,mb: integer);
begin
  if NewGame.PTInside(x,y) then
    KeyPress('G');
end;

/// Обработчик таймера
procedure TimerProc;
begin
  if kLeftKey and (Player.Left>0) then
    Player.MoveOn(-10,0);
  if kRightKey and (Player.Left+Player.Width<WindowWidth) then
    Player.MoveOn(10,0);
  if kSpaceKey=1 then
  begin
    new Pulya(Player.Left+Player.Width div 2,Player.Top-10);
    kSpaceKey := 0;
  end;
  MoveObjects;
  DestroyKilledObjects;
  RedrawObjects;
  ChangeInfoString;
  var n := NumberOfEnemies;
  // Страховка от случая, когда процедура таймера выполняется одновременно в нескольких потоках
  if n=0 then
    EndOfGame := True;
  if EndOfGame then
  begin
    if t.Enabled=False then Exit;  
    t.Stop;
    if n>0 then
      Falls += 1
    else Wins += 1;
    NewGame.Visible := True;
    DestroyObjects;
    ChangeInfoString;
    RedrawObjects;
  end;
end;

begin
  Window.Title := 'Стрелялка';
  Window.IsFixedSize := True;
  ClearWindow(clBlack);
  LockDrawingObjects;
  EndOfGame := True;
  InfoString := new RectangleABC(0,0,Window.Width,38,Color.DarkBlue);
  InfoString.Bordered := False;
  InfoString.FontColor := clWhite;
  InfoString.TextScale := 0.9;
  
  var zz := 100;
  NewGame := new RoundRectABC(zz,200,400,200,30,Color.Violet);
  NewGame.Center := Window.Center;
  NewGame.Text := 'G - Новая игра';
  StaticObjectsCount := Objects.Count;
  ChangeInfoString;
  RedrawObjects;

  OnKeyDown := KeyDown;
  OnKeyPress := KeyPress;
  OnKeyUp := KeyUp;
  OnMouseUp := MouseUp;

  t := new Timer(1,TimerProc);
end.
