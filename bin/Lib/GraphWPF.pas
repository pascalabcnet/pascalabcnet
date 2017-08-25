///Основной модуль графики
unit GraphWPF;

{$reference 'PresentationFramework.dll'}
{$reference 'WindowsBase.dll'}
{$reference 'PresentationCore.dll'}

{$apptype windows}

uses System.Windows.Controls;
uses System.Windows; 
uses System.Windows.Input; 
uses System.Windows.Media; 
uses System.Windows.Media.Animation; 
uses System.Windows.Media.Imaging;
uses System.Windows.Data; 
uses System.Reflection;
uses System.Collections.ObjectModel;
uses System.Threading;
uses System.Windows.Shapes;
uses System.Windows.Threading;

var app: Application;
var CountVisuals := 0;

procedure InvokeVisual(d: System.Delegate; params args: array of object);
begin
  if CountVisuals <= 1000 then
    app.Dispatcher.Invoke(d,args)
  else
  begin
    app.Dispatcher.Invoke(d,DispatcherPriority.Background,args);
    Sleep(10);    
  end;
end;

procedure Invoke(d: System.Delegate; params args: array of object) := app.Dispatcher.Invoke(d,args);
procedure Invoke(d: ()->()) := app.Dispatcher.Invoke(d);
function Invoke<T>(d: Func0<T>) := app.Dispatcher.Invoke&<T>(d);

type 
  Key = System.Windows.Input.Key;
  Colors = System.Windows.Media.Colors;
  GColor = Color;
  GRect = System.Windows.Rect;
  GWindow = Window;
  GPen = System.Windows.Media.Pen;
  GBrush = System.Windows.Media.Brush;
  ///!#
  FontStyle = (Normal,Bold,Italic,BoldItalic);
  
  BrushType = class
  private
    c := Colors.White;
    function BrushClone := new SolidColorBrush(c);
  public  
    property Color: GColor read c write c;
  end;
  
  PenType = class
  private
    c: Color := Colors.Black;
    th: real := 1;
    fx,fy: real;
    function PenClone := new GPen(new SolidColorBrush(c),th);
  public  
    property Color: GColor read c write c;
    property Width: real read th write th;
    property X: real read fx;
    property Y: real read fy;
  end;

  ///!#
  FontType = class
  private
    tf := new Typeface('Arial');
    sz: real := 12;
    br := new SolidColorBrush(Colors.Black);
    function GetC := br.Color;
    procedure SetCP(c: Color) := br := new SolidColorBrush(c);
    procedure SetC(c: Color) := Invoke(SetCP,c);
    procedure SetNameP(s: string) := tf := new Typeface(new FontFamily(s),FontStyles.Normal,FontWeights.Normal,FontStretches.Normal); 
    function GetName := tf.FontFamily.ToString;
    procedure SetName(s: string) := Invoke(SetNameP,s);
    procedure SetFSP(fs: FontStyle);
    begin
      var s := FontStyles.Normal;
      var w := FontWeights.Normal;
      case fs of
    FontStyle.Bold: w := FontWeights.Bold;
    FontStyle.Italic: s := FontStyles.Italic;
    FontStyle.BoldItalic: begin s := FontStyles.Italic; w := FontWeights.Bold; end;
      end;
      tf := new Typeface(new FontFamily(Name),s,w,FontStretches.Normal); 
    end;
    procedure SetFS(fs: FontStyle) := Invoke(SetFSP,fs);
    function TypefaceClone := tf;
  public  
    property Color: GColor read GetC write SetC;
    property Name: string read GetName write SetName;
    property Size: real read sz write sz;
    property Style: FontStyle write SetFS;
  end;
  
  ///!#
  WindowType = class
  private 
    procedure SetLeft(l: real);
    function GetLeft: real;
    procedure SetTop(t: real);
    function GetTop: real;
    procedure SetWidth(w: real);
    function GetWidth: real;
    procedure SetHeight(h: real);
    function GetHeight: real;
    procedure SetCaption(c: string);
    function GetCaption: string;
  public 
    /// Отступ графического окна от левого края экрана в пикселах
    property Left: real read GetLeft write SetLeft;
    /// Отступ графического окна от верхнего края экрана в пикселах
    property Top: real read GetTop write SetTop;
    /// Ширина клиентской части графического окна в пикселах
    property Width: real read GetWidth write SetWidth;
    /// Высота клиентской части графического окна в пикселах
    property Height: real read GetHeight write SetHeight;
    /// Заголовок графического окна
    property Caption: string read GetCaption write SetCaption;
    /// Заголовок графического окна
    property Title: string read GetCaption write SetCaption;
    /// Очищает графическое окно белым цветом
    procedure Clear;
    /// Очищает графическое окно цветом c
    procedure Clear(c: Color);
    /// Устанавливает размеры клиентской части графического окна в пикселах
    procedure SetSize(w, h: real);
    /// Устанавливает отступ графического окна от левого верхнего края экрана в пикселах
    procedure SetPos(l, t: real);
    /// Сохраняет содержимое графического окна в файл с именем fname
    procedure Save(fname: string);
    /// Восстанавливает содержимое графического окна из файла с именем fname
    procedure Load(fname: string);
    /// Заполняет содержимое графического окна обоями из файла с именем fname
    procedure Fill(fname: string);
    /// Закрывает графическое окно и завершает приложение
    procedure Close;
    /// Сворачивает графическое окно
    procedure Minimize;
    /// Максимизирует графическое окно
    procedure Maximize;
    /// Возвращает графическое окно к нормальному размеру
    procedure Normalize;
    /// Центрирует графическое окно по центру экрана
    procedure CenterOnScreen;
    /// Возвращает центр графического окна
    function Center: Point;
    /// Возвращает прямоугольник клиентской области окна
    function ClientRect: GRect;
  end;
  
type 
  MyVisualHost = class(FrameworkElement)
  public  
    children: VisualCollection;
  protected 
    function GetVisualChild(index: integer): Visual; override;
    begin
      if (index < 0) or (index >= children.Count) then
        raise new System.ArgumentOutOfRangeException();
      Result := children[index];
    end;
    function get_VisualChildrenCount := children.Count;
  public  
    constructor;
    begin
      children := new VisualCollection(Self);
    end;
    property VisualChildrenCount: integer read get_VisualChildrenCount; override;
  end;

var MainFormThread: Thread;
//var LeftPanel: StackPanel;
var Host: MyVisualHost;
var MainWindow: GWindow;
var Brush: BrushType;
var Pen: PenType;
var Font: FontType;
var Window: WindowType;

var
  /// Событие нажатия на кнопку мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 1, если нажата левая кнопка мыши, и 2, если нажата правая кнопка мыши
  OnMouseDown: procedure(x, y: real; mousebutton: integer);
  /// Событие отжатия кнопки мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 1, если отжата левая кнопка мыши, и 2, если отжата правая кнопка мыши
  OnMouseUp: procedure(x, y: real; mousebutton: integer);
  /// Событие перемещения мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 0, если кнопка мыши не нажата, 1, если нажата левая кнопка мыши, и 2, если нажата правая кнопка мыши
  OnMouseMove: procedure(x, y: real; mousebutton: integer);
  /// Событие нажатия клавиши
  OnKeyDown: procedure(k: Key);
  /// Событие отжатия клавиши
  OnKeyUp: procedure(k: Key);
  /// Событие нажатия символьной клавиши
  OnKeyPress: procedure(ch: char);

function RGB(r,g,b: byte) := Color.Fromrgb(r, g, b);
function ARGB(a,r,g,b: byte) := Color.FromArgb(a, r, g, b);
function RandomColor := RGB(PABCSystem.Random(256), PABCSystem.Random(256), PABCSystem.Random(256));
function clRandom := RandomColor();
function Pnt(x,y: real) := new Point(x,y);
function Rect(x,y,w,h: real) := new System.Windows.Rect(x,y,w,h);

///---- Helpers
procedure SetLeft(Self: UIElement; l: integer); extensionmethod := Canvas.SetLeft(Self,l);

procedure SetTop(Self: UIElement; t: integer); extensionmethod := Canvas.SetTop(Self,t);

procedure MoveTo(Self: UIElement; l,t: integer); extensionmethod;
begin
  Canvas.SetLeft(Self,l);
  Canvas.SetTop(Self,t);
end;

function GetDC1: DrawingContext;
begin
  var visual := new DrawingVisual();
  //visual.Drawing.
  Host.children.Add(visual);
  CountVisuals += 1;
  Result := visual.RenderOpen();
end;

var GetDC: function : DrawingContext := GetDC1;

function ScaleToDevice: (real,real);
begin
  var pSource := PresentationSource.FromVisual(MainWindow);
  var m := pSource.CompositionTarget.TransformToDevice;
  Result := (m.M11,m.M22);
end;

function wplus := SystemParameters.WindowResizeBorderThickness.Left + SystemParameters.WindowResizeBorderThickness.Right;
function hplus := SystemParameters.WindowCaptionHeight + SystemParameters.WindowResizeBorderThickness.Top + SystemParameters.WindowResizeBorderThickness.Bottom;

//procedure SetBrushColorP(c: Color) := Brush.br := new SolidColorBrush(c); // hook

///---- P - primitives 

procedure EllipsePFull(x,y,r1,r2: real; b: GBrush; p: GPen);
begin
  var dc := GetDC();
  dc.DrawEllipse(b, p, Pnt(x+0.5, y+0.5), r1, r2);
  dc.Close();
end;

type VE = auto class
  g: ()->Geometry;
end;

procedure DrawGeometryP(g: VE);
begin
  var dc := GetDC();
  dc.DrawGeometry(Brush.BrushClone,Pen.PenClone,g.g());
  dc.Close();
end;

procedure RectanglePFull(x,y,w,h: real; b: GBrush; p: GPen);
begin
  var dc := GetDC();
  dc.DrawRectangle(b, p, Rect(x+0.5,y+0.5,w,h));
  dc.Close();
  var f := Host.children[0] as DrawingVisual;
  var geo := f.Drawing.Children[0] as GeometryDrawing;
  geo.Brush := Brushes.Blue;
end;

procedure LinePFull(x,y,x1,y1: real; p: GPen);
begin
  var dc := GetDC();
  dc.DrawLine(p, Pnt(x+0.5,y+0.5), Pnt(x1+0.5,y1+0.5));
  dc.Close();
end;

function FormText(text: string) := 
  new FormattedText(text,new System.Globalization.CultureInfo('ru-ru'), FlowDirection.LeftToRight, 
                    Font.TypefaceClone, Font.Size, Font.br.Clone);
  
function TextWidthP(text: string) := FormText(text).Width;
function TextHeightP(text: string) := FormText(text).Height;

type TextV = auto class
  text: string;
  function TextWidth := TextWidthP(text);
  function TextHeight := TextHeightP(text);
  function TextSize: Size;
  begin
    var ft := FormText(text);
    Result := new Size(ft.Width,ft.Height);
  end;
end;

procedure TextPFull(x,y: real; text: string; b: GBrush);
begin
  var dc := GetDC();
  //dc.DrawRectangle(Brushes.White,nil,new GRect(new Point(x,y),TextV.Create(text).TextSize));
  dc.DrawText(FormText(text),new Point(x,y));
  dc.Close();
end;

procedure DrawImageP(x,y: real; fname: string);
begin
  var dc := GetDC();
  var img := new BitmapImage(new System.Uri(fname,System.UriKind.Relative));
  dc.DrawImage(img, Rect(x, y, img.PixelWidth, img.PixelHeight));
  dc.Close();
end;

procedure DrawImageUnscaledP(x,y: real; fname: string);
begin
  var dc := GetDC();
  var (scalex,scaley) := ScaleToDevice;
  var img := new BitmapImage(new System.Uri(fname,System.UriKind.Relative));
  dc.DrawImage(img, Rect(x, y, img.PixelWidth/scalex, img.PixelHeight/scaley));
  dc.Close();
end;

// Нет свойств посмотреть размеры видео!
procedure DrawVideoP(x,y,w,h: real; fname: string);
begin
  var dc := GetDC();
  var pl := new MediaPlayer();
  pl.Open(new System.Uri(fname, System.UriKind.Relative));
  pl.Play();
  dc.DrawVideo(pl, Rect(x+0.5, y+0.5, w, h));
  dc.Close();
end;

// А теперь займёмся полигонами...
procedure DrawPolygonOrPolyline(Self: DrawingContext;
  b: GBrush; p: GPen; points: array of Point; draw_polygon: boolean); extensionmethod;
begin
  var geo := new StreamGeometry();
  geo.FillRule := FillRule.EvenOdd;

  var context: StreamGeometryContext := geo.Open();
  context.BeginFigure(Pnt(points[0].X + 0.5,points[0].Y + 0.5), true, draw_polygon);
  context.PolyLineTo(points.Select(p->Pnt(p.x+0.5,p.y+0.5)).Skip(1).ToArray(), true, false);
  context.Close;   
  
  Self.DrawGeometry(b, p, geo);
end;

procedure DrawPolygon(Self: DrawingContext; b: GBrush; p: GPen; points: array of Point); extensionmethod
  := Self.DrawPolygonOrPolyline(b,p,points,true);

procedure DrawPolyline(Self: DrawingContext; p: GPen; points: array of Point); extensionmethod
  := Self.DrawPolygonOrPolyline(nil,p,points,false);
  
procedure PolyLinePFull(points: array of Point; p: GPen);
begin
  var dc := GetDC();
  dc.DrawPolyline(p, points);
  dc.Close();
end;

procedure PolygonPFull(points: array of Point; b: GBrush; p: GPen);
begin
  var dc := GetDC();
  dc.DrawPolygon(b, p, points);
  dc.Close();
end;

procedure ArcSectorPFull(x, y, r, angle1, angle2: real; b: GBrush; p: GPen; issector: boolean);
begin
  if angle1>angle2 then Swap(angle1,angle2);
  if angle2-angle1 >= 360 then
    (angle1,angle2) := (0,360-0.00001);
  var dc := GetDC();
  var geo := new PathGeometry();
  var f := new PathFigure();
  geo.Figures.Add(f);
  var p1 := Pnt(x + r * cos(angle1*Pi/180), y - r * sin(angle1*Pi/180));
  var p2 := Pnt(x + r * cos(angle2*Pi/180), y - r * sin(angle2*Pi/180));
  f.StartPoint := p1;
  var a := new ArcSegment(p2, new Size(r,r), 0, abs(angle2-angle1)>180, SweepDirection.Counterclockwise, true);
  f.Segments.Add(a);
  if IsSector then 
  begin
    f.Segments.Add(new LineSegment(Pnt(x,y),true));
    f.Segments.Add(new LineSegment(p1,true));
  end;
  
  dc.DrawGeometry(b,p,geo);
  dc.Close;
end;

procedure ArcPFull(x, y, r, angle1, angle2: real; p: GPen) 
  := ArcSectorPFull(x, y, r, angle1, angle2, nil, p, false);

procedure SectorPFull(x, y, r, angle1, angle2: real; b: GBrush; p: GPen)
  := ArcSectorPFull(x, y, r, angle1, angle2, b, p, true);

procedure EllipseP(x,y,r1,r2: real) := EllipsePFull(x,y,r1,r2,Brush.BrushClone,Pen.PenClone);
procedure DrawEllipseP(x,y,r1,r2: real) := EllipsePFull(x,y,r1,r2,nil,Pen.PenClone);
procedure FillEllipseP(x,y,r1,r2: real) := EllipsePFull(x,y,r1,r2,Brush.BrushClone,nil);
procedure RectangleP(x,y,w,h: real) := RectanglePFull(x,y,w,h,Brush.BrushClone,Pen.PenClone);
procedure DrawRectangleP(x,y,w,h: real) := RectanglePFull(x,y,w,h,nil,Pen.PenClone);
procedure FillRectangleP(x,y,w,h: real) := RectanglePFull(x,y,w,h,Brush.BrushClone,nil);
procedure LineP(x,y,x1,y1: real) := LinePFull(x,y,x1,y1,Pen.PenClone);
procedure PolyLineP(points: array of Point) := PolyLinePFull(points,Pen.PenClone);
procedure PolygonP(points: array of Point) := PolygonPFull(points,Brush.BrushClone,Pen.PenClone);
procedure DrawPolygonP(points: array of Point) := PolygonPFull(points,nil,Pen.PenClone);
procedure FillPolygonP(points: array of Point) := PolygonPFull(points,Brush.BrushClone,nil);
procedure DrawTextP(x,y: real; text: string) := TextPFull(x,y,text,Brush.BrushClone);
procedure ArcP(x, y, r, angle1, angle2: real) := ArcPFull(x, y, r, angle1, angle2, Pen.PenClone);
procedure SectorP(x, y, r, angle1, angle2: real) := SectorPFull(x, y, r, angle1, angle2, Brush.BrushClone, Pen.PenClone);

procedure EllipseNew(x,y,r1,r2: real) 
  := InvokeVisual(DrawGeometryP,VE.Create(()->EllipseGeometry.Create(Pnt(x,y),r1,r2)));

procedure Ellipse(x,y,r1,r2: real) := InvokeVisual(EllipseP,x,y,r1,r2);
procedure DrawEllipse(x,y,r1,r2: real) := InvokeVisual(DrawEllipseP,x,y,r1,r2);
procedure FillEllipse(x,y,r1,r2: real) := InvokeVisual(FillEllipseP,x,y,r1,r2);
procedure Circle(x,y,r: real) := InvokeVisual(EllipseP,x,y,r,r);
procedure DrawCircle(x,y,r: real) := InvokeVisual(DrawEllipseP,x,y,r,r);
procedure FillCircle(x,y,r: real) := InvokeVisual(FillEllipseP,x,y,r,r);
procedure Rectangle(x,y,w,h: real) := InvokeVisual(RectangleP,x,y,w,h);
procedure DrawRectangle(x,y,w,h: real) := InvokeVisual(DrawRectangleP,x,y,w,h);
procedure FillRectangle(x,y,w,h: real) := InvokeVisual(FillRectangleP,x,y,w,h);
procedure Line(x,y,x1,y1: real) := InvokeVisual(LineP,x,y,x1,y1);
procedure DrawImage(x,y: real; fname: string) := InvokeVisual(DrawImageP,x,y,fname);
procedure DrawImageUnscaled(x,y: real; fname: string) := InvokeVisual(DrawImageUnscaledP,x,y,fname);
procedure DrawVideo(x,y: real; fname: string) := InvokeVisual(DrawVideoP,x,y,fname);
procedure PolyLine(points: array of Point) := InvokeVisual(PolyLineP,points);
procedure Polygon(points: array of Point) := InvokeVisual(PolygonP,points);
procedure DrawPolygon(points: array of Point) := InvokeVisual(DrawPolygonP,points);
procedure FillPolygon(points: array of Point) := InvokeVisual(FillPolygonP,points);
/// Рисует дугу окружности с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы angle1 и angle2 с осью OX (angle1 и angle2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
procedure Arc(x, y, r, angle1, angle2: real) := InvokeVisual(ArcP,x, y, r, angle1, angle2);
/// Рисует сектор окружности с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы angle1 и angle2 с осью OX (angle1 и angle2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
procedure Sector(x, y, r, angle1, angle2: real) := InvokeVisual(SectorP,x, y, r, angle1, angle2);

/// Ширина текста при выводе
function TextWidth(text: string) := Invoke&<real>(TextV.Create(text).TextWidth);
/// Высота текста при выводе
function TextHeight(text: string) := Invoke&<real>(TextV.Create(text).TextHeight);
/// Размер текста при выводе
function TextSize(text: string): Size := Invoke&<Size>(TextV.Create(text).TextSize);
/// Выводит строку в прямоугольник к координатами левого верхнего угла (x,y)
procedure TextOut(x,y: real; text: string) := InvokeVisual(DrawTextP,x,y,text);
/// Выводит целое в прямоугольник к координатами левого верхнего угла (x,y)
procedure TextOut(x,y: real; number: integer) := TextOut(x,y,'' + number);
/// Выводит вещественное в прямоугольник к координатами левого верхнего угла (x,y)
procedure TextOut(x,y: real; number: real) := TextOut(x,y,'' + number);
/// Выводит строку в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawTextCentered(x, y, w, h: real; text: string);
begin
  var sz := TextSize(text);
  var (dw,dh) := ((w-sz.Width)/2,(h-sz.Height)/2);
  TextOut(x+dw,y+dh,text)
end;
/// Выводит целое в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawTextCentered(x, y, w, h: real; number: integer) := DrawTextCentered(x, y, w, h, '' + number);
/// Выводит вещественное в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawTextCentered(x, y, w, h: real; number: real) := DrawTextCentered(x, y, w, h, '' + number);
/// Выводит строку в прямоугольник
procedure DrawTextCentered(r: GRect; text: string) := DrawTextCentered(r.x,r.y,r.Width,r.Height,text);
/// Выводит целое в прямоугольник
procedure DrawTextCentered(r: GRect; number: integer) := DrawTextCentered(r.x,r.y,r.Width,r.Height,number);
/// Выводит вещественное в прямоугольник
procedure DrawTextCentered(r: GRect; number: real) := DrawTextCentered(r.x,r.y,r.Width,r.Height,number);


procedure MoveTo(x,y: real) := (Pen.fx,Pen.fy) := (x,y);
procedure LineTo(x,y: real);
begin 
  Line(Pen.fx,Pen.fy,x,y);
  MoveTo(x,y);
end;
procedure MoveRel(dx,dy: real) := (Pen.fx,Pen.fy) := (Pen.fx + dx, Pen.fy + dy);
procedure LineRel(dx,dy: real) := LineTo(Pen.fx + dx, Pen.fy + dy);
procedure MoveOn(dx,dy: real) := MoveRel(dx,dy);
procedure LineOn(dx,dy: real) := LineRel(dx,dy);

type
  FS = auto class
    mx, my, a, min, max: real;
    x1, y1: real;
    f: real-> real;
    
    function Apply(x: real) := Pnt(x1 + mx * (x - a), y1 + my * (max - f(x)));
    function RealToScreenX(x: real) := x1 + mx * (x - a);
    function RealToScreenY(y: real) := y1 - my * (y + min);
  end;

/// Рисует график функции f, заданной на отрезке [a,b] по оси абсцисс и на отрезке [min,max] по оси ординат, в прямоугольнике, задаваемом параметрами x,y,w,h, 
procedure Draw(f: real-> real; a, b, min, max, x, y, w, h: real);
begin
  var coefx := w / (b - a);
  var coefy := h / (max - min);
  
  var fso := new FS(coefx, coefy, a, min, max, x, y, f);
  
  // Линии 
  {Pen.Color := Color.LightGray;
  
  var hx := 1.0;
  var xx := hx;
  while xx<b do
  begin
    var x0 := fso.RealToScreenX(xx);
    Line(x0,y1,x0,y2);
    xx += hx
  end;
  
  xx := -hx;
  while xx>a do
  begin
    var x0 := fso.RealToScreenX(xx);
    Line(x0,y1,x0,y2);
    xx -= hx
  end;
  
  var hy := 1.0;
  var yy := hy;
  while yy<max do
  begin
    var y0 := fso.RealToScreenY(yy);
    Line(x1,y0,x2,y0);
    yy += hy
  end;
  
  yy := -hy;
  while yy>min do
  begin
    var y0 := fso.RealToScreenY(yy);
    Line(x1,y0,x2,y0);
    yy -= hy
  end;
  
  // Оси 
  Pen.Color := Color.Blue;
  
  var x0 := fso.RealToScreenX(0);
  var y0 := fso.RealToScreenY(0);
  
  Line(x0,y1,x0,y2);
  Line(x1,y0,x2,y0);}
  
  // График
  
  Pen.Color := Colors.Black;
  Rectangle(x, y, w, h);
  
  Pen.Color := Colors.Black;
  var n := Round(w / 3);
  Polyline(Partition(a, b, n).Select(fso.Apply).ToArray);
end;

procedure Draw(f: real -> real; a, b, min, max: real; r: GRect) := Draw(f, a, b, min, max, r.X, r.Y, r.Width, r.Height);  

procedure Draw(f: real -> real; a, b, min, max: real) := Draw(f, a, b, min, max, Window.ClientRect);

procedure Draw(f: real -> real; a, b: real; x, y, w, h: real);
begin
  var n := Round(w / 3);
  var q := Partition(a, b, n);
  Draw(f, a, b, q.Min(f), q.Max(f), x, y, w, h)
end;

procedure Draw(f: real -> real; a, b: real; r: GRect) := Draw(f, a, b, r.X, r.Y, r.Width, r.Height);

procedure Draw(f: real -> real; r: GRect) := Draw(f, -5, 5, r);

procedure Draw(f: real -> real; a, b: real) := Draw(f, a, b, 0, 0, Window.Width - 1, Window.Height - 1);

procedure Draw(f: real -> real) := Draw(f, -5, 5);

///---- Window -----

procedure WindowTypeSetLeftP(l: real) := MainWindow.Left := l;
procedure WindowType.SetLeft(l: real) := Invoke(WindowTypeSetLeftP,l);

function WindowTypeGetLeftP := MainWindow.Left;
function WindowType.GetLeft := Invoke&<real>(WindowTypeGetLeftP);

procedure WindowTypeSetTopP(t: real) := MainWindow.Top := t;
procedure WindowType.SetTop(t: real) := Invoke(WindowTypeSetTopP,t);

function WindowTypeGetTopP := MainWindow.Top;
function WindowType.GetTop := Invoke&<real>(WindowTypeGetTopP);

procedure WindowTypeSetWidthP(w: real) := MainWindow.Width := w + wplus;
procedure WindowType.SetWidth(w: real) := Invoke(WindowTypeSetWidthP,w);

function WindowTypeGetWidthP := MainWindow.Width - wplus;
function WindowType.GetWidth := Invoke&<real>(WindowTypeGetWidthP);

procedure WindowTypeSetHeightP(h: real) := MainWindow.Height := h + hplus;
procedure WindowType.SetHeight(h: real) := Invoke(WindowTypeSetHeightP,h);

function WindowTypeGetHeightP := MainWindow.Height - hplus;
function WindowType.GetHeight := Invoke&<real>(WindowTypeGetHeightP);

procedure WindowTypeSetCaptionP(c: string) := MainWindow.Title := c;
procedure WindowType.SetCaption(c: string) := Invoke(WindowTypeSetCaptionP,c);

function WindowTypeGetCaptionP := MainWindow.Title;
function WindowType.GetCaption := Invoke&<string>(WindowTypeGetCaptionP);

procedure WindowTypeClearP := Host.children.Clear;
procedure WindowType.Clear := app.Dispatcher.Invoke(WindowTypeClearP);

procedure WindowType.Clear(c: Color);
begin
  raise new System.NotImplementedException('WindowType.Clear(c) пока не реализовано. Честное слово - в будущем!')
end;

procedure WindowTypeSetSizeP(w, h: real);
begin
  WindowTypeSetWidthP(w);
  WindowTypeSetHeightP(h);
end;
procedure WindowType.SetSize(w, h: real) := Invoke(WindowTypeSetSizeP,w,h);

procedure WindowTypeSetPosP(l, t: real);
begin
  WindowTypeSetLeftP(l);
  WindowTypeSetTopP(t);
end;
procedure WindowType.SetPos(l, t: real) := Invoke(WindowTypeSetPosP,l,t);

procedure SaveWindowP(canvas: FrameworkElement; filename: string);
begin
  var (scalex,scaley) := ScaleToDevice;
  var (dpiX,dpiY) := (scalex * 96, scaley * 96);
  var bmp := new RenderTargetBitmap(Round(Window.Width*scalex), Round(Window.Height*scaley), dpiX, dpiY, PixelFormats.Pbgra32);
  
  bmp.Render(canvas);
  
  var ext := ExtractFileExt(filename).ToLower;
  
  var encoder: BitmapEncoder; 
  case ext of
    '.png': encoder := new PngBitmapEncoder(); 
    '.jpg': encoder := new JpegBitmapEncoder(); 
    '.bmp': encoder := new BmpBitmapEncoder(); 
    '.gif': encoder := new GifBitmapEncoder(); 
    '.tiff': encoder := new TiffBitmapEncoder(); 
    else encoder := new PngBitmapEncoder();
  end;
  
  encoder.Frames.Add(BitmapFrame.Create(bmp));
  var fil := System.IO.File.Create(filename);
  encoder.Save(fil);
  fil.Close();
end;

procedure WindowType.Save(fname: string) := Invoke(SaveWindowP,host,fname);

procedure WindowType.Load(fname: string) := DrawImageUnscaled(0,0,fname);

procedure WindowType.Fill(fname: string);
begin
  //FillWindow(fname);
end;

procedure WindowType.Close := Invoke(MainWindow.Close);

procedure WindowTypeMinimizeP := MainWindow.WindowState := WindowState.Minimized;
procedure WindowType.Minimize := Invoke(WindowTypeMinimizeP);

procedure WindowTypeMaximizeP := MainWindow.WindowState := WindowState.Maximized;
procedure WindowType.Maximize := Invoke(WindowTypeMaximizeP);

procedure WindowTypeNormalizeP := MainWindow.WindowState := WindowState.Normal;
procedure WindowType.Normalize := Invoke(WindowTypeNormalizeP);

procedure WindowTypeCenterOnScreenP := MainWindow.WindowStartupLocation := WindowStartupLocation.CenterScreen;
procedure WindowType.CenterOnScreen := Invoke(WindowTypeCenterOnScreenP);

function WindowType.Center := Pnt(Width/2,Height/2);

function WindowType.ClientRect := Rect(0,0,Window.Width,Window.Height);


/// --- SystemMouseEvents
procedure SystemOnMouseDown(sender: Object; e: MouseButtonEventArgs);
begin
  var mb := 0;
  var p := e.GetPosition(host);
  if e.LeftButton = MouseButtonState.Pressed then
    mb := 1
  else if e.RightButton = MouseButtonState.Pressed then
    mb := 2;
  if OnMouseDown <> nil then  
    OnMouseDown(p.x, p.y, mb);
end;

procedure SystemOnMouseUp(sender: Object; e: MouseButtonEventArgs);
begin
  var mb := 0;
  var p := e.GetPosition(host);
  if e.LeftButton = MouseButtonState.Pressed then
    mb := 1
  else if e.RightButton = MouseButtonState.Pressed then
    mb := 2;
  if OnMouseUp <> nil then  
    OnMouseUp(p.x, p.y, mb);
end;

procedure SystemOnMouseMove(sender: Object; e: MouseEventArgs);
begin
  var mb := 0;
  var p := e.GetPosition(host);
  if e.LeftButton = MouseButtonState.Pressed then
    mb := 1
  else if e.RightButton = MouseButtonState.Pressed then
    mb := 2;
  if OnMouseMove <> nil then  
    OnMouseMove(p.x, p.y, mb);
end;

/// --- SystemKeyEvents
procedure SystemOnKeyDown(sender: Object; e: KeyEventArgs) := 
  if OnKeyDown<>nil then
    OnKeyDown(e.Key);

procedure SystemOnKeyUp(sender: Object; e: KeyEventArgs) := 
  if OnKeyUp<>nil then
    OnKeyUp(e.Key);

///----------------------------------------------------------------------

{procedure RenderFrame(s: Object; e: System.EventArgs);
begin
end;}

var mre := new ManualResetEvent(false);

procedure InitForm0;
begin
  app := new Application();
  MainWindow := new GWindow;
  host := new MyVisualHost();
  MainWindow.Content := host;
  
  Brush := new BrushType;
  Pen := new PenType;
  Font := new FontType;
  Window := new WindowType;

  MainWindow.Title := 'Графика WPF';
  var (w,h) := (800,600);
  
  (MainWindow.Width, MainWindow.Height) := (w + wplus, h + hplus);
  MainWindow.WindowStartupLocation := WindowStartupLocation.CenterScreen;
  
  MainWindow.Show;
  MainWindow.Closed += procedure(sender,e) -> begin Halt; end;
  MainWindow.MouseDown += SystemOnMouseDown;
  MainWindow.MouseUp += SystemOnMouseUp;
  MainWindow.MouseMove += SystemOnMouseMove;
  MainWindow.KeyDown += SystemOnKeyDown;
  MainWindow.KeyUp += SystemOnKeyUp;
  
  mre.Set();

  //CompositionTarget.Rendering += RenderFrame;
  app.Dispatcher.UnhandledException += (o,e) -> Println(e);
  
  app.Run();
end;

begin
  MainFormThread := new System.Threading.Thread(InitForm0);
  MainFormThread.SetApartmentState(ApartmentState.STA);
  MainFormThread.Start;
  
  mre.WaitOne; // Основная программа не начнется пока не будут инициализированы все компоненты приложения
end.