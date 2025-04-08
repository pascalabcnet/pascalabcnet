/// Исполнитель Черепаха
unit Turtle;

uses GraphWPFBase;
uses GraphWPF;
uses System.Threading.Tasks;
uses Coords;

type 
  Colors = GraphWPF.Colors;

// Переменные для Черепашки
var 
  tp: Point;   // текущая точка Черепахи (экранная, но сделаем её логической)
  angle: real; // Угол поворота Черепахи
  dr := False; // Опущен хвост Черепахи или нет

procedure MoveToReal(x,y: real);
begin
  MoveTo(fso.RealToScreenX(x),fso.RealToScreenY(y));
end;

  
{$region Команды Черепахи при "проигрывании"}

// Сделаем свои команды ForwPlay, TurnPlay, CirclePlay и т.д.

/// Продвигает Черепаху вперёд на расстояние r
procedure ForwPlay(dc: DrawingContext; r: real);
begin
  tp += r * Vect(Cos(DegToRad(angle)),Sin(DegToRad(angle)));
  if dr then 
    dc.DrawLine(CurrentPen,Pnt(Pen.X,Pen.Y),fso.RealToScreen(tp));
  MoveToReal(tp.X,tp.Y)
end;

procedure TurnPlay(da: real);
begin
  angle -= da;
end;

procedure DownPlay;
begin
  dr := True;
end;  

procedure UpPlay;
begin
  dr := False;
end;  

procedure ToPointPlay(x,y: real);
begin
  tp := Pnt(x,y);
  MoveToReal(tp.X,tp.Y);
end;

procedure CirclePenPosPlay(dc: DrawingContext; r: real; color: GColor);
begin
  dc.DrawEllipse(ColorBrush(color),ColorPen(Pen.Color,1),fso.RealToScreen(tp),r * Scale,r * Scale);
end;  

procedure SetWidthPlay(r: real);
begin
  CurrentPen := ColorPen(CurrentPenColor,r);
end;  

procedure SetColorPlay(c: GColor);
begin
  CurrentPen := ColorPen(c,CurrentPen.Thickness);
end;  

{$endregion}
  
type
  ForwC = auto class(Command)
    r: real;
  public  
    procedure Play(dc: DrawingContext); override;
    begin
      ForwPlay(dc,r);
    end;
  end;
  TurnC = auto class(Command)
    da: real;
  public  
    procedure Play(dc: DrawingContext); override;
    begin
      TurnPlay(da)
    end;
  end;
  UpC = auto class(Command)
  public  
    procedure Play(dc: DrawingContext); override;
    begin
      UpPlay
    end;
  end;
  DownC = auto class(Command)
  public  
    procedure Play(dc: DrawingContext); override;
    begin
      DownPlay
    end;
  end;
  ToPointC = auto class(Command)
    x,y: real;
  public  
    procedure Play(dc: DrawingContext); override;
    begin
      ToPointPlay(x,y);
    end;
  end;
  CircleC = auto class(Command)
    r: real;
    c: Color;
  public  
    procedure Play(dc: DrawingContext); override;
    begin
      CirclePenPosPlay(dc,r,c);
    end;
  end;
  SetWidthC = auto class(Command)
    w: real;
  public  
    procedure Play(dc: DrawingContext); override;
    begin
      SetWidthPlay(w);
    end;
  end;
  SetColorC = auto class(Command)
    c: Color;
  public  
    procedure Play(dc: DrawingContext); override;
    begin
      SetColorPlay(c);
    end;
  end;
  

{$endregion}

function Window := GraphWPF.Window;

// fso - глобальная и всегда инициализированная!!!
// tp - в логических (это точка Черепахи)

// Вспомогательные для Circle
procedure CirclePenPosReal(r: real); 
begin
  var w := Pen.Width;
  Pen.Width := 1;
  GraphWPF.Circle(Pen.X,Pen.Y,fso.Scale * r);
  Pen.Width := w;
end;

procedure CirclePenPosReal(r: real; c: Color);
begin
  var w := Pen.Width;
  Pen.Width := 1;
  GraphWPF.Circle(Pen.X,Pen.Y,fso.Scale * r,c);
  Pen.Width := w;
end;


{$region Команды Черепахи}

/// Продвигает Черепаху вперёд на расстояние r
procedure Forw(r: real);
begin
  AddCommand(new ForwC(r));
  tp += r * Vect(Cos(DegToRad(angle)),Sin(DegToRad(angle)));
  var p1 := Pnt(Pen.X,Pen.Y);
  var p2 := fso.RealToScreen(tp);
  //var p1r := fso.ScreenToReal(p1);
  //var b1 := fso.PointInside(p1r);
  //var b2 := fso.PointInside(tp);
  if dr {and (b1 or b2)} then 
  begin
    FastDraw(dc -> begin
      dc.DrawLine(CurrentPen,p1,p2);
    end  
    );
  end; 
  MoveToReal(tp.X,tp.Y)
end;

/// Продвигает Черепаху назад на расстояние r
procedure Back(r: real) := Forw(-r);

/// Поворачивает Черепаху на угол da по часовой стрелке
procedure Turn(da: real);
begin
  angle -= da;
  AddCommand(new TurnC(da));
end;

/// Поворачивает Черепаху на угол da по часовой стрелке
procedure TurnRight(da: real) := Turn(da);

/// Поворачивает Черепаху на угол da против часовой стрелки
procedure TurnLeft(da: real) := Turn(-da);

/// Опускает хвост Черепахи
procedure Down;
begin
  AddCommand(new DownC);
  dr := True;
end;  

/// Поднимает хвост Черепахи
procedure Up;
begin
  AddCommand(new UpC);
  dr := False;
end;  

/// Перемещает Черепаху в точку (x,y)
procedure ToPoint(x,y: real);
begin
  AddCommand(new ToPointC(x,y));
  tp := Pnt(x,y);
  MoveToReal(tp.X,tp.Y);
end;

// Это в Turtle добавление
/// Рисует окружность указанного радиуса
procedure Circle(r: real);
begin
  CirclePenPosReal(r);
  AddCommand(new CircleC(r,Colors.White));
end;
  
/// Рисует окружность указанного радиуса и цвета 
procedure Circle(r: real; color: GColor);
begin
  CirclePenPosReal(r,color);
  AddCommand(new CircleC(r,color));
end;  

/// Рисует точку заданным цветом
procedure DrawPoint(x,y: real; color: GColor := Colors.Black; PointRadius: real := 2) := Coords.DrawPoint(x,y,color,PointRadius);

/// Рисует точки заданным цветом
procedure DrawPoints(points: array of Point; color: GColor; PointRadius: real := -1) := Coords.DrawPoints(points,color,PointRadius);

/// Рисует точки следующим цветом в палитре цветов
procedure DrawPoints(points: array of Point; PointRadius: real := -1) := Coords.DrawPoints(points,PointRadius);

/// Рисует точки заданным цветом
procedure DrawPoints(xx,yy: array of real; color: GColor; PointRadius: real := -1) := Coords.DrawPoints(xx,yy,color,PointRadius);

/// Рисует точки следующим цветом в палитре цветов
procedure DrawPoints(xx,yy: array of real; PointRadius: real := -1) := Coords.DrawPoints(xx,yy,PointRadius);

/// Устанавливает ширину линии Черепахи
procedure SetWidth(w: real);
begin
  CurrentPen := ColorPen(Pen.Color,w);
  Pen.Width := w;
end;  

/// Устанавливает цвет линии Черепахи
procedure SetColor(c: GColor);
begin
  CurrentPen := ColorPen(c,Pen.Width);
  Pen.Color := c;
end;  

{$endregion}

/// Установка положения начала координат 
procedure SetOrigin(x,y: real);
begin
  Coords.SetOrigin(x,y);
  Up;
  ToPoint(x,y);
end;

procedure InitTurtle;
begin
  tp := (0,0);
  angle := 90;
  MoveToReal(tp.X,tp.Y);
  dr := False;
end;

procedure InitOnce; // Это тоже относится к Черепахе
begin
  Window.Title := 'Исполнитель Черепаха';
  Font.Size := 14;
  Pen.RoundCap := True;
end;

procedure KeyDown(k: Key);
begin
  case k of
    key.Space: 
    begin
      DrawCoords += 1;
      if DrawCoords > 2 then
        DrawCoords := 0;
      Redraw;
    end;  
  end;
end;

initialization
  InitProc := InitTurtle;
  InitTurtle;
  InitOnce;
finalization  
  OnKeyDown := KeyDown;
end.