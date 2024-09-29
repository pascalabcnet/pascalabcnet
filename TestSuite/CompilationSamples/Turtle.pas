/// Исполнитель Черепаха
unit Turtle;

uses GraphWPFBase;
uses GraphWPF;
uses System.Threading.Tasks;

type 
  Colors = GraphWPF.Colors;
  CommandType = (ForwComm,TurnComm,CircleComm,PointComm,PointsComm,UpComm,DownComm,ToPointComm,SetColorComm,SetWidthComm);
  ICommand = interface
    procedure Play(dc: DrawingContext);
  end;
{  Command = record
    typ: CommandType;
    r,da,x,y: real;
    points: array of Point;
    color: GColor;
  end;
}
var 
  CurrentPen := ColorPen(Colors.Black,1.4);
  Palette: array of Color := Arr(Colors.Green, Colors.Blue, Colors.Red, Colors.Orange, 
    Colors.Magenta, Colors.LightGreen, Colors.LightBlue, Colors.Coral, Colors.Gray, Colors.LightGray);
  PaletteIndex := 0;
  
function Pnt(x,y: real): Point := GraphWPF.Pnt(x,y);

{$region Класс координатной сетки FS}

var MinLen := 25;
var PointRadius := 2.0;

// Сделаем в логических координатах fso.LineToReal, fso.MoveToReal, fso.CircleReal
type
  FS = class
    // origin - точка по центру графика в логических координатах
    origin: Point; 
    // scale - количество пикселей на одну единицу
    scale: real; 
    mx, my: real;
    a, b, min, max: real; // реальные координаты [a.b] x [min,max]
    x, y, w, h: real; // экранные координаты - левый верхний угол, ширина и высота
    // XTicks, YTicks - сколько логических единиц занимает одно деление (по умолчанию 1) 
    XTicks, YTicks: real; 
    // XTicks = YTicks = FindP(Scale)/Scale
    // Ticks будет меняться по стратегии: 0.1 0.2 0.5 1 2 5 10
    MarginY := 6;
    MarginX := 6;
    XTicksPrecision := 4;
    YTicksPrecision := 5;
    spaceBetweenTextAndGraph := 0;
    
    constructor (scale, x, y, w, h: real; origin: Point := Pnt(0,0));
    begin
      Self.Scale := scale;
      Self.x := x; Self.y := y; Self.w := w; Self.h := h; 
      Self.origin := origin;
      CorrectBounds; 
    end;
    
    function PointInside(p: Point): boolean;
    begin
      Result := (p.X >= a) and (p.X <= b) and (p.Y >= min) and (p.Y <= max);
    end;
    
    function FindP(Scale: real): real;
    begin // MinLen = 25 - то самое число
      var p := Scale;
      if p < MinLen then
        while p < MinLen do
          p *= 10
      else begin
        while p >= MinLen * 10 do
          p /= 10;
      end;
      // p in [MinLen..MinLen * 10)
      if p >= MinLen * 5 then
        p /= 5
      else if p >= MinLen *2 then
        p /= 2;
      Result := p;
    end;
    
    procedure CorrectBounds;
    begin
      mx := Scale;
      my := Scale;
      a := -w/2/mx;
      b := -a;
      max := h/2/my;
      min := - max;
      
      XTicks := FindP(Scale)/Scale;
      YTicks := XTicks;

      var th := TextHeightP('0'); // высота текста
      // tw - ширина текста - максимум по всем числам
      var RY0 := GetRY0; // самое маленькое логическое значение по y. Соответствует нижней части экрана
      var tw := RY0.Step(YTicks).TakeWhile(ry -> ry <= max + origin.y)
        .Select(y -> TextWidthP(y.Round(YTicksPrecision).ToString)).DefaultIfEmpty.Max;
      if tw = 0 then
        tw := TextWidthP('0');  
      var dd := TextWidthP(b.Round(xTicksPrecision).ToString)/2;
      // dd - это половинка ширины последней цифры
      //var tw := TextWidth('-99.9');
      w -= {MarginX * 2 +} spaceBetweenTextAndGraph + tw + dd;
      h -= MarginY * 2 + spaceBetweenTextAndGraph + th;
      x += MarginX + spaceBetweenTextAndGraph + tw; 
      y += MarginY;
      
      // И ещё раз пересчитаем!
      a := -w/2/mx;
      b := -a;
      max := h/2/my;
      min := - max;
      
      XTicks := FindP(Scale)/Scale;
      YTicks := XTicks;
    end;
    
    // a,b,min,max - логические
    function RealToScreenX(xx: real) := x + mx * (xx - origin.x - a);
    function RealToScreenY(yy: real) := y + h - my * (yy - origin.y - min); // ?
    function ScreenToRealX(xx: real) := (xx - x) / mx + a + origin.x;
    function ScreenToRealY(yy: real) := (y + h - yy) / my + min + origin.y;
    function ScreenToReal(p: Point): Point := Pnt(ScreenToRealX(p.X),ScreenToRealY(p.Y));
    function RealToScreen(p: Point): Point := Pnt(RealToScreenX(p.X),RealToScreenY(p.Y));
    
    /// Самое маленькое логическое значение по x
    function GetRX0: real;
    begin
      var xt := XTicks * Trunc(Abs(a + origin.x)/XTicks);
      var rx0: real; 
      if a + origin.x <= 0 then 
        rx0 := -xt 
      else rx0 := xt + XTicks;
      Result := rx0;
    end;
    
    /// Самое маленькое логическое значение по y
    function GetRY0: real;
    begin
      var yt: real := YTicks * Trunc(Abs(min + origin.y)/YTicks);
      var ry0: real;
      if min + origin.y <= 0 then 
        ry0 := -yt 
      else ry0 := yt + YTicks;
      Result := ry0;
    end;

    procedure DrawDC(dc: DrawingContext);
    begin
      var AxisColor := GrayColor(112);
      var GridPen := ColorPen(Colors.LightGray,1);
      var AxisPen := ColorPen(AxisColor,1);

      var stepx := mx * XTicks; // Экранный шаг по оси x
      var rx0 := GetRX0; // реальная начальная координата по x. При запуске = 14
      var x0 := RealToScreenX(rx0); // Экранная начальная координата по x. 
      
      // ширина надписи по оси ox
      var www := PABCSystem.max(TextWidthP(rx0.Round(XTicksPrecision).ToString),
                   TextWidthP((rx0+XTicks).Round(XTicksPrecision).ToString)) + 3;
      var numTicks := Trunc(w / 2 / stepx);
                   
      var it := 0;
      if numTicks.IsEven then
        it += 1;

      while x0<=x+w+0.000001 do
      begin
        if Abs(rx0)<0.000001 then
          dc.DrawLine(AxisPen,Pnt(x0,y),Pnt(x0,y+h))
        else dc.DrawLine(GridPen,Pnt(x0,y),Pnt(x0,y+h));
        var text := rx0.Round(XTicksPrecision).ToString;
        if (www <= stepx) or (it mod 2 = 1) then
          TextOutDC(dc,x0,y+h+4,text,Alignment.CenterTop);
        x0 += stepx;
        rx0 += XTicks;
        it += 1;
      end;
      
      var stepy := my * YTicks;
      var ry0 := GetRY0;
      var y0 := RealToScreenY(ry0);
      
      while y0>=y-0.000001 do
      begin
        if Abs(ry0)<0.000001 then
          DrawLineDC(dc,x,y0,x+w,y0,AxisPen)
        else DrawLineDC(dc,x,y0,x+w,y0,GridPen);  
        TextOutDC(dc,x-4,y0,ry0.Round(YTicksPrecision).ToString,Alignment.RightCenter);
        y0 -= stepy;
        ry0 += YTicks;
      end;
      
      DrawRectangleDC(dc, x, y, w, h, nil, ColorPen(Colors.Black,1));
    end;
    
    procedure Draw := FastDraw(DrawDC);
    
    procedure DrawPoints := FastDraw(dc -> begin
      var sz := 1.2;
      if Scale < 10 then
        exit;
      var a := ScreenToRealX(x); var b := ScreenToRealX(x+w);
      var min := ScreenToRealY(y+h); var max := ScreenToRealY(y);
      for var x := Ceil(a) to Floor(b) do
      for var y := Ceil(min) to Floor(max) do
      begin
        var p := RealToScreen(Pnt(x,y));
        dc.DrawEllipse(ColorBrush(Colors.Black),nil,p,sz,sz);
      end;
    end);
    
    // Примитивы рисования в логических (реальных) координатах, необходимые Черепашке для рисования
    procedure LineToReal(x,y: real);
    begin
      LineTo(RealToScreenX(x),RealToScreenY(y));
    end;

    procedure MoveToReal(x,y: real);
    begin
      MoveTo(RealToScreenX(x),RealToScreenY(y));
    end;
    
    procedure CircleReal(r: real);
    begin
      var w := Pen.Width;
      Pen.Width := 1;
      GraphWPF.Circle(Pen.X,Pen.Y,Scale * r);
      Pen.Width := w;
    end;
    
    procedure CircleRealColor(r: real; c: Color);
    begin
      var w := Pen.Width;
      Pen.Width := 1;
      GraphWPF.Circle(Pen.X,Pen.Y,Scale * r,c);
      Pen.Width := w;
    end;

    procedure PointRealColor(x,y: real; c: Color) := FastDraw(dc ->
    begin
      var p := RealToScreen(Pnt(x,y));
      dc.DrawEllipse(ColorBrush(c),nil,p,PointRadius,PointRadius);
    end);

    procedure PointsRealColor(points: array of Point; c: Color) := FastDraw(dc ->
    begin
      var cc := ColorBrush(c);
      foreach var p in points do
      begin  
        var pp := RealToScreen(p);
        dc.DrawEllipse(cc,nil,pp,PointRadius,PointRadius);
      end;  
    end);
  end;
  
{$endregion}  

// Переменные для системы координат
var 
  Scale := 27.1;
  Origin := Pnt(0,0);
  
// Переменные для Черепашки
var 
  tp: Point;   // текущая точка Черепахи (экранная, но сделаем её логической)
  angle: real; // Угол поворота Черепахи
  dr := False; // Опущен хвост Черепахи или нет
  
// Команды для "проигрывания" при перерисовке (изменении масштаба и сдвиге)
  Commands := new List<ICommand>;

// Система координат  
var fso: FS;

// Вспомогательные переменные для событий
var 
  StartPoint,MousePoint: Point; 
  deltaMultScale := 1.2; // на сколько увеличивать-уменьшать при прокручивании колёсика мыши
  
// Вспомогательная переменная для задачи перерисовки
var tsk: System.Threading.Tasks.Task := nil; // Задача для перерисовки. В каждый момент одна
  

{$region Функции создания команд для "проигрывания"}

{type
  Command = record
    typ: CommandType;
    r,da,x,y: real;
    points: array of Point;
    color: GColor;
  end;}
  
{$region Команды Черепахи при "проигрывании"}

// Сделаем свои команды ForwPlay, TurnPlay, CirclePlay и т.д.

/// Продвигает Черепаху вперёд на расстояние r
procedure ForwPlay(dc: DrawingContext; r: real);
begin
  tp += r * Vect(Cos(DegToRad(angle)),Sin(DegToRad(angle)));
  if dr then 
    dc.DrawLine(CurrentPen,Pnt(Pen.X,Pen.Y),fso.RealToScreen(tp));
  fso.MoveToReal(tp.X,tp.Y)
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
  fso.MoveToReal(tp.X,tp.Y);
end;

procedure CirclePlay(dc: DrawingContext; r: real; color: GColor);
begin
  dc.DrawEllipse(ColorBrush(color),ColorPen(Pen.Color,1),fso.RealToScreen(tp),r * Scale,r * Scale);
end;  

procedure PointPlay(dc: DrawingContext; x,y: real; color: GColor);
begin
  dc.DrawEllipse(ColorBrush(color),nil,fso.RealToScreen(Pnt(x,y)),PointRadius,PointRadius);
end;  

procedure PointsPlay(dc: DrawingContext; points: array of Point; color: GColor);
begin
  var cc := ColorBrush(color);
  foreach var p in points do
  begin  
    var pp := fso.RealToScreen(p);
    dc.DrawEllipse(cc,nil,pp,PointRadius,PointRadius);
  end;  
end;  

procedure SetWidthPlay(r: real);
begin
  CurrentPen := ColorPen(Pen.Color,r);
end;  

procedure SetColorPlay(c: GColor);
begin
  CurrentPen := ColorPen(c,Pen.Width);
end;  

{$endregion}
  
type
  ForwC = auto class(ICommand)
    r: real;
  public  
    procedure Play(dc: DrawingContext);
    begin
      ForwPlay(dc,r);
    end;
  end;
  TurnC = auto class(ICommand)
    da: real;
  public  
    procedure Play(dc: DrawingContext);
    begin
      TurnPlay(da)
    end;
  end;
  UpC = auto class(ICommand)
  public  
    procedure Play(dc: DrawingContext);
    begin
      UpPlay
    end;
  end;
  DownC = auto class(ICommand)
  public  
    procedure Play(dc: DrawingContext);
    begin
      DownPlay
    end;
  end;
  ToPointC = auto class(ICommand)
    x,y: real;
  public  
    procedure Play(dc: DrawingContext);
    begin
      ToPointPlay(x,y);
    end;
  end;
  CircleC = auto class(ICommand)
    r: real;
    c: Color;
  public  
    procedure Play(dc: DrawingContext);
    begin
      CirclePlay(dc,r,c);
    end;
  end;
  PointC = auto class(ICommand)
    x,y: real;
    c: Color;
  public  
    procedure Play(dc: DrawingContext);
    begin
      PointPlay(dc,x,y,c);
    end;
  end;
  PointsC = auto class(ICommand)
    points: array of Point;
    c: Color;
  public  
    procedure Play(dc: DrawingContext);
    begin
      PointsPlay(dc,points,c);
    end;
  end;
  SetWidthC = auto class(ICommand)
    w: real;
  public  
    procedure Play(dc: DrawingContext);
    begin
      SetWidthPlay(w);
    end;
  end;
  SetColorC = auto class(ICommand)
    c: Color;
  public  
    procedure Play(dc: DrawingContext);
    begin
      SetColorPlay(c);
    end;
  end;

procedure AddCommand(c: ICommand);
begin
  Commands.Add(c);
end;

{$endregion}

function Window := GraphWPF.Window;

// fso - глобальная и всегда инициализированная!!!
// tp - в логических (это точка Черепахи)

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
  fso.MoveToReal(tp.X,tp.Y)
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
  fso.MoveToReal(tp.X,tp.Y);
end;

/// Рисует окружность указанного радиуса
procedure Circle(r: real);
begin
  fso.CircleReal(r);
  AddCommand(new CircleC(r,Colors.White));
end;
  
/// Рисует окружность указанного радиуса и цвета 
procedure Circle(r: real; color: GColor);
begin
  fso.CircleRealColor(r,color);
  AddCommand(new CircleC(r,color));
end;  

/// Рисует точку заданным цветом
procedure DrawPoint(x,y: real; color: GColor := Colors.Black);
begin
  fso.PointRealColor(x,y,color);
  AddCommand(new PointC(x,y,color));
end;  

/// Рисует точки заданным цветом
procedure DrawPoints(points: array of Point; color: GColor);
begin
  fso.PointsRealColor(points,color);
  AddCommand(new PointsC(points,color));
end;  

/// Рисует точки следующим цветом в палитре цветов
procedure DrawPoints(points: array of Point);
begin
  var color := Palette[PaletteIndex];
  PaletteIndex += 1;
  if PaletteIndex >= Palette.Length then
    PaletteIndex := 0;
  DrawPoints(points,color);
end;  

/// Рисует точки заданным цветом
procedure DrawPoints(xx,yy: array of real; color: GColor);
begin
  var points := Zip(xx,yy,(x,y) -> Pnt(x,y)).ToArray;
  DrawPoints(points,color);
end;  

/// Рисует точки следующим цветом в палитре цветов
procedure DrawPoints(xx,yy: array of real);
begin
  var points := Zip(xx,yy,(x,y) -> Pnt(x,y)).ToArray;
  DrawPoints(points);
end;  


/// Устанавливает ширину линии 
procedure SetWidth(w: real);
begin
  CurrentPen := ColorPen(Pen.Color,w);
  Pen.Width := w;
end;  

/// Устанавливает цвет линии 
procedure SetColor(c: GColor);
begin
  CurrentPen := ColorPen(c,Pen.Width);
  Pen.Color := c;
end;  

{$endregion}


procedure SetOrigin(x,y: real);
begin
  Origin := Pnt(x,y);
  Up;
  ToPoint(x,y);
end;



var cansellation := False;

procedure PlayCommands;
begin
  FastDraw(dc -> begin
    dc.PushClip(new System.Windows.Media.RectangleGeometry(Rect(fso.x,fso.y,fso.w,fso.h)));
    foreach var command in commands do
      command.Play(dc);
    dc.Pop;  
  end);  
end;

procedure InitTurtle;
begin
  tp := (0,0);
  angle := 90;
  fso.MoveToReal(tp.X,tp.Y);
end;

var DrawCoords := 1;

procedure InitCoordinates;
begin
  fso := new FS(Scale,0,0,Window.Width,Window.Height,Origin);
  if DrawCoords = 1 then
    fso.Draw
  else if DrawCoords = 2 then
  begin
    fso.Draw;
    fso.DrawPoints;
  end;
end;

procedure Redraw;
begin
  if (tsk <> nil) and not tsk.IsCompleted then
    exit;
  tsk := Task.Run(() -> begin
    GraphWPF.Redraw(()->begin
    Window.Clear;
    InitCoordinates;
    InitTurtle;
    PlayCommands;
    end)
  end)
end;

procedure MouseDown(x,y: real; mb: integer);
begin
  StartPoint := Pnt(x,y);
end;

var AfterResize := False;

procedure MouseMove(x,y: real; mb: integer);
begin
  if AfterResize then
  begin
    AfterResize := False;
    exit;
  end;
  if (tsk<>nil) and not tsk.IsCompleted then
    exit;
  MousePoint := Pnt(x,y);
  if mb<>1 then
    exit;
  var v := fso.ScreenToReal(MousePoint) - fso.ScreenToReal(StartPoint);
  Origin.X -= v.X;
  Origin.Y -= v.Y;
  StartPoint := MousePoint;
  Redraw;
end;


procedure MouseWheel(delta: real);
begin
  if (tsk<>nil) and not tsk.IsCompleted then
    exit;
    
  if delta > 0 then // вверх - увеличение
  begin
    if MinLen/Scale > 0.0001 then
      Scale *= deltaMultScale
    else exit;
    Origin := Origin + (deltaMultScale - 1)/deltaMultScale * (fso.ScreenToReal(mousePoint) - Origin);
  end  
  else 
  begin 
    if MinLen/Scale < 500 then
      Scale /= deltaMultScale
    else exit;  
    Origin := Origin - (deltaMultScale - 1) * (fso.ScreenToReal(mousePoint) - Origin);
  end;
  Redraw; 
end;

procedure SetScale(sc: real);
begin
  Scale := sc;
  Window.Clear;
  InitCoordinates;
  InitTurtle;
end;


procedure Init;
begin
  Window.Title := 'Исполнитель Черепаха';
  Font.Size := 12;
  Pen.RoundCap := True;
end;

procedure Resize;
begin
  Redraw;
  AfterResize := True;
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
  Init;
  InitCoordinates;
  InitTurtle;
finalization  
  Redraw;
  OnMouseWheel := MouseWheel;
  OnMouseDown := MouseDown;
  OnMouseMove := MouseMove;
  OnResize := Resize;
  OnKeyDown := KeyDown;
end.