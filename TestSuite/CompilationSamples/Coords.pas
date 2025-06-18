/// Координатная сетка с возможностью масштабирования мышью
unit Coords;

uses GraphWPFBase;
uses GraphWPF;
uses System.Threading.Tasks;

type
  Globals = static class
    static PointRadius: real := 2.0;
    static LineWidth: real := 1.0;
    static FontBackgroundColor: Color := Colors.Transparent;
    static FontBorderWidth: real := 1.0;
  end;

type 
  Colors = GraphWPF.Colors;
  Point = System.Windows.Point;
  Alignment = GraphWPF.Alignment;
  Command = class
    procedure Play(dc: DrawingContext); abstract;
    function AddToListAndPlay: Command;
  end;
  TypeFace = System.Windows.Media.Typeface;
  FormattedText = System.Windows.Media.FormattedText;
  
var 
// Команды для "проигрывания" при перерисовке (изменении масштаба и сдвиге)
  Commands := new List<Command>;

function Command.AddToListAndPlay: Command;
begin
  Commands.Add(Self);
  FastDraw(dc -> Self.Play(dc));
  Result := Self;
end;

var 
  CurrentPen := ColorPen(Colors.Black,1.4); // Для LineTo
  
  CurrentFontFace := new Typeface('Arial');

  Palette: array of Color := Arr(Colors.Green, Colors.Blue, Colors.Red, Colors.Orange, 
    Colors.Magenta, Colors.LightGreen, Colors.LightBlue, Colors.Coral, Colors.Gray, Colors.LightGray);
  PaletteIndex := 0;
  
function Pnt(x,y: real): Point := GraphWPF.Pnt(x,y);

function CurrentPenColor: Color 
  := (CurrentPen.Brush as System.Windows.Media.SolidColorBrush).Color;

function CurrentPenWidth: real := CurrentPen.Thickness;

{$region Класс координатной сетки FS}

var MinLen := 25;

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
    
    // Рисование координат
    procedure Draw := FastDraw(DrawDC);
    
    // Рисование точек в целочисленных координатах. Специфический метод системы координат
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
  end;
  
const Scale0 = 27.1;
  
{$endregion}  

// Переменные для системы координат
var 
  Scale := Scale0;
  Origin := Pnt(0,0);

var  
  // Система координат  
  fso: FS;

// Вспомогательная переменная для задачи перерисовки
var tsk: System.Threading.Tasks.Task := nil; // Задача для перерисовки. В каждый момент одна

var RusCultureInfo := new System.Globalization.CultureInfo('ru-ru');

function FormText(text: string; size: real := 14; color: GColor := Colors.Black): FormattedText := 
  new FormattedText(text, RusCultureInfo, System.Windows.FlowDirection.LeftToRight, 
    CurrentFontFace, size, Brushes.Black);

function FormTextWithName(text: string; FontName: string := 'Arial'; size: real := 14; color: GColor := Colors.Black): FormattedText := 
  new FormattedText(text, RusCultureInfo, System.Windows.FlowDirection.LeftToRight, 
    new Typeface(FontName), size, Brushes.Black);

{$region Команды при "проигрывании"}
var PointRadius := 2.0;

var x := FormText('sdfgh');

procedure PointPlay(dc: DrawingContext; x,y: real; c: GColor; PointRadius: real);
begin
  if PointRadius = -1 then
    PointRadius := Globals.PointRadius;
  var p := fso.RealToScreen(Pnt(x,y));
  dc.DrawEllipse(ColorBrush(c),nil,p,PointRadius,PointRadius);
end;  

procedure PointsPlay(dc: DrawingContext; points: array of Point; c: GColor; PointRadius: real);
begin
  if PointRadius = -1 then
    PointRadius := Globals.PointRadius;
  var cc := ColorBrush(c);
  foreach var p in points do
  begin  
    var pp := fso.RealToScreen(p);
    dc.DrawEllipse(cc,nil,pp,PointRadius,PointRadius);
  end;  
end;

procedure LineToPlay(dc: DrawingContext; x,y: real);
begin
  var p := fso.RealToScreen(Pnt(x,y));
  dc.DrawLine(CurrentPen,Pnt(Pen.X,Pen.Y),p);
  GraphWPF.MoveTo(p.X,p.Y);
end;

procedure CirclePlay(dc: DrawingContext; x,y,r: real; c: Color; borderc: Color);
begin
  var cb := ColorBrush(c);
  var cp := ColorPen(borderc,CurrentPenWidth);
  var p := fso.RealToScreen(Pnt(x,y));
  r := r * Scale;
  dc.DrawEllipse(cb,cp,p,r,r);
end;

procedure RectanglePlay(dc: DrawingContext; x,y,w,h: real; c: Color; borderc: Color);
begin
  var cb := ColorBrush(c);
  var cp := ColorPen(borderc,CurrentPenWidth);
  var p := fso.RealToScreen(Pnt(x,y));
  w := w * Scale;
  h := h * Scale;
  dc.DrawRectangle(cb,cp,Rect(p.x,p.y,w,h));
end;

procedure LinePlay(dc: DrawingContext; x,y,x1,y1: real; c: Color; Width: real);
begin
  if Width = -1 then
    Width := Globals.LineWidth;
  var cp := ColorPen(c,Width);
  var p := fso.RealToScreen(Pnt(x,y));
  var p1 := fso.RealToScreen(Pnt(x1,y1));
  dc.DrawLine(cp,p,p1);
end;

procedure CorrectXYForTextHelper(var p: Point; w,h: real; Align: Alignment);
begin
  case Align of
    Alignment.LeftTop: {ничего};
    Alignment.LeftCenter: p.y -= h/2;
    Alignment.LeftBottom: p.y -= h;
    Alignment.CenterTop: begin p.x -= w/2 end;
    Alignment.Center: begin p.x -= w/2; p.y -= h/2 end;
    Alignment.CenterBottom: begin p.x -= w/2; p.y -= h end;
    Alignment.RightTop: begin p.x -= w; end;
    Alignment.RightCenter: begin p.x -= w; p.y -= h/2 end;
    Alignment.RightBottom: begin p.x -= w; p.y -= h end;
  end;
end;

procedure TextPlay(dc: DrawingContext; x,y: real; text,fontname: string; size: real; color: GColor; 
  Align: Alignment; unscaled: boolean; backgroundColor: GColor; borderWidth: real);
begin
  if unscaled = False then 
    size := size*Scale/Scale0;
  if size > 30000 then
    size := 30000;
  var p := fso.RealToScreen(Pnt(x,y));
  var ft: FormattedText := 
    fontname = nil ? FormText(text,size,color) : FormTextWithName(text,fontname,size,color);
  var (w,h) := (ft.Width,ft.Height);
  CorrectXYForTextHelper(p,w,h,Align);
  if backgroundColor = Colors.Transparent then
    backgroundColor := Globals.FontBackgroundColor;
  if borderWidth = -1 then
    borderWidth := Globals.FontBorderWidth;
  if backgroundColor <> Colors.Transparent then
    dc.DrawRectangle(ColorBrush(backgroundColor),ColorPen(Colors.Black,borderWidth),Rect(p.x-3,p.y-2,w+6,h+4));
  dc.DrawText(ft,p);
end;


{$endregion}

 
type
  PointC = auto class(Command)
    x,y: real;
    c: Color;
    pointradius: real;
  public  
    procedure Play(dc: DrawingContext); override := PointPlay(dc,x,y,c,pointradius);
  end;
  PointsC = auto class(Command)
    points: array of Point;
    c: Color;
    pointradius: real;
  public  
    procedure Play(dc: DrawingContext); override := PointsPlay(dc,points,c,pointradius);
  end;
  CircleC = auto class(Command)
    x,y,r: real;
    c,borderc: Color;
  public  
    procedure Play(dc: DrawingContext); override := CirclePlay(dc,x,y,r,c,borderc);
  end;
  RectangleC = auto class(Command)
    x,y,w,h: real;
    c,borderc: Color;
  public  
    procedure Play(dc: DrawingContext); override := RectanglePlay(dc,x,y,w,h,c,borderc);
  end;
  LineC = auto class(Command)
    x,y,x1,y1: real;
    c: Color;
    width: real;
  public  
    procedure Play(dc: DrawingContext); override := LinePlay(dc,x,y,x1,y1,c,width);
  end;
  TextC = auto class(Command)
    x,y: real;
    text: string;
    size: real;
    c: Color;
    fontname: string;
    Align: Alignment;
    unscaled: boolean;
    backgroundColor: Color;
    borderWidth: real;
  public  
    procedure Play(dc: DrawingContext); override := TextPlay(dc,x,y,text,fontname,size,c,Align,unscaled,backgroundColor,borderWidth);
  end;

procedure AddCommand(c: Command);
begin
  Commands.Add(c);
end;

{$endregion}

function Window := GraphWPF.Window;

// fso - глобальная и всегда инициализированная!!!
// tp - в логических (это точка Черепахи)

{$region Примитивы рисования, вносимые в список команд}

/// Рисует точку заданным цветом
procedure DrawPoint(x,y: real; Color: GColor := Colors.Black; PointRadius: real := -1)
  := PointC.Create(x,y,color,PointRadius).AddToListAndPlay;

/// Рисует точку заданным цветом
procedure DrawPoint(p: Point; Color: GColor := Colors.Black; PointRadius: real := -1)
  := DrawPoint(p.x,p.y,Color,PointRadius);

/// Рисует прямоугольник
procedure DrawRectangle(x,y,w,h: real; color: GColor := Colors.White; borderColor: GColor := Colors.Black)
  := RectangleC.Create(x,y,w,h,color,borderColor).AddToListAndPlay;

/// Рисует отрезок
procedure DrawLine(x,y,x1,y1: real; Color: GColor := Colors.Black; Width: real := -1)
  := LineC.Create(x,y,x1,y1,color,width).AddToListAndPlay;

/// Рисует отрезок
procedure DrawLine(p1,p2: Point; Color: GColor := Colors.Black; Width: real := -1)
  := DrawLine(p1.x,p1.y,p2.x,p2.y,Color,Width);

/// Рисует прямоугольник без границы
procedure FillRectangle(x,y,w,h: real; Color: GColor := Colors.White)
  := RectangleC.Create(x,y,w,h,color,Colors.Transparent).AddToListAndPlay;

/// Рисует круг
procedure DrawCircle(x,y,r: real; Color: GColor := Colors.White; borderColor: GColor := Colors.Black)
  := CircleC.Create(x,y,r,color,borderColor).AddToListAndPlay;

/// Рисует круг
procedure DrawCircle(p: Point; r: real; Color: GColor := Colors.White; borderColor: GColor := Colors.Black)
  := DrawCircle(p.X,p.Y,r,Color,borderColor);

/// Рисует круг без границы
procedure FillCircle(x,y,r: real; Color: GColor := Colors.White)
  := CircleC.Create(x,y,r,color,Colors.Transparent).AddToListAndPlay;

/// Рисует круг без границы
procedure FillCircle(p: Point; r: real; Color: GColor := Colors.White)
  := FillCircle(p.X,p.Y,r,color);

/// Выводит текст в указанную позицию
procedure DrawText(x,y: real; text: string; Size: real := 14; Color: GColor := Colors.Black; 
  FontName: string := nil; Align: Alignment := Alignment.LeftTop;
  BackgroundColor: GColor := Colors.Transparent; BorderWidth: real := -1);
begin
  TextC.Create(x,y,text,Size,Color,fontname,Align,false,BackgroundColor,BorderWidth).AddToListAndPlay
end;  

/// Выводит текст в указанную позицию
procedure DrawText(p: Point; text: string; Size: real := 14; Color: GColor := Colors.Black; 
  FontName: string := nil; Align: Alignment := Alignment.LeftTop;
  BackgroundColor: GColor := Colors.Transparent; BorderWidth: real := -1);
begin
  DrawText(p.X,p.Y,text,Size,Color,FontName,Align,BackgroundColor,BorderWidth);
end;  

/// Выводит текст в указанную позицию
procedure DrawTextUnscaled(x,y: real; text: string; Size: real := 14; Color: GColor := Colors.Black; 
  FontName: string := nil; Align: Alignment := Alignment.LeftTop;
  BackgroundColor: GColor := Colors.Transparent; BorderWidth: real := -1);
begin
  TextC.Create(x,y,text,Size,Color,fontname,Align,true,BackgroundColor,BorderWidth).AddToListAndPlay
end;  

/// Выводит текст в указанную позицию
procedure DrawTextUnscaled(p: Point; text: string; Size: real := 14; Color: GColor := Colors.Black; 
  FontName: string := nil; Align: Alignment := Alignment.LeftTop;
  BackgroundColor: GColor := Colors.Transparent; BorderWidth: real := -1);
begin
  DrawTextUnscaled(p.X,p.Y,text,Size,Color,FontName,Align,BackgroundColor,BorderWidth);
end;  


/// Рисует точки заданным цветом
procedure DrawPoints(points: array of Point; color: GColor; PointRadius: real := -1)
  := PointsC.Create(points,color,PointRadius).AddToListAndPlay;

/// Рисует точки следующим цветом в палитре цветов
procedure DrawPoints(points: array of Point; PointRadius: real := -1);
begin
  var color := Palette[PaletteIndex];
  PaletteIndex += 1;
  if PaletteIndex >= Palette.Length then
    PaletteIndex := 0;
  DrawPoints(points,color,PointRadius);
end;  

/// Рисует точки заданным цветом
procedure DrawPoints(xx,yy: array of real; color: GColor; PointRadius: real := -1);
begin
  var points := Zip(xx,yy,(x,y) -> Pnt(x,y)).ToArray;
  DrawPoints(points,color,PointRadius);
end;  

/// Рисует точки следующим цветом в палитре цветов
procedure DrawPoints(xx,yy: array of real; PointRadius: real := -1);
begin
  var points := Zip(xx,yy,(x,y) -> Pnt(x,y)).ToArray;
  DrawPoints(points,PointRadius);
end;

/// Расстояние между точками
function Distance(p1,p2: Point): real := Sqrt((p2.x-p1.x)**2 + (p2.y-p1.y)**2);
/// Расстояние между точками
function Distance(x1,y1,x2,y2: real): real := Sqrt((x2-x1)**2 + (y2-y1)**2);

/// Расстояние между точками
function Distance(Self: Point; p2: Point): real; extensionmethod := Distance(Self,p2);

/// Середина отрезка
function Middle(p1,p2: Point): Point := Pnt((p1.X + p2.X)/2, (p1.Y + p2.Y)/2);


{$endregion}

procedure PlayCommands;
begin
  FastDraw(dc -> begin
    dc.PushClip(new System.Windows.Media.RectangleGeometry(Rect(fso.x,fso.y,fso.w,fso.h)));
    foreach var command in commands do
      command.Play(dc);
    dc.Pop;  
  end);  
end;

var DrawCoords := 1;

procedure InitCoordGrid;
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

var 
  // Процедура дополнительной инициализации для модулей на основе Coords
  InitProc: procedure := nil;

/// Установка положения начала координат 
procedure SetOrigin(x,y: real);
begin
  Origin := Pnt(x,y);
end;

/// Установка масштаба 
procedure SetScale(sc: real); 
begin
  Scale := sc;
  Window.Clear;
  InitCoordGrid;
  if InitProc <> nil then
    InitProc;
end;

procedure Redraw;
begin
  if (tsk <> nil) and not tsk.IsCompleted then
    exit;
  tsk := Task.Run(() -> begin
    GraphWPF.Redraw(()->begin
    Window.Clear;
    InitCoordGrid;
    if InitProc <> nil then
      InitProc;
    PlayCommands;
    end)
  end)
end;

type 
  Handlers = static class
// Вспомогательные переменные для событий
  static StartPoint,MousePoint: Point; 
  static deltaMultScale := 1.2; // на сколько увеличивать-уменьшать при прокручивании колёсика мыши
  static AfterResize := False;
  
  static procedure MouseDown(x,y: real; mb: integer);
  begin
    StartPoint := Pnt(x,y);
  end;
  
  static procedure MouseMove(x,y: real; mb: integer);
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
  
  static procedure MouseWheel(delta: real);
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
  
  static procedure KeyPress(c: char);
  begin
    if c.ToLower = 's' then
      Window.SaveToClipboard;
  end;
  
  static procedure Resize;
  begin
    Redraw;
    AfterResize := True;
  end;
  end;

initialization
  InitCoordGrid;
  Window.Title := 'Система координат';
finalization  
  Redraw;
  OnMouseWheel += Handlers.MouseWheel;
  OnMouseDown += Handlers.MouseDown;
  OnMouseMove += Handlers.MouseMove;
  OnResize += Handlers.Resize;
  OnKeyPress += Handlers.KeyPress;
end.