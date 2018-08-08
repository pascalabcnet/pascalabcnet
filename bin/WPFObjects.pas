// Copyright (©) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///Модуль векторных графических объектов на основе WPF
unit WPFObjects;

interface

uses GraphWPFBase;

uses System.Windows; 
uses System.Windows.Controls;
uses System.Windows.Controls.Primitives;
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

procedure SetLeft(Self: UIElement; l: integer);
procedure SetTop(Self: UIElement; t: integer); 

type 
  /// Тип клавиши
  Key = System.Windows.Input.Key;
  /// Цветовые константы
  Colors = System.Windows.Media.Colors;
  /// Тип цвета
  Color = System.Windows.Media.Color;
  /// Тип цвета
  GColor = System.Windows.Media.Color;
  /// Тип прямоугольника
  GRect = System.Windows.Rect;
  GWindow = System.Windows.Window;
  GPen = System.Windows.Media.Pen;
  GPoint = System.Windows.Point;
  GBrush = System.Windows.Media.Brush;
  /// Тип стиля шрифта
  FontStyle = (Normal,Bold,Italic,BoldItalic);
  
type 
  MyVisualHost = class(Canvas) // мб Canvas - тогда можно размещать другие элементы!
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

var host: Canvas;

type
  GraphWindowType = class
  private
    function GetTop: real;
    function GetLeft: real;
    function GetWidth: real;
    function GetHeight: real;
  public  
    /// Отступ графического окна от левого края главного окна  
    property Left: real read GetLeft;
    /// Отступ графического окна от верхнего края главного окна   
    property Top: real read GetTop;
    /// Ширина графического окна
    property Width: real read GetWidth;
    /// Высота графического окна
    property Height: real read GetHeight;
  end;
  
type
  ObjectWPF = class
  private
    ob: FrameworkElement;
    procedure InitOb(x,y,w,h: real; o: FrameworkElement);
    begin
      ob := o;
      ob.Width := w;
      ob.Height := h;
      MoveTo(x,y);
      host.Children.Add(ob);
    end;
  public
    property X: real read InvokeReal(()->Canvas.GetLeft(ob)) write Invoke(procedure->Canvas.SetLeft(ob,value)); 
    property Y: real read InvokeReal(()->Canvas.GetTop(ob)) write Invoke(procedure->Canvas.SetTop(ob,value)); 
    property Width: real read InvokeReal(()->ob.Width) write Invoke(procedure->ob.Width := value); virtual;
    property Height: real read InvokeReal(()->ob.Height) write Invoke(procedure->ob.Height := value); virtual;
    procedure MoveTo(x,y: real) := (Self.X,Self.Y) := (x,y);
    procedure MoveOn(dx,dy: real) := MoveTo(x+dx,y+dy);
  end;
  
  ShapeWPF = class(ObjectWPF)
  private
    function Element := ob as Shape;
    procedure InitOb1(x,y,w,h: real; c: Color; o: FrameworkElement);
    begin
      InitOb(x,y,w,h,o);
      Color := c;
    end;
  public
    procedure EF(value: GColor) := Element.Fill := new SolidColorBrush(Value);
    property Color: GColor 
      read Invoke&<GColor>(()->(Element.Fill as SolidColorBrush).Color) 
      write Invoke(EF,value);
    procedure ES(value: GColor) := Element.Stroke := new SolidColorBrush(Value);
    property BorderColor: GColor 
      read Invoke&<GColor>(()->(Element.Stroke as SolidColorBrush).Color) 
      write Invoke(ES,value);
    procedure EST(value: real) := Element.StrokeThickness := Value;
    property BorderWidth: real 
      read InvokeReal(()->Element.StrokeThickness)
      write Invoke(EST,value);
  end;
  
  EllipseWPF = class(ShapeWPF)
  private
    procedure InitOb2(x,y,w,h: real; c: GColor) := InitOb1(x,y,w,h,c,new Ellipse());
  public
    constructor (x,y,w,h: real; c: GColor) := Invoke(InitOb2,x,y,w,h,c);
  end;

  CircleWPF = class(ShapeWPF)
  private
    procedure InitOb2(x,y,r: real; c: GColor) := InitOb1(x-r,y-r,2*r,2*r,c,new Ellipse());
  public
    constructor (x,y,r: real; c: GColor) := Invoke(InitOb2,x,y,r,c);
    procedure WT(value: real) := (ob.Width,ob.Height) := (value,value);
    property Width: real 
      read InvokeReal(()->ob.Width) 
      write Invoke(WT,Value); override;
    procedure HT(value: real) := (ob.Width,ob.Height) := (value,value);
    property Height: real 
      read InvokeReal(()->ob.Height) 
      write Invoke(HT,Value); override;
  end;

  // RectangleABC
  RectangleWPF = class(ShapeWPF)
    procedure InitOb2(x,y,w,h: real; c: GColor);
    begin
      var rr := new Rectangle();
      InitOb1(x,y,w,h,c,rr);
    end;
  public
    constructor (x,y,w,h: real; c: GColor) := Invoke(InitOb2,x,y,w,h,c);
  end;
  
type
  SquareWPF = class(CircleWPF)
    procedure InitOb2(x,y,w: real; c: GColor) := InitOb1(x,y,w,w,c,new Rectangle());
  public
    constructor (x,y,w: real; c: GColor) := Invoke(InitOb2,x,y,w,c);
  end;
  
  RoundRectWPF = class(ShapeWPF)
    procedure InitOb2(x,y,w,h,r: real; c: GColor);
    begin
      var rr := new Rectangle();
      rr.RadiusX := r;
      rr.RadiusY := r;
      InitOb1(x,y,w,h,c,rr);
    end;
  public
    constructor (x,y,w,h,r: real; c: GColor) := Invoke(InitOb2,x,y,w,h,r,c);
  end;
  
  RoundSquareWPF = class(CircleWPF)
    procedure InitOb2(x,y,w,r: real; c: GColor);
    begin
      var rr := new Rectangle();
      rr.RadiusX := r;
      rr.RadiusY := r;
      InitOb1(x,y,w,w,c,rr);
    end;
  public
    constructor (x,y,w,r: real; c: GColor) := Invoke(InitOb2,x,y,w,r,c);
  end;

  PolygonWPF = class(ShapeWPF)
  private
    procedure InitOb2(c: GColor; pp: array of Point) := InitOb1(0,0,real.NaN,real.NaN,c,CreatePolygon(pp));
    function CreatePolygon(pp: array of Point): Polygon;
    begin
      var p := new Polygon();
      p.Points := new PointCollection(pp);
      Result := p;
    end;
  public
    constructor (pp: array of Point; c: GColor) := Invoke(InitOb2,c);
  end;

/// Главное окно
var Window: WindowType;
/// Графическое окно
var GraphWindow: GraphWindowType;

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
  /// Событие изменения размера графического окна
  OnResize: procedure;

/// Возвращает цвет по красной, зеленой и синей составляющей (в диапазоне 0..255)
function RGB(r,g,b: byte): Color;
/// Возвращает цвет по красной, зеленой и синей составляющей и параметру прозрачности (в диапазоне 0..255)
function ARGB(a,r,g,b: byte): Color;
/// Возвращает случайный цвет
function RandomColor: Color;
/// Возвращает случайный цвет
function clRandom: Color;
/// Возвращает точку с координатами (x,y)
function Pnt(x,y: real): GPoint;
/// Возвращает прямоугольник с координатами угла (x,y), шириной w и высотой h
function Rect(x,y,w,h: real): GRect;
/// Возвращает однотонную цветную кисть, заданную цветом
function ColorBrush(c: Color): GBrush;


implementation

function RGB(r,g,b: byte) := Color.Fromrgb(r, g, b);
function ARGB(a,r,g,b: byte) := Color.FromArgb(a, r, g, b);
function RandomColor := RGB(PABCSystem.Random(256), PABCSystem.Random(256), PABCSystem.Random(256));
function clRandom := RandomColor();
function Pnt(x,y: real) := new Point(x,y);
function Rect(x,y,w,h: real) := new System.Windows.Rect(x,y,w,h);
function ColorBrush(c: Color) := new SolidColorBrush(c);

procedure InvokeVisual(d: System.Delegate; params args: array of object);
begin
  app.Dispatcher.Invoke(d,DispatcherPriority.Background,args);
end;

function operator implicit(Self: (integer, integer)): Point; extensionmethod := new Point(Self[0], Self[1]);
function operator implicit(Self: (integer, real)): Point; extensionmethod := new Point(Self[0], Self[1]);
function operator implicit(Self: (real, integer)): Point; extensionmethod := new Point(Self[0], Self[1]);
function operator implicit(Self: (real, real)): Point; extensionmethod := new Point(Self[0], Self[1]);

function operator implicit(Self: array of (real, real)): array of Point; extensionmethod := 
  Self.Select(t->new Point(t[0],t[1])).ToArray;
function operator implicit(Self: array of (integer, integer)): array of Point; extensionmethod := 
  Self.Select(t->new Point(t[0],t[1])).ToArray;


///---- Helpers
procedure SetLeft(Self: UIElement; l: integer); extensionmethod := Canvas.SetLeft(Self,l);

procedure SetTop(Self: UIElement; t: integer); extensionmethod := Canvas.SetTop(Self,t);

procedure SetLeft(Self: UIElement; l: integer) := Self.SetLeft(l);
procedure SetTop(Self: UIElement; t: integer) := Self.SetTop(t);


{procedure MoveTo(Self: UIElement; l,t: integer); extensionmethod;
begin
  Canvas.SetLeft(Self,l);
  Canvas.SetTop(Self,t);
end;}

function GraphWindowTypeGetLeftP: real;
begin
  Result := 0;
  foreach var p in MainDockPanel.Children do
    if (p is FrameworkElement) and (p<>host) then
    begin
      var d := DockPanel.GetDock(FrameworkElement(p));
      if d=Dock.Left then
        Result += FrameworkElement(p).Width;
    end;
end;

function GraphWindowTypeGetTopP: real;
begin
  Result := 0;
  foreach var p in MainDockPanel.Children do
    if (p is FrameworkElement) and (p<>host) then
    begin
      var d := DockPanel.GetDock(FrameworkElement(p));
      if d=Dock.Top then
        Result += FrameworkElement(p).Height;
    end;
end;

function GraphWindowType.GetLeft := InvokeReal(GraphWindowTypeGetLeftP);
function GraphWindowType.GetTop := InvokeReal(GraphWindowTypeGetTopP);

function GraphWindowTypeGetWidthP: real;
begin
  {if host.DataContext = nil then
    Result := 0
  else Result := Size(host.DataContext).Width;}
  Result := Window.Width;
  foreach var p in MainDockPanel.Children do
    if (p is FrameworkElement) and (p<>host) then
    begin
      var d := DockPanel.GetDock(FrameworkElement(p));
      if (d=Dock.Left) or (d=Dock.Right) then
        Result -= FrameworkElement(p).Width;
    end;
end;
function GraphWindowType.GetWidth := InvokeReal(GraphWindowTypeGetWidthP);

function GraphWindowTypeGetHeightP: real;
begin
  {if host.DataContext = nil then
    Result := 0
  else Result := Size(host.DataContext).Height;}
  Result := Window.Height;
  foreach var p in MainDockPanel.Children do
    if (p is FrameworkElement) and (p<>host) then
    begin
      var d := DockPanel.GetDock(FrameworkElement(p));
      if (d=Dock.Top) or (d=Dock.Bottom) then
        Result -= FrameworkElement(p).Height;
    end;
end;
function GraphWindowType.GetHeight := InvokeReal(GraphWindowTypeGetHeightP);

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
    
procedure SystemOnResize(sender: Object; e: SizeChangedEventArgs) := 
  if OnResize<>nil then
    OnResize();

var mre := new ManualResetEvent(false);

type 
GraphWPFWindow = class(GMainWindow)
public
  procedure InitMainGraphControl; override;
  begin
    host := new Canvas();
    //host.ClipToBounds := True;
    host.SizeChanged += (s,e) ->
    begin
      var sz := e.NewSize;
      host.DataContext := sz;
    end;
    // Всегда последнее
    var g := Content as DockPanel;
    g.children.Add(host);
  end;

  procedure InitWindowProperties; override;
  begin
    Title := 'WPF объекты';
    var (w,h) := (800,600);
    
    (Width, Height) := (w + wplus, h + hplus);
    WindowStartupLocation := System.Windows.WindowStartupLocation.CenterScreen;
  end;

  procedure InitGlobals; override;
  begin
    Window := new WindowType;
    GraphWindow := new GraphWindowType;
  end;
  
  procedure InitHandlers; override;
  begin
    Closed += procedure(sender,e) -> begin Halt; end;
    MouseDown += SystemOnMouseDown;
    MouseUp += SystemOnMouseUp;
    MouseMove += SystemOnMouseMove;
    KeyDown += SystemOnKeyDown;
    KeyUp += SystemOnKeyUp;
    SizeChanged += SystemOnResize;
    
    Loaded += (o,e) -> mre.Set();

    {PreviewMouseDown += (o,e) -> SystemOnMouseDown(o,e);  
    PreviewMouseUp += (o,e) -> SystemOnMouseUp(o,e);  
    PreviewMouseMove += (o,e) -> SystemOnMouseMove(o,e);  
  
    PreviewKeyDown += (o,e)-> SystemOnKeyDown(o,e);
    PreviewKeyUp += (o,e)-> SystemOnKeyUp(o,e);

    Closed += procedure(sender, e) -> begin Halt; end;}
  end;

end;

procedure InitApp;
begin
  app := new Application;
  
  app.Dispatcher.UnhandledException += (o, e) -> begin
    Println(e.Exception.Message); 
    if e.Exception.InnerException<>nil then
      Println(e.Exception.InnerException.Message); 
    halt; 
  end;
  
  MainWindow := new GraphWPFWindow;

  mre.Set();
  
  app.Run(MainWindow);
end;

procedure InitMainThread;
begin
  var MainFormThread := new System.Threading.Thread(InitApp);
  MainFormThread.SetApartmentState(ApartmentState.STA);
  MainFormThread.Start;
  
  mre.WaitOne; // Основная программа не начнется пока не будут инициализированы все компоненты приложения
end;

var
  ///--
  __initialized := false;

var
  ///--
  __finalized := false;

procedure __InitModule;
begin
  InitMainThread;
end;

///--
procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    __InitModule;
  end;
end;

///--
procedure __FinalizeModule__;
begin
  if not __finalized then
  begin
    __finalized := true;
  end;
end;

initialization
  __InitModule;

finalization  
end. 