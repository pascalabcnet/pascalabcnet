unit TurtleABC;

interface

uses GraphABC;

type 
  TPoint = class
  public  
    X,Y: real;
    constructor(xx,yy: real) := (X,Y) := (xx,yy);
  end;
  Color = GraphABC.Color;

function Pnt(x,y: real): TPoint;

/// Передвигает Черепаху вперёд на расстояние r
procedure Forw(r: real);

/// Передвигает Черепаху назад на расстояние r
procedure Back(r: real);

/// Поворачивает Черепаху на угол da по часовой стрелке
procedure Turn(da: real);

/// Поворачивает Черепаху на угол da влево
procedure TurnLeft(da: real);

/// Поворачивает Черепаху на угол da вправо
procedure TurnRight(da: real);

/// Возвращает цвет по красной, зеленой и синей составляющей (в диапазоне 0..255)
function RGB(r,g,b: byte): Color;

procedure Mark;

/// Опускает перо Черепахи
procedure Down;

/// Поднимает перо Черепахи
procedure Up;

/// Устанавливает ширину линии 
procedure SetWidth(w: integer);

/// Устанавливает цвет линии 
procedure SetColor(c: Color);

/// Перемещает Черепаху в точку (x,y)
procedure MoveTo(x,y: real);

/// Перемещает Черепаху в точку (x,y)
procedure MoveTo(x,y: integer);

/// Сохраняет изображение, нарисованное Черепахой, в файл fname (например, 'a.png')
procedure Save(fname: string);

/// Список уникальных точек
function TurtlePoints: List<TPoint>;

/// Устанавливает масштаб
procedure SetScale(s: real);

/// Устанавливает начало координат
procedure SetOrigin(x,y: real);

/// Рисует точки в целочисленных координатах на всём экране
procedure DrawCoordPoints(R: integer := 0; DotsColor: GraphABC.Color := Color.Black);

implementation

var
  TurtleX: real := 0;
  TurtleY: real := 0;
  angle: real := 90;  // начальное направление - вверх
  penDown: boolean := True;
  lstPoints := new List<TPoint>;
  scale: real := 1.0;
  screenOriginX: real := Window.Width / 2;
  screenOriginY: real := Window.Height / 2;
  screenCenterX: real := Window.Width / 2;
  screenCenterY: real := Window.Height / 2;
  
function RGB(r,g,b: byte): Color := GraphABC.RGB(r, g, b);

function RealToScreenX(X: real) := Round(screenOriginX + X * scale);
function RealToScreenY(Y: real) := Round(screenOriginY - Y * scale);
function ScreenToRealX(sx: real) := (sx-screenOriginX)/scale;
function ScreenToRealY(sy: real) := (screenOriginY-sy)/scale;

procedure Mark;
begin
  var screenX := RealToScreenX(TurtleX);
  var screenY := RealToScreenY(TurtleY);
  Circle(screenX,screenY,3);
end;

/// Поворачивает Черепаху на угол da по часовой стрелке
procedure Turn(da: real);
begin
  angle -= da;
end;

procedure TurnLeft(da: real) := Turn(-da);

procedure TurnRight(da: real) := Turn(da);

function Pnt(x,y: real): TPoint := new TPoint(x,y);

/// Перемещает Черепаху в точку (x,y)
procedure MoveTo(x,y: real);
begin
  TurtleX := x;
  TurtleY := y;
  var screenX := RealToScreenX(TurtleX);
  var screenY := RealToScreenY(TurtleY);
  GraphABC.MoveTo(screenX, screenY);
  lstPoints.Add(Pnt(TurtleX,TurtleY));
end;

/// Перемещает Черепаху в точку (x,y)
procedure MoveTo(x,y: integer);
begin
  MoveTo(real(x),real(y));
end;

procedure LineTo(x,y: real);
begin
  TurtleX := x;
  TurtleY := y;
  var screenX := RealToScreenX(TurtleX);
  var screenY := RealToScreenY(TurtleY);
  GraphABC.LineTo(screenX, screenY);
  lstPoints.Add(Pnt(TurtleX,TurtleY));
end;

procedure LineTo(x,y: integer);
begin
  LineTo(real(x),real(y));
end;

/// Продвигает Черепаху вперёд на расстояние r
procedure Forw(r: real);
begin
  var radAngle := DegToRad(angle);
  var dx := r * Cos(radAngle);
  var dy := r * Sin(radAngle);
  
  TurtleX += dx;
  TurtleY += dy;
  
  if penDown then
    LineTo(TurtleX, TurtleY)
  else MoveTo(TurtleX, TurtleY);
end;

procedure Back(r: real) := Forw(-r);

/// Опускает перо Черепахи
procedure Down := penDown := True;

/// Поднимает перо Черепахи
procedure Up := penDown := False;

/// Устанавливает ширину линии 
procedure SetWidth(w: integer) := GraphABC.Pen.Width := w;

/// Устанавливает цвет линии 
procedure SetColor(c: Color) := GraphABC.Pen.Color := c;


procedure Save(fname: string) := GraphABC.SaveWindow(fname);

var eps := 1e-5;

function TurtlePoints: List<TPoint>;
begin
  var unique := new List<TPoint>;
  
  foreach var point in lstPoints do
    if not unique.Any(p -> Sqrt(Sqr(p.X - point.X) + Sqr(p.Y - point.Y)) < eps) then
      unique.Add(point);
  
  Result := unique;
end;

/// Устанавливает масштаб
procedure SetScale(s: real);
begin
  scale := s;
end;

/// Устанавливает начало координат
procedure SetOrigin(x,y: real);
begin
  (TurtleX,TurtleY) := (0,0);
  screenOriginX := screenCenterX + x * scale;
  screenOriginY := screenCenterY - y * scale;
  GraphABC.MoveTo(Round(screenOriginX),Round(screenOriginY));
end;

procedure DrawCoordPoints(R: integer; DotsColor: GraphABC.Color);
begin
  var x1 := screenOriginX;
  while x1 >= 0 do
    x1 -= scale;
  var xx1 := Round(ScreenToRealX(x1));
  x1 := screenOriginX;
  while x1 <= Window.Width do
    x1 += scale;
  var xx2 := Round(ScreenToRealX(x1));
  
  var y1 := screenOriginY;
  while y1 >= 0 do
    y1 -= scale;
  var yy1 := Round(ScreenToRealy(y1));
  y1 := screenOriginy;
  while y1 <= Window.Height do
    y1 += scale;
  var yy2 := Round(ScreenToRealY(y1));
  if yy1 > yy2 then
    Swap(yy1,yy2);
  
  LockDrawing;
  Brush.Color := DotsColor;
  for var x := xx1 to xx2 do
  for var y := yy1 to yy2 do
  begin
    var screenX := RealToScreenX(x);
    var screenY := RealToScreenY(y);
    if R = 0 then 
      PutPixel(screenX,screenY,DotsColor)
    else FillCircle(screenX,screenY,R)
  end;
  UnLockDrawing
end;

procedure Init;
begin
  GraphABC.Window.Title := 'Исполнитель Черепаха ABC';
  MoveTo(TurtleX, TurtleY);
  OnKeyPress := ch -> begin
    if ch = ' ' then
      DrawCoordPoints;
  end;
end;

begin
  Init;
end.