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

function operator implicit(Self: (integer, integer)): Point; extensionmethod := new Point(Self[0], Self[1]);
function operator implicit(Self: (integer, real)): Point; extensionmethod := new Point(Self[0], Self[1]);
function operator implicit(Self: (real, integer)): Point; extensionmethod := new Point(Self[0], Self[1]);
function operator implicit(Self: (real, real)): Point; extensionmethod := new Point(Self[0], Self[1]);

function operator implicit(Self: array of (real, real)): array of Point; extensionmethod := 
  Self.Select(t->new Point(t[0],t[1])).ToArray;
function operator implicit(Self: array of (integer, integer)): array of Point; extensionmethod := 
  Self.Select(t->new Point(t[0],t[1])).ToArray;

type 
  Key = System.Windows.Input.Key;
  Colors = System.Windows.Media.Colors;
  GColor = Color;
  GRect = System.Windows.Rect;
  GWindow = Window;
  GPen = System.Windows.Media.Pen;
  GPoint = System.Windows.Point;
  GBrush = System.Windows.Media.Brush;
  ///!#
  FontStyle = (Normal,Bold,Italic,BoldItalic);
  
  BrushType = class
  private
    c := Colors.White;
    function BrushConstruct := new SolidColorBrush(c);
  public  
    property Color: GColor read c write c;
  end;
  
  PenType = class
  private
    c: Color := Colors.Black;
    th: real := 1;
    fx,fy: real;
    function PenConstruct: GPen;
    begin
      Result := new GPen(new SolidColorBrush(c),th);
      Result.LineJoin := PenLineJoin.Round;
    end;
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
    c: GColor := Colors.Black;
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
    function BrushConstruct := new SolidColorBrush(c);
  public  
    property Color: GColor read c write c;
    property Name: string read GetName write SetName;
    /// Размер шрифта в единицах по 1/96 дюйма
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

  CoordType = (MathematicalCoords,ScreenCoords);
  Alignment = (LeftTop,CenterTop,RightTop,LeftCenter,Center,RightCenter,LeftBottom,CenterBottom,RightBottom);


var MainFormThread: Thread;
//var LeftPanel: StackPanel;
var Host: MyVisualHost;
var MainWindow: GWindow;
var Brush: BrushType;
var Pen: PenType;
var Font: FontType;
var Window: WindowType;

var 
  XOrigin := 0.0;
  YOrigin := 0.0;
  GlobalScale := 1.0;
  CurrentCoordType: CoordType := ScreenCoords;

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
function ColorBrush(c: Color) := new SolidColorBrush(c);
function ColorPen(c: Color) := new GPen(ColorBrush(c),Pen.Width);

///---- Helpers
procedure SetLeft(Self: UIElement; l: integer); extensionmethod := Canvas.SetLeft(Self,l);

procedure SetTop(Self: UIElement; t: integer); extensionmethod := Canvas.SetTop(Self,t);

procedure MoveTo(Self: UIElement; l,t: integer); extensionmethod;
begin
  Canvas.SetLeft(Self,l);
  Canvas.SetTop(Self,t);
end;

function GetDC: DrawingContext;
begin
  var visual := new DrawingVisual();
  Host.children.Add(visual);
  CountVisuals += 1;
  Result := visual.RenderOpen();
end;

function GetDC(t: Transform): DrawingContext;
begin
  var visual := new DrawingVisual();
  visual.Transform := t;
  Host.children.Add(visual);
  CountVisuals += 1;
  Result := visual.RenderOpen();
end;

function ScaleToDevice: (real,real);
begin
  var pSource := PresentationSource.FromVisual(MainWindow);
  if pSource = nil then // не знаю, почему
  begin
    Result := (1.0,1.0);
    exit
  end;
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
  dc.DrawEllipse(b, p, Pnt(x, y), r1, r2);
  dc.Close();
end;

type VE = auto class
  g: ()->Geometry;
end;

procedure DrawGeometryP(g: VE);
begin
  var dc := GetDC();
  dc.DrawGeometry(Brush.BrushConstruct,Pen.PenConstruct,g.g());
  dc.Close();
end;

procedure RectanglePFull(x,y,w,h: real; b: GBrush; p: GPen);
begin
  if h<0 then
  begin
    h := -h;
    y -= h;
  end;
  if w<0 then
  begin
    w := -w;
    x -= w;
  end;
  var dc := GetDC();
  dc.DrawRectangle(b, p, Rect(x,y,w,h));
  dc.Close();
  var f := Host.children[0] as DrawingVisual;
  var geo := f.Drawing.Children[0] as GeometryDrawing;
  geo.Brush := Brushes.Blue;
end;

procedure LinePFull(x,y,x1,y1: real; p: GPen);
begin
  var dc := GetDC();
  dc.DrawLine(p, Pnt(x,y), Pnt(x1,y1));
  dc.Close();
end;

function FormText(text: string) := 
  new FormattedText(text,new System.Globalization.CultureInfo('ru-ru'), FlowDirection.LeftToRight, 
                    Font.TypefaceClone, Font.Size, Font.BrushConstruct);
  
function FormTextC(text: string; c: GColor): FormattedText := 
  new FormattedText(text,new System.Globalization.CultureInfo('ru-ru'), FlowDirection.LeftToRight, 
                    Font.TypefaceClone, Font.Size, ColorBrush(c));
    
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

procedure TextPFull(x,y: real; text: string);
begin
  var dc: DrawingContext;
  if CurrentCoordType = ScreenCoords then
  begin
    dc := GetDC();
    dc.DrawText(FormText(text),new Point(x,y));
  end  
  else   
  begin
    var m := Host.RenderTransform.Value;
    var mt := new MatrixTransform(1/m.M11,0,0,1/m.M22,x,y);
    dc := GetDC(mt);
    dc.DrawText(FormText(text),new Point(0,0));
  end;
  //dc.DrawRectangle(Brushes.White,nil,new GRect(new Point(x,y),TextV.Create(text).TextSize));
  dc.Close();
end;

procedure TextPFull(x,y: real; text: string; c: Color);
begin
  var dc: DrawingContext;
  if CurrentCoordType = ScreenCoords then
  begin
    dc := GetDC();
    dc.DrawText(FormTextC(text,c),new Point(x,y));
  end  
  else   
  begin
    var m := Host.RenderTransform.Value;
    var mt := new MatrixTransform(1/m.M11,0,0,1/m.M22,x,y);
    dc := GetDC(mt);
    dc.DrawText(FormTextC(text,c),new Point(0,0));
  end;  
  //dc.DrawRectangle(Brushes.White,nil,new GRect(new Point(x,y),TextV.Create(text).TextSize));
  dc.Close();
end;

var dpic := new Dictionary<string, BitmapImage>;

function GetBitmapImage(fname: string): BitmapImage;
begin
  if not dpic.ContainsKey(fname) then 
    dpic[fname] := new BitmapImage(new System.Uri(fname,System.UriKind.Relative));
  Result := dpic[fname];
end;

procedure DrawImageP(x,y: real; fname: string);
begin
  var dc := GetDC();
  var img := GetBitmapImage(fname);
  dc.DrawImage(img, Rect(x, y, img.PixelWidth, img.PixelHeight));
  dc.Close();
end;

procedure DrawImageWHP(x,y,w,h: real; fname: string);
begin
  var dc := GetDC();
  var img := GetBitmapImage(fname);
  dc.DrawImage(img, Rect(x, y, w, h));
  dc.Close();
end;

procedure DrawImageUnscaledP(x,y: real; fname: string);
begin
  var dc := GetDC();
  var (scalex,scaley) := ScaleToDevice;
  var img := GetBitmapImage(fname);
  dc.DrawImage(img, Rect(x, y, img.PixelWidth/scalex, img.PixelHeight/scaley));
  dc.Close();
end;

function ImageWidthP(fname: string) := GetBitmapImage(fname).PixelWidth;
function ImageHeightP(fname: string) := GetBitmapImage(fname).PixelHeight;
function ImageSizeP(fname: string) := (GetBitmapImage(fname).PixelWidth,GetBitmapImage(fname).PixelHeight);

type ImHelper = auto class
  fname: string;
  function IW := ImageWidthP(fname);
  function IH := ImageHeightP(fname);
  function ISz := ImageSizeP(fname);
end;

function ImageWidth(fname: string) := Invoke&<integer>(ImHelper.Create(fname).IW);
function ImageHeight(fname: string) := Invoke&<integer>(ImHelper.Create(fname).IH);
function ImageSize(fname: string) := Invoke&<(integer,integer)>(ImHelper.Create(fname).ISz);


// Нет свойств посмотреть размеры видео!
procedure DrawVideoP(x,y,w,h: real; fname: string);
begin
  var dc := GetDC();
  var pl := new MediaPlayer();
  pl.Open(new System.Uri(fname, System.UriKind.Relative));
  pl.Play();
  dc.DrawVideo(pl, Rect(x, y, w, h));
  dc.Close();
end;

// А теперь займёмся полигонами...
procedure DrawPolygonOrPolyline(Self: DrawingContext;
  b: GBrush; p: GPen; points: array of Point; draw_polygon: boolean); extensionmethod;
begin
  var geo := new StreamGeometry();
  geo.FillRule := FillRule.EvenOdd;

  var context: StreamGeometryContext := geo.Open();
  context.BeginFigure(Pnt(points[0].X,points[0].Y), true, draw_polygon);
  context.PolyLineTo(points.Select(p->Pnt(p.x,p.y)).Skip(1).ToArray(), true, false);
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
    (angle1,angle2) := (0,360-0.0001);
  var dc := GetDC();
  var geo := new PathGeometry();
  var f := new PathFigure();
  geo.Figures.Add(f);
  var sgn := CurrentCoordType = MathematicalCoords ? 1 : -1;
  var p1 := Pnt(x + r * cos(angle1*Pi/180), y + sgn * r * sin(angle1*Pi/180));
  var p2 := Pnt(x + r * cos(angle2*Pi/180), y + sgn * r * sin(angle2*Pi/180));
  if CurrentCoordType = MathematicalCoords then
    Swap(p1,p2);
  Println(p1);  
  Println(p2);  
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

procedure ArcPFull(x, y, r, angle1, angle2: real; p: GPen) := ArcSectorPFull(x, y, r, angle1, angle2, nil, p, false);

procedure SectorPFull(x, y, r, angle1, angle2: real; b: GBrush; p: GPen) := ArcSectorPFull(x, y, r, angle1, angle2, b, p, true);

procedure EllipseP(x,y,r1,r2: real) := EllipsePFull(x,y,r1,r2,Brush.BrushConstruct,Pen.PenConstruct);
procedure DrawEllipseP(x,y,r1,r2: real) := EllipsePFull(x,y,r1,r2,nil,Pen.PenConstruct);
procedure FillEllipseP(x,y,r1,r2: real) := EllipsePFull(x,y,r1,r2,Brush.BrushConstruct,nil);
procedure EllipsePC(x,y,r1,r2: real; c: GColor) := EllipsePFull(x,y,r1,r2,ColorBrush(c),Pen.PenConstruct);
procedure DrawEllipsePC(x,y,r1,r2: real; c: GColor) := EllipsePFull(x,y,r1,r2,nil,ColorPen(c));
procedure FillEllipsePC(x,y,r1,r2: real; c: GColor) := EllipsePFull(x,y,r1,r2,ColorBrush(c),nil);

procedure RectangleP(x,y,w,h: real) := RectanglePFull(x,y,w,h,Brush.BrushConstruct,Pen.PenConstruct);
procedure DrawRectangleP(x,y,w,h: real) := RectanglePFull(x,y,w,h,nil,Pen.PenConstruct);
procedure FillRectangleP(x,y,w,h: real) := RectanglePFull(x,y,w,h,Brush.BrushConstruct,nil);
procedure RectanglePC(x,y,r1,r2: real; c: GColor) := RectanglePFull(x,y,r1,r2,ColorBrush(c),Pen.PenConstruct);
procedure DrawRectanglePC(x,y,r1,r2: real; c: GColor) := RectanglePFull(x,y,r1,r2,nil,ColorPen(c));
procedure FillRectanglePC(x,y,r1,r2: real; c: GColor) := RectanglePFull(x,y,r1,r2,ColorBrush(c),nil);

procedure ArcP(x, y, r, angle1, angle2: real) := ArcPFull(x, y, r, angle1, angle2, Pen.PenConstruct);

procedure SectorP(x, y, r, angle1, angle2: real) := SectorPFull(x, y, r, angle1, angle2, Brush.BrushConstruct, Pen.PenConstruct);
procedure DrawSectorP(x, y, r, angle1, angle2: real) := SectorPFull(x, y, r, angle1, angle2, nil, Pen.PenConstruct);
procedure FillSectorP(x, y, r, angle1, angle2: real) := SectorPFull(x, y, r, angle1, angle2, Brush.BrushConstruct, nil);
procedure SectorPC(x, y, r, angle1, angle2: real; c: GColor) := SectorPFull(x, y, r, angle1, angle2, ColorBrush(c), Pen.PenConstruct);
procedure DrawSectorPC(x, y, r, angle1, angle2: real; c: GColor) := SectorPFull(x, y, r, angle1, angle2, nil, ColorPen(c));
procedure FillSectorPC(x, y, r, angle1, angle2: real; c: GColor) := SectorPFull(x, y, r, angle1, angle2, ColorBrush(c), nil);

procedure LineP(x,y,x1,y1: real) := LinePFull(x,y,x1,y1,Pen.PenConstruct);
procedure LinePC(x,y,x1,y1: real; c: GColor) := LinePFull(x,y,x1,y1,ColorPen(c));
procedure PolyLineP(points: array of Point) := PolyLinePFull(points,Pen.PenConstruct);
procedure PolyLinePC(points: array of Point; c: GColor) := PolyLinePFull(points,ColorPen(c));

procedure PolygonP(points: array of Point) := PolygonPFull(points,Brush.BrushConstruct,Pen.PenConstruct);
procedure DrawPolygonP(points: array of Point) := PolygonPFull(points,nil,Pen.PenConstruct);
procedure FillPolygonP(points: array of Point) := PolygonPFull(points,Brush.BrushConstruct,nil);
procedure PolygonPC(points: array of Point; c: GColor) := PolygonPFull(points,ColorBrush(c),Pen.PenConstruct);
procedure DrawPolygonPC(points: array of Point; c: GColor) := PolygonPFull(points,nil,ColorPen(c));
procedure FillPolygonPC(points: array of Point; c: GColor) := PolygonPFull(points,ColorBrush(c),nil);

procedure DrawTextP(x,y: real; text: string) := TextPFull(x,y,text);
procedure DrawTextPC(x,y: real; text: string; c: GColor) := TextPFull(x,y,text,c);

procedure EllipseNew(x,y,r1,r2: real) 
  := InvokeVisual(DrawGeometryP,VE.Create(()->EllipseGeometry.Create(Pnt(x,y),r1,r2)));

procedure Ellipse(x,y,r1,r2: real) := InvokeVisual(EllipseP,x,y,r1,r2);
procedure DrawEllipse(x,y,r1,r2: real) := InvokeVisual(DrawEllipseP,x,y,r1,r2);
procedure FillEllipse(x,y,r1,r2: real) := InvokeVisual(FillEllipseP,x,y,r1,r2);
procedure Ellipse(x,y,r1,r2: real; c: GColor) := InvokeVisual(EllipsePC,x,y,r1,r2,c);
procedure DrawEllipse(x,y,r1,r2: real; c: GColor) := InvokeVisual(DrawEllipsePC,x,y,r1,r2,c);
procedure FillEllipse(x,y,r1,r2: real; c: GColor) := InvokeVisual(FillEllipsePC,x,y,r1,r2,c);

procedure Circle(x,y,r: real) := InvokeVisual(EllipseP,x,y,r,r);
procedure DrawCircle(x,y,r: real) := InvokeVisual(DrawEllipseP,x,y,r,r);
procedure FillCircle(x,y,r: real) := InvokeVisual(FillEllipseP,x,y,r,r);
procedure Circle(x,y,r: real; c: GColor) := InvokeVisual(EllipsePC,x,y,r,r,c);
procedure DrawCircle(x,y,r: real; c: GColor) := InvokeVisual(DrawEllipsePC,x,y,r,r,c);
procedure FillCircle(x,y,r: real; c: GColor) := InvokeVisual(FillEllipsePC,x,y,r,r,c);

procedure Rectangle(x,y,w,h: real) := InvokeVisual(RectangleP,x,y,w,h);
procedure DrawRectangle(x,y,w,h: real) := InvokeVisual(DrawRectangleP,x,y,w,h);
procedure FillRectangle(x,y,w,h: real) := InvokeVisual(FillRectangleP,x,y,w,h);
procedure Rectangle(x,y,w,h: real; c: GColor) := InvokeVisual(RectanglePC,x,y,w,h,c);
procedure DrawRectangle(x,y,w,h: real; c: GColor) := InvokeVisual(DrawRectanglePC,x,y,w,h,c);
procedure FillRectangle(x,y,w,h: real; c: GColor) := InvokeVisual(FillRectanglePC,x,y,w,h,c);

/// Рисует дугу окружности с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы angle1 и angle2 с осью OX (angle1 и angle2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
procedure Arc(x, y, r, angle1, angle2: real) := InvokeVisual(ArcP,x, y, r, angle1, angle2);

/// Рисует сектор окружности с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы angle1 и angle2 с осью OX (angle1 и angle2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
procedure Sector(x, y, r, angle1, angle2: real) := InvokeVisual(SectorP,x, y, r, angle1, angle2);
procedure DrawSector(x, y, r, angle1, angle2: real) := InvokeVisual(DrawSectorP,x, y, r, angle1, angle2);
procedure FillSector(x, y, r, angle1, angle2: real) := InvokeVisual(FillSectorP,x, y, r, angle1, angle2);
procedure Sector(x, y, r, angle1, angle2: real; c: GColor) := InvokeVisual(SectorPC,x, y, r, angle1, angle2, c);
procedure DrawSector(x, y, r, angle1, angle2: real; c: GColor) := InvokeVisual(DrawSectorPC,x, y, r, angle1, angle2, c);
procedure FillSector(x, y, r, angle1, angle2: real; c: GColor) := InvokeVisual(FillSectorPC,x, y, r, angle1, angle2, c);

procedure Line(x,y,x1,y1: real) := InvokeVisual(LineP,x,y,x1,y1);
procedure Line(x,y,x1,y1: real; c: GColor) := InvokeVisual(LinePC,x,y,x1,y1,c);
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

procedure PolyLine(points: array of Point) := InvokeVisual(PolyLineP,points);
procedure PolyLine(points: array of Point; c: GColor) := InvokeVisual(PolyLinePC,points,c);

procedure Polygon(points: array of Point) := InvokeVisual(PolygonP,points);
procedure DrawPolygon(points: array of Point) := InvokeVisual(DrawPolygonP,points);
procedure FillPolygon(points: array of Point) := InvokeVisual(FillPolygonP,points);
procedure Polygon(points: array of Point; c: GColor) := InvokeVisual(PolygonPC,points,c);
procedure DrawPolygon(points: array of Point; c: GColor) := InvokeVisual(DrawPolygonPC,points,c);
procedure FillPolygon(points: array of Point; c: GColor) := InvokeVisual(FillPolygonPC,points,c);


procedure DrawImage(x,y: real; fname: string) := InvokeVisual(DrawImageP,x,y,fname);
procedure DrawImage(x,y,w,h: real; fname: string) := InvokeVisual(DrawImageWHP,x,y,w,h,fname);
procedure DrawImageUnscaled(x,y: real; fname: string) := InvokeVisual(DrawImageUnscaledP,x,y,fname);
procedure DrawVideo(x,y: real; fname: string) := InvokeVisual(DrawVideoP,x,y,fname);

/// Ширина текста при выводе
function TextWidth(text: string) := Invoke&<real>(TextV.Create(text).TextWidth);
/// Высота текста при выводе
function TextHeight(text: string) := Invoke&<real>(TextV.Create(text).TextHeight);
/// Размер текста при выводе
function TextSize(text: string): Size := Invoke&<Size>(TextV.Create(text).TextSize);

procedure TextOutHelper(x,y: real; text: string) := InvokeVisual(DrawTextP,x,y,text);
//procedure TextOut(x,y: real; number: integer) := TextOut(x,y,'' + number);
//procedure TextOut(x,y: real; number: real) := TextOut(x,y,'' + number);
procedure TextOutHelper(x,y: real; text: string; c: GColor) := InvokeVisual(DrawTextPC,x,y,text,c);
//procedure TextOut(x,y: real; number: integer; c: GColor) := TextOut(x,y,'' + number,c);
//procedure TextOut(x,y: real; number: real; c: GColor) := TextOut(x,y,'' + number,c);

procedure DrawTextHelper(var x, y: real; w, h: real; text: string; align: Alignment := Alignment.Center);
begin
  if h<0 then
  begin
    h := -h;
    y -= h;
  end;
  if w<0 then
  begin
    w := -w;
    x -= w;
  end;
  var sz := TextSize(text);
  var dw,dh: real;
  if CurrentCoordType = ScreenCoords then
    (dw,dh) := ((w-sz.Width)/2,(h-sz.Height)/2)
  else 
  begin
    var (szw,szh) := (sz.Width/GlobalScale,sz.Height/GlobalScale);
    (dw,dh) := ((w-szw)/2,(h-szh)/2);
    dh := h-dh;
  end;
//  Println(dw,dh);
  case align of
    Alignment.LeftTop: {ничего};
    Alignment.LeftCenter: y += dh;
    Alignment.LeftBottom: y += 2*dh;
    Alignment.CenterTop: x += dw;
    Alignment.Center: begin x += dw; y += dh; end;
    Alignment.CenterBottom: begin x += dw; y += 2*dh; end;
    Alignment.RightTop: x += 2*dw;
    Alignment.RightCenter: begin x += 2*dw; y += dh; end;
    Alignment.RightBottom: begin x += 2*dw; y += 2*dh; end;
  end;
  if CurrentCoordType = MathematicalCoords then
    case align of
      Alignment.LeftTop, Alignment.CenterTop, Alignment.RightTop: y += h;
      Alignment.LeftBottom, Alignment.CenterBottom, Alignment.RightBottom: y -= h;
    end;
end;
/// Выводит строку в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; text: string; align: Alignment := Alignment.Center);
begin
  DrawTextHelper(x, y, w, h, text,align);
  TextOutHelper(x,y,text)
end;
/// Выводит строку в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; text: string; c: GColor; align: Alignment := Alignment.Center);
begin
  DrawTextHelper(x, y, w, h,text,align);
  TextOutHelper(x,y,text,c)
end;
/// Выводит целое в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: integer; align: Alignment := Alignment.Center) := DrawText(x, y, w, h, '' + number,align);
/// Выводит вещественное в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: real; align: Alignment := Alignment.Center) := DrawText(x, y, w, h, '' + number,align);
/// Выводит строку в прямоугольник
procedure DrawText(r: GRect; text: string; align: Alignment := Alignment.Center) := DrawText(r.x,r.y,r.Width,r.Height,text,align);
/// Выводит целое в прямоугольник
procedure DrawText(r: GRect; number: integer; align: Alignment := Alignment.Center) := DrawText(r.x,r.y,r.Width,r.Height,number,align);
/// Выводит вещественное в прямоугольник
procedure DrawText(r: GRect; number: real; align: Alignment := Alignment.Center) := DrawText(r.x,r.y,r.Width,r.Height,number,align);
/// Выводит целое в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: integer; c: GColor; align: Alignment := Alignment.Center) := DrawText(x, y, w, h, '' + number,c,align);
/// Выводит вещественное в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: real; c: GColor; align: Alignment := Alignment.Center) := DrawText(x, y, w, h, '' + number,c,align);
/// Выводит строку в прямоугольник
procedure DrawText(r: GRect; text: string; c: GColor; align: Alignment := Alignment.Center) := DrawText(r.x,r.y,r.Width,r.Height,text,c,align);
/// Выводит целое в прямоугольник
procedure DrawText(r: GRect; number: integer; c: GColor; align: Alignment := Alignment.Center) := DrawText(r.x,r.y,r.Width,r.Height,number,c,align);
/// Выводит вещественное в прямоугольник
procedure DrawText(r: GRect; number: real; c: GColor; align: Alignment := Alignment.Center) := DrawText(r.x,r.y,r.Width,r.Height,number,c,align);

function ConvertAlign(align: Alignment): Alignment;
begin
  Result := align;
  case align of
    Alignment.LeftTop: Result := Alignment.RightBottom;
    Alignment.LeftCenter: Result := Alignment.RightCenter;
    Alignment.LeftBottom: Result := Alignment.RightTop;
    Alignment.CenterTop: Result := Alignment.CenterBottom;
    Alignment.CenterBottom: Result := Alignment.CenterTop;
    Alignment.RightTop: Result := Alignment.LeftBottom;
    Alignment.RightCenter: Result := Alignment.LeftCenter;
    Alignment.RightBottom: Result := Alignment.LeftTop;
  end;
end;

procedure TextOut(x, y: real; text: string; align: Alignment := Alignment.RightBottom) := DrawText(x, y, 0, 0, text,ConvertAlign(align));
procedure TextOut(x, y: real; text: string; c: GColor; align: Alignment := Alignment.RightBottom) := DrawText(x, y, 0, 0, text, c,ConvertAlign(align));
procedure TextOut(x, y: real; text: integer; align: Alignment := Alignment.RightBottom) := TextOut(x, y, ''+text,align);
procedure TextOut(x, y: real; text: integer; c: GColor; align: Alignment := Alignment.RightBottom) := TextOut(x, y, ''+text, c,align);
procedure TextOut(x, y: real; text: real; align: Alignment := Alignment.RightBottom) := TextOut(x, y, ''+text,align);
procedure TextOut(x, y: real; text: real; c: GColor; align: Alignment := Alignment.RightBottom) := TextOut(x, y, ''+text, c,align);


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

procedure WindowTypeClearP := begin Host.children.Clear; CountVisuals := 0; end;
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

function XMin := -XOrigin/GlobalScale;
function XMax := (Window.Width-XOrigin)/GlobalScale;
function YMin := -(Window.Height-YOrigin)/GlobalScale;
function YMax := YOrigin/GlobalScale;

procedure DrawGridP;
begin
  if CurrentCoordType = ScreenCoords then
    raise new Exception('Рисование координатной сетки возможно только в математическом режиме');
  
  var xfrom := Round(XMin);
  var xto := Round(XMax);
  var yfrom := Round(YMin);
  var yto := Round(YMax);
  
  Range(yfrom,yto).ForEach(y->Line(XMin,y,XMax,y,Colors.LightGray));
  Range(xfrom,xto).ForEach(x->Line(x,YMin,x,YMax,Colors.LightGray));
  Line(XMin,0,XMax,0);
  Line(0,YMin,0,YMax);
end;
procedure DrawGrid := Invoke(DrawGridP);

procedure SetMathematicCoordsScaleP(x0,y0,scale: real);
begin
  CurrentCoordType := MathematicalCoords;
  XOrigin := x0;
  YOrigin := y0;
  GlobalScale := scale;
  var m: Transform := new MatrixTransform(scale,0,0,-scale,x0,y0);
  Host.RenderTransform := m;
  Pen.Width := Pen.Width / scale;
end;
procedure SetMathematicCoordsScale(x0,y0,scale: real) := Invoke(SetMathematicCoordsScaleP,x0,y0,scale);

procedure SetMathematicCoordsP(x1,x2: real; drawcoords: boolean);
begin
  Window.Clear;
  CurrentCoordType := MathematicalCoords;
  // x1 0 x2
  // 0 x0 Window.Width
  GlobalScale := Window.Width/(x2-x1);
  XOrigin := -x1*GlobalScale;
  YOrigin := Window.Height/2;
  //Print(XOrigin,YOrigin,GlobalScale);
  SetMathematicCoordsScaleP(XOrigin,YOrigin,GlobalScale);
  if drawcoords then
    DrawGridP
end;
procedure SetMathematicCoordsP1(x1,x2,ymin: real; drawcoords: boolean);
begin
  Window.Clear;
  CurrentCoordType := MathematicalCoords;
  // x1 0 x2
  // 0 x0 Window.Width
  GlobalScale := Window.Width/(x2-x1);
  XOrigin := -x1*GlobalScale;
  //Print(XOrigin,YOrigin,GlobalScale);
  // -ymin*scale - сколько надо отступить от низа окна
  // Window.Height + ymin*scale
  YOrigin := Window.Height + ymin*GlobalScale;
  SetMathematicCoordsScaleP(XOrigin,YOrigin,GlobalScale);
  if drawcoords then
    DrawGridP
end;
procedure SetMathematicCoords(x1: real := -10; x2: real := 10; drawcoords: boolean := true) := Invoke(SetMathematicCoordsP,x1,x2,drawcoords);
procedure SetMathematicCoords(x1,x2,ymin: real; drawcoords: boolean := true) := Invoke(SetMathematicCoordsP1,x1,x2,ymin,drawcoords);

procedure SetStandardCoordsP;
begin
  Window.Clear;
  CurrentCoordType := ScreenCoords;
  XOrigin := 0;
  YOrigin := 0;
  GlobalScale := 1;
  Host.RenderTransform := Transform.Identity;
  Pen.Width := 1;
end;
procedure SetStandardCoords := Invoke(SetStandardCoordsP);


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

var OnDraw: procedure := nil;

var FrameRate := 60; // кадров в секунду. Можно меньше!
var LastUpdatedTime := new System.TimeSpan(integer.MinValue); 

procedure RenderFrame(o: Object; e: System.EventArgs);
begin
  // Надо вычесть старое время
  if OnDraw<>nil then
  begin
    var e1 := RenderingEventArgs(e).RenderingTime;
    var dt := e1 - LastUpdatedTime;
    var delta := 1000/Framerate; // через какое время обновлять
    if dt.TotalMilliseconds < delta then
      exit
    else LastUpdatedTime := e1;  
    Window.Clear;
    OnDraw();
  end;  
end;

procedure BeginFrameBasedAnimation(Draw: procedure; frate: integer := 60);
begin
  OnDraw := Draw;
  FrameRate := frate;
end;

procedure EndFrameBasedAnimation;
begin
  OnDraw := nil;
  FrameRate := 60;
end;  

procedure InitForm0;
begin
  app := new Application();
  
  app.Dispatcher.UnhandledException += (o,e) -> Println(e);
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
  
  //MainWindow.Show;
  MainWindow.Closed += procedure(sender,e) -> begin Halt; end;
  MainWindow.MouseDown += SystemOnMouseDown;
  MainWindow.MouseUp += SystemOnMouseUp;
  MainWindow.MouseMove += SystemOnMouseMove;
  MainWindow.KeyDown += SystemOnKeyDown;
  MainWindow.KeyUp += SystemOnKeyUp;
  
  mre.Set();

  CompositionTarget.Rendering += RenderFrame;
  
  app.Run(MainWindow);
end;

begin
  MainFormThread := new System.Threading.Thread(InitForm0);
  MainFormThread.SetApartmentState(ApartmentState.STA);
  MainFormThread.Start;
  
  mre.WaitOne; // Основная программа не начнется пока не будут инициализированы все компоненты приложения
end.