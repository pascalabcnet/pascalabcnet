// Отражение шарика от стенок. Консольный режим
// Для запуска программы используйте Shift+F9 !!!
uses CRT;

const 
// Ширина поля
  w = 80; 
// Высота поля
  h = 24;

var
/// Координаты шарика 
  ax,ay: integer;
/// Вектор перемещения шарика
  vx,vy: integer;
/// Массив клеток поля  
  a: array [1..w,1..h] of char;

/// Очистка массива a
procedure CleanA;
begin
  for var j := 1 to h do
  for var i := 1 to w do
    A[i,j] := ' '
end;

/// Создание горизонтальной стены 
procedure HorizWall(x,y,L: integer);
begin
  for var i := x to x+L-1 do
    A[i,y] := '*'
end;

/// Создание вертикальной стены 
procedure VertWall(x,y,L: integer);
begin
  for var j := y to y+L-1 do
    A[x,j] := '*'
end;

/// Заполнение поля стенами 
procedure Fill;
begin
  CleanA;
  HorizWall(1,1,w);
  HorizWall(1,h,w);
  VertWall(1,1,h);
  VertWall(w,1,h);

  HorizWall(49,9,31);
  HorizWall(49,14,31);
end;

/// Перерисовка экрана 
procedure DrawScreen;
begin
  TextColor(White);
  ClrScr;
  for var j := 1 to h do
  for var i := 1 to w do
    write(A[i,j])
end;

/// Возвращает True, если на пути шарика препятствие 
function FilledInFront: boolean;
begin
  Result := (A[ax+vx,ay]<>' ') or (A[ax,ay+vy]<>' ') or (A[ax+vx,ay+vy]<>' ')
end;

/// Меняет направление шарика
procedure ChangeDirection;
begin
  if A[ax+vx,ay]<>' ' then 
    vx := -vx;
  if A[ax,ay+vy]<>' ' then 
    vy := -vy;
  if (A[ax+vx,ay]=' ') and (A[ax,ay+vy]=' ') and (A[ax+vx,ay+vy]<>' ') then
  begin
    vx := -vx;
    vy := -vy
  end;
  Sleep(10);
end;

/// Рисует шарик
procedure ShowBall;
begin
  GotoXY(ax,ay);
  write('B');
end;

/// Стирает шарик
procedure HideBall;
begin
  GotoXY(ax,ay);
  write(' ');
end;

/// Устанавливает вектор движение шарика
procedure SetBallCoords(x,y: integer);
begin
  ax := x;
  ay := y
end;

/// Устанавливает координаты шарика
procedure SetBallVeloc(vx0,vy0: integer);
begin
  vx := vx0;
  vy := vy0
end;

/// Перемещает шарик к позиции (x,y)
procedure MoveTo(x,y: integer);
begin
  HideBall;
  SetBallCoords(x,y);
  ShowBall
end;

/// Перемещает шарик на вектор (dx,dy)
procedure MoveOn(dx,dy: integer);
begin
  MoveTo(ax+dx,ay+dy);
end;

BEGIN
  SetWindowTitle('Биллиард (ностальгия по CRT)');
  HideCursor;

  Fill;
  DrawScreen;

  SetBallCoords(70,13);
  SetBallVeloc(1,1);
  TextColor(Yellow);
  ShowBall;

  repeat
    Delay(20);
    if FilledInFront then 
      ChangeDirection;
    MoveOn(vx,vy);
  until KeyPressed;
END.
