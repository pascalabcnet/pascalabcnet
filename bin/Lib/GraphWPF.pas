// Copyright (©) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///Модуль графики
unit GraphWPF;

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

var CountVisuals := 0;

{procedure Invoke(d: System.Delegate; params args: array of object);
procedure Invoke(d: ()->());
function Invoke<T>(d: Func0<T>): T;
function InvokeReal(f: ()->real): real;
function InvokeString(f: ()->string): string;}

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
  
  ///!#
  BrushType = class
  private
    c := Colors.White;
    function BrushConstruct := new SolidColorBrush(c);
  public  
    /// Цвет кисти
    property Color: GColor read c write c;
  end;
  
  ///!#
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
    /// Цвет пера
    property Color: GColor read c write c;
    /// Ширина пера
    property Width: real read th write th;
    /// Текущая координата X пера
    property X: real read fx;
    /// Текущая координата Y пера
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
    /// Цвет шрифта
    property Color: GColor read c write c;
    /// Имя шрифта
    property Name: string read GetName write SetName;
    /// Размер шрифта в единицах по 1/96 дюйма
    property Size: real read sz write sz;
    /// Стиль шрифта
    property Style: FontStyle write SetFS;
  end;
  
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
    /// Сохраняет содержимое графического окна в файл с именем fname
    procedure Save(fname: string);
    /// Восстанавливает содержимое графического окна из файла с именем fname
    procedure Load(fname: string);
    /// Заполняет содержимое графического окна обоями из файла с именем fname
    procedure Fill(fname: string);
  end;
  
  // Специфический тип окна для модуля GraphWPF
  WindowTypeWPF = class(WindowType)
  public
    /// Сохраняет содержимое графического окна в файл с именем fname
    procedure Save(fname: string);
    /// Восстанавливает содержимое графического окна из файла с именем fname
    procedure Load(fname: string);
    /// Очищает графическое окно белым цветом
    procedure Clear; override;
  end;

  
  /// Виды системы координат
  CoordType = (MathematicalCoords,StandardCoords);
  /// Константы выравнивания текста относительно точки
  Alignment = (LeftTop,CenterTop,RightTop,LeftCenter,Center,RightCenter,LeftBottom,CenterBottom,RightBottom);

/// Рисует эллипс с центром в точке (x,y) и радиусами rx и ry
procedure Ellipse(x,y,rx,ry: real);
/// Рисует контур эллипса с центром в точке (x,y) и радиусами rx и ry
procedure DrawEllipse(x,y,rx,ry: real);
/// Рисует внутренность эллипса с центром в точке (x,y) и радиусами rx и ry
procedure FillEllipse(x,y,rx,ry: real);
/// Рисует эллипс с центром в точке (x,y), радиусами rx и ry и цветом внутренности c
procedure Ellipse(x,y,rx,ry: real; c: Color);
/// Рисует контур эллипса с центром в точке (x,y), радиусами rx и ry и цветом c
procedure DrawEllipse(x,y,rx,ry: real; c: Color);
/// Рисует внутренность эллипса с центром в точке (x,y), радиусами rx и ry и цветом c
procedure FillEllipse(x,y,rx,ry: real; c: Color);

/// Рисует окружность с центром в точке (x,y) и радиусом r
procedure Circle(x,y,r: real);
/// Рисует контур окружности с центром в точке (x,y) и радиусом r
procedure DrawCircle(x,y,r: real);
/// Рисует внутренность окружности с центром в точке (x,y) и радиусом r
procedure FillCircle(x,y,r: real);
/// Рисует окружность с центром в точке (x,y), радиусом r и цветом c
procedure Circle(x,y,r: real; c: Color);
/// Рисует контур окружности с центром в точке (x,y), радиусом r и цветом c
procedure DrawCircle(x,y,r: real; c: Color);
/// Рисует внутренность окружности с центром в точке (x,y), радиусом r и цветом c
procedure FillCircle(x,y,r: real; c: Color);

/// Рисует прямоугольник с координатами вершин (x,y) и (x+w,y+h)
procedure Rectangle(x,y,w,h: real);
/// Рисует контур прямоугольника с координатами вершин (x,y) и (x+w,y+h)
procedure DrawRectangle(x,y,w,h: real);
/// Рисует внутренность прямоугольника с координатами вершин (x,y) и (x+w,y+h)
procedure FillRectangle(x,y,w,h: real);
/// Рисует прямоугольник с координатами вершин (x,y) и (x+w,y+h) цветом c
procedure Rectangle(x,y,w,h: real; c: Color);
/// Рисует контур прямоугольника с координатами вершин (x,y) и (x+w,y+h) цветом c
procedure DrawRectangle(x,y,w,h: real; c: Color);
/// Рисует внутренность прямоугольника с координатами вершин (x,y) и (x+w,y+h) цветом c
procedure FillRectangle(x,y,w,h: real; c: Color);

/// Рисует дугу окружности с центром в точке (x,y) и радиусом r, заключенную между двумя лучами, образующими углы angle1 и angle2 с осью OX
procedure Arc(x, y, r, angle1, angle2: real);
/// Рисует дугу окружности с центром в точке (x,y) и радиусом r, заключенную между двумя лучами, образующими углы angle1 и angle2 с осью OX, цветом c
procedure Arc(x, y, r, angle1, angle2: real; c: Color);

/// Рисует сектор окружности с центром в точке (x,y) и радиусом r, заключенный между двумя лучами, образующими углы angle1 и angle2 с осью OX
procedure Sector(x, y, r, angle1, angle2: real);
/// Рисует сектор окружности с центром в точке (x,y) и радиусом r, заключенный между двумя лучами, образующими углы angle1 и angle2 с осью OX
procedure Pie(x, y, r, angle1, angle2: real);
/// Рисует контур сектора окружности с центром в точке (x,y) и радиусом r, заключенного между двумя лучами, образующими углы angle1 и angle2 с осью OX
procedure DrawSector(x, y, r, angle1, angle2: real);
/// Рисует внутренность сектора окружности с центром в точке (x,y) и радиусом r, заключенного между двумя лучами, образующими углы angle1 и angle2 с осью OX
procedure FillSector(x, y, r, angle1, angle2: real);
/// Рисует сектор окружности с центром в точке (x,y) и радиусом r, заключенный между двумя лучами, образующими углы angle1 и angle2 с осью OX, цветом c
procedure Sector(x, y, r, angle1, angle2: real; c: Color);
/// Рисует контур сектора окружности с центром в точке (x,y) и радиусом r, заключенного между двумя лучами, образующими углы angle1 и angle2 с осью OX, цветом c
procedure DrawSector(x, y, r, angle1, angle2: real; c: Color);
/// Рисует внутренность сектора окружности с центром в точке (x,y) и радиусом r, заключенного между двумя лучами, образующими углы angle1 и angle2 с осью OX, цветом c
procedure FillSector(x, y, r, angle1, angle2: real; c: Color);

/// Рисует отрезок прямой от точки (x,y) до точки (x1,y1)
procedure Line(x,y,x1,y1: real);
/// Рисует отрезок прямой от точки (x,y) до точки (x1,y1) цветом c
procedure Line(x,y,x1,y1: real; c: Color);
/// Устанавливает текущую позицию рисования в точку (x,y)
procedure MoveTo(x,y: real);
/// Рисует отрезок от текущей позиции до точки (x,y). Текущая позиция переносится в точку (x,y)
procedure LineTo(x,y: real);
/// Перемещает текущую позицию рисования на вектор (dx,dy)
procedure MoveRel(dx,dy: real);
/// Рисует отрезок от текущей позиции до точки, смещённой на вектор (dx,dy). Текущая позиция переносится в новую точку
procedure LineRel(dx,dy: real);
/// Перемещает текущую позицию рисования на вектор (dx,dy)
procedure MoveOn(dx,dy: real);
/// Рисует отрезок от текущей позиции до точки, смещённой на вектор (dx,dy). Текущая позиция переносится в новую точку
procedure LineOn(dx,dy: real);

/// Рисует ломаную, заданную массивом точек 
procedure PolyLine(points: array of Point);
/// Рисует ломаную заданную массивом точек и цветом
procedure PolyLine(points: array of Point; c: Color);

/// Рисует многоугольник, заданный массивом точек 
procedure Polygon(points: array of Point);
/// Рисует контур многоугольника, заданного массивом точек 
procedure DrawPolygon(points: array of Point);
/// Рисует внутренность многоугольника, заданного массивом точек 
procedure FillPolygon(points: array of Point);
/// Рисует многоугольник, заданный массивом точек и цветом
procedure Polygon(points: array of Point; c: Color);
/// Рисует контур многоугольника, заданного массивом точек и цветом 
procedure DrawPolygon(points: array of Point; c: GColor);
/// Рисует внутренность многоугольника, заданного массивом точек и цветом
procedure FillPolygon(points: array of Point; c: GColor);

/// Рисует изображение из файла fname в позиции (x,y)
procedure DrawImage(x,y: real; fname: string);
/// Рисует изображение из файла fname в позиции (x,y) размера w на h
procedure DrawImage(x,y,w,h: real; fname: string);
/// Рисует немасштабированное изображение из файла fname в позиции (x,y)
procedure DrawImageUnscaled(x,y: real; fname: string);
/// Выводит видеоиз файла fname в позицию (x,y)
procedure DrawVideo(x,y: real; fname: string);

/// Ширина изображения в пикселах
function ImageWidth(fname: string): integer;
/// Высота изображения в пикселах
function ImageHeight(fname: string): integer;
/// Размер изображения в пикселах
function ImageSize(fname: string): (integer,integer);

/// Ширина текста при выводе
function TextWidth(text: string): real;
/// Высота текста при выводе
function TextHeight(text: string): real;
/// Размер текста при выводе
function TextSize(text: string): Size;

/// Текущая кисть
var Brush: BrushType;
/// Текущее перо
var Pen: PenType;
/// Текущий шрифт
var Font: FontType;
/// Главное окно
var Window: WindowTypeWPF;
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
/// Возвращает однотонное цветное перо, заданное цветом
function ColorPen(c: Color): GPen;

/// Начать анимацию, основанную на кадре
procedure BeginFrameBasedAnimation(Draw: procedure; frate: integer := 60);
/// Начать анимацию, основанную на кадре
procedure BeginFrameBasedAnimation(Draw: procedure(frame: integer); frate: integer := 60);
/// Завершить анимацию, основанную на кадре
procedure EndFrameBasedAnimation;

/// Выводит строку в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; text: string; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит строку в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; text: string; c: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит целое в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: integer; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит вещественное в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: real; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит строку в прямоугольник
procedure DrawText(r: GRect; text: string; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит целое в прямоугольник
procedure DrawText(r: GRect; number: integer; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит вещественное в прямоугольник
procedure DrawText(r: GRect; number: real; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит целое в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: integer; c: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит вещественное в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: real; c: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит строку в прямоугольник
procedure DrawText(r: GRect; text: string; c: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит целое в прямоугольник
procedure DrawText(r: GRect; number: integer; c: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит вещественное в прямоугольник
procedure DrawText(r: GRect; number: real; c: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);

/// Выводит строку в позицию (x,y)
procedure TextOut(x, y: real; text: string; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
/// Выводит строку в позицию (x,y) цветом c
procedure TextOut(x, y: real; text: string; c: GColor; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
/// Выводит целое в позицию (x,y)
procedure TextOut(x, y: real; text: integer; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
/// Выводит целое в позицию (x,y) цветом c
procedure TextOut(x, y: real; text: integer; c: GColor; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
/// Выводит вещественное в позицию (x,y)
procedure TextOut(x, y: real; text: real; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
/// Выводит вещественное в позицию (x,y) цветом c
procedure TextOut(x, y: real; text: real; c: GColor; align: Alignment := Alignment.LeftTop; angle: real := 0.0);

/// Рисует график функции f, заданной на отрезке [a,b] по оси абсцисс и на отрезке [min,max] по оси ординат, в прямоугольнике, задаваемом координатами x1,y1,x2,y2, 
procedure DrawGraph(f: real -> real; a, b, min, max, x, y, w, h: real);
/// Рисует график функции f, заданной на отрезке [a,b] по оси абсцисс и на отрезке [min,max] по оси ординат, в прямоугольнике r
procedure DrawGraph(f: real -> real; a, b, min, max: real; r: GRect);  
/// Рисует график функции f, заданной на отрезке [a,b] по оси абсцисс и на отрезке [min,max] по оси ординат, на полное графическое окно
procedure DrawGraph(f: real -> real; a, b, min, max: real);
/// Рисует график функции f, заданной на отрезке [a,b], в прямоугольнике, задаваемом координатами x1,y1,x2,y2, 
procedure DrawGraph(f: real -> real; a, b: real; x, y, w, h: real);
/// Рисует график функции f, заданной на отрезке [a,b], в прямоугольнике r 
procedure DrawGraph(f: real -> real; a, b: real; r: GRect);
/// Рисует график функции f, заданной на отрезке [-5,5], в прямоугольнике r 
procedure DrawGraph(f: real -> real; r: GRect);
/// Рисует график функции f, заданной на отрезке [a,b], на полное графическое окно 
procedure DrawGraph(f: real -> real; a, b: real);
/// Рисует график функции f, заданной на отрезке [-5,5], на полное графическое окно  
procedure DrawGraph(f: real -> real);

procedure SetMathematicCoords(x1: real := -10; x2: real := 10; drawcoords: boolean := true);
procedure SetMathematicCoords(x1,x2,ymin: real; drawcoords: boolean := true);
procedure SetStandardCoords(scale: real := 1.0; x0: real := 0; y0: real := 0);
procedure SetStandardCoordsSharpLines(x0: real := 0; y0: real := 0);
procedure DrawGrid;

function XMin: real;
function XMax: real;
function YMin: real;
function YMax: real;

{procedure AddRightPanel(Width: real := 200; c: Color := Colors.LightGray);
procedure AddLeftPanel(Width: real := 200; c: Color := Colors.LightGray);
procedure AddTopPanel(Height: real := 100; c: Color := Colors.LightGray);
procedure AddBottomPanel(Height: real := 100; c: Color := Colors.LightGray);

procedure AddStatusBar(Height: real := 24);}

implementation

function RGB(r,g,b: byte) := Color.Fromrgb(r, g, b);
function ARGB(a,r,g,b: byte) := Color.FromArgb(a, r, g, b);
function RandomColor := RGB(PABCSystem.Random(256), PABCSystem.Random(256), PABCSystem.Random(256));
function clRandom := RandomColor();
function Pnt(x,y: real) := new Point(x,y);
function Rect(x,y,w,h: real) := new System.Windows.Rect(x,y,w,h);
function ColorBrush(c: Color) := new SolidColorBrush(c);
function ColorPen(c: Color) := new GPen(ColorBrush(c),Pen.Width);

procedure InvokeVisual(d: System.Delegate; params args: array of object);
begin
  if CountVisuals <= 1000000 then
    app.Dispatcher.Invoke(d,args)
  else
  begin
    //Print(CountVisuals);
    app.Dispatcher.Invoke(d,DispatcherPriority.Background,args);
    Sleep(10);    
  end;
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

{procedure MoveTo(Self: UIElement; l,t: integer); extensionmethod;
begin
  Canvas.SetLeft(Self,l);
  Canvas.SetTop(Self,t);
end;}
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

var Host: MyVisualHost;

var 
  XOrigin := 0.0;
  YOrigin := 0.0;
  GlobalScale := 1.0;
  CurrentCoordType: CoordType := StandardCoords;

{procedure ProbaAnimP;
begin
  var dv := Host.children[0] as DrawingVisual;
  
  var animation := new VectorAnimation();
  animation.From := new Vector(20,20);
  animation.To := new Vector(100,200);
  animation.Duration := System.TimeSpan.FromSeconds(5);
  dv.BeginAnimation(OffsetProperty, animation);
  
  var geo := dv.Drawing.Children[0] as GeometryDrawing;
  geo.Pen := ColorPen(Colors.Green);
  var rg := geo.Geometry as RectangleGeometry;
  
  //Print(geo.Geometry.);
  //geo.Brush := Brushes.Blue;
  dv.Offset := new Vector(200,40);
end;

procedure ProbaAnim := Invoke(ProbaAnimP);}

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
  //var f := Host.children[0] as DrawingVisual;
  {var geo := f.Drawing.Children[0] as GeometryDrawing;
  geo.Brush := Brushes.Blue;}
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

procedure TextPFull(x,y: real; text: string; angle,x0,y0: real);
begin
  var dc: DrawingContext;
  if CurrentCoordType = StandardCoords then
  begin
    dc := GetDC();
    var RT := new RotateTransform(angle,x0,y0);
    dc.PushTransform(RT);
    dc.DrawText(FormText(text),new Point(x,y));
    dc.Pop();
  end  
  else   
  begin
    var m := Host.RenderTransform.Value;
    var mt := new MatrixTransform(1/m.M11,0,0,1/m.M22,x,y);
    dc := GetDC();
    var RT := new RotateTransform(angle,x0,y0);
    dc.PushTransform(RT);
    dc.PushTransform(mt);
    dc.DrawText(FormText(text),new Point(0,0));
    dc.Pop();
    dc.Pop();
  end;
  //dc.DrawRectangle(Brushes.White,nil,new GRect(new Point(x,y),TextV.Create(text).TextSize));
  dc.Close();
end;

procedure TextPFull(x,y: real; text: string; angle,x0,y0: real; c: Color);
begin
  var dc: DrawingContext;
  if CurrentCoordType = StandardCoords then
  begin
    dc := GetDC();
    var RT := new RotateTransform(angle,x0,y0);
    dc.PushTransform(RT);
    dc.DrawText(FormTextC(text,c),new Point(x,y));
    dc.Pop();
  end  
  else   
  begin
    var m := Host.RenderTransform.Value;
    var mt := new MatrixTransform(1/m.M11,0,0,1/m.M22,x,y);
    dc := GetDC();
    var RT := new RotateTransform(angle,x0,y0);
    dc.PushTransform(RT);
    dc.PushTransform(mt);
    dc.DrawText(FormTextC(text,c),new Point(0,0));
    dc.Pop();
    dc.Pop();
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
procedure ArcPC(x, y, r, angle1, angle2: real; c: GColor) := ArcPFull(x, y, r, angle1, angle2, ColorPen(c));

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

procedure DrawTextP(x,y: real; text: string; angle,x0,y0: real) := TextPFull(x,y,text,angle,x0,y0);
procedure DrawTextPC(x,y: real; text: string; angle,x0,y0: real; c: GColor) := TextPFull(x,y,text,angle,x0,y0,c);

procedure EllipseNew(x,y,r1,r2: real) 
  := InvokeVisual(DrawGeometryP,VE.Create(()->EllipseGeometry.Create(Pnt(x,y),r1,r2)));

procedure Ellipse(x,y,rx,ry: real) := InvokeVisual(EllipseP,x,y,rx,ry);
procedure DrawEllipse(x,y,rx,ry: real) := InvokeVisual(DrawEllipseP,x,y,rx,ry);
procedure FillEllipse(x,y,rx,ry: real) := InvokeVisual(FillEllipseP,x,y,rx,ry);
procedure Ellipse(x,y,rx,ry: real; c: GColor) := InvokeVisual(EllipsePC,x,y,rx,ry,c);
procedure DrawEllipse(x,y,rx,ry: real; c: GColor) := InvokeVisual(DrawEllipsePC,x,y,rx,ry,c);
procedure FillEllipse(x,y,rx,ry: real; c: GColor) := InvokeVisual(FillEllipsePC,x,y,rx,ry,c);

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
procedure Arc(x, y, r, angle1, angle2: real; c: GColor) := InvokeVisual(ArcPC,x, y, r, angle1, angle2, c);

/// Рисует сектор окружности с центром в точке (x,y) и радиусом r, заключенной между двумя лучами, образующими углы angle1 и angle2 с осью OX (angle1 и angle2 – вещественные, задаются в градусах и отсчитываются против часовой стрелки)
procedure Sector(x, y, r, angle1, angle2: real) := InvokeVisual(SectorP,x, y, r, angle1, angle2);
procedure Pie(x, y, r, angle1, angle2: real) := InvokeVisual(SectorP,x, y, r, angle1, angle2);
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
function TextWidth(text: string) := InvokeReal(TextV.Create(text).TextWidth);
/// Высота текста при выводе
function TextHeight(text: string) := InvokeReal(TextV.Create(text).TextHeight);
/// Размер текста при выводе
function TextSize(text: string): Size := Invoke&<Size>(TextV.Create(text).TextSize);

procedure TextOutHelper(x,y: real; text: string; angle: real; x0,y0: real) := InvokeVisual(DrawTextP,x,y,text,angle,x0,y0);
//procedure TextOut(x,y: real; number: integer) := TextOut(x,y,'' + number);
//procedure TextOut(x,y: real; number: real) := TextOut(x,y,'' + number);
procedure TextOutHelper(x,y: real; text: string; angle: real; c: GColor; x0,y0: real) := InvokeVisual(DrawTextPC,x,y,text,angle,x0,y0,c);
//procedure TextOut(x,y: real; number: integer; c: GColor) := TextOut(x,y,'' + number,c);
//procedure TextOut(x,y: real; number: real; c: GColor) := TextOut(x,y,'' + number,c);

procedure DrawTextHelper(var x, y, x0, y0: real; w, h: real; text: string; align: Alignment := Alignment.Center);
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
  if CurrentCoordType = StandardCoords then
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
    Alignment.LeftCenter: begin y += dh; y0+=h/2 end;
    Alignment.LeftBottom: begin y += 2*dh; y0+=h end;
    Alignment.CenterTop: begin x += dw; x0 += w/2 end;
    Alignment.Center: begin x += dw; y += dh; y0 += h/2; x0 += w/2 end;
    Alignment.CenterBottom: begin x += dw; y += 2*dh; y0 += h; x0 += w/2 end;
    Alignment.RightTop: begin x += 2*dw; x0 += w; end;
    Alignment.RightCenter: begin x += 2*dw; y += dh; y0+=h/2; x0 += w; end;
    Alignment.RightBottom: begin x += 2*dw; y += 2*dh; y0+=h; x0 += w; end;
  end;
  if CurrentCoordType = MathematicalCoords then
    case align of
      Alignment.LeftTop, Alignment.CenterTop, Alignment.RightTop: begin y += h; y0 += h end;
      Alignment.LeftBottom, Alignment.CenterBottom, Alignment.RightBottom: begin y -= h; y0 -= h; end;
    end;
end;
/// Выводит строку в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; text: string; align: Alignment; angle: real);
begin
  var (x0,y0) := (x,y);
  DrawTextHelper(x, y, x0, y0, w, h, text, align);
  //FillCircle(x0,y0,0.1,Colors.Blue);
  //FillCircle(x,y,0.1,Colors.Red);
  TextOutHelper(x,y,text,angle,x0,y0)
end;
/// Выводит строку в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; text: string; c: GColor; align: Alignment; angle: real);
begin
  var (x0,y0) := (x,y);
  DrawTextHelper(x, y, x0, y0, w, h, text, align);
  //FillCircle(x0,y0,0.1,Colors.Blue);
  //FillCircle(x,y,0.1,Colors.Red);
  TextOutHelper(x,y,text,angle,c,x0,y0)
end;
/// Выводит целое в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: integer; align: Alignment; angle: real) := DrawText(x, y, w, h, '' + number,align,angle);
/// Выводит вещественное в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: real; align: Alignment; angle: real) := DrawText(x, y, w, h, '' + number,align,angle);
/// Выводит строку в прямоугольник
procedure DrawText(r: GRect; text: string; align: Alignment; angle: real) := DrawText(r.x,r.y,r.Width,r.Height,text,align,angle);
/// Выводит целое в прямоугольник
procedure DrawText(r: GRect; number: integer; align: Alignment; angle: real) := DrawText(r.x,r.y,r.Width,r.Height,number,align,angle);
/// Выводит вещественное в прямоугольник
procedure DrawText(r: GRect; number: real; align: Alignment; angle: real) := DrawText(r.x,r.y,r.Width,r.Height,number,align,angle);
/// Выводит целое в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: integer; c: GColor; align: Alignment; angle: real) := DrawText(x, y, w, h, '' + number,c,align,angle);
/// Выводит вещественное в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: real; c: GColor; align: Alignment; angle: real) := DrawText(x, y, w, h, '' + number,c,align,angle);
/// Выводит строку в прямоугольник
procedure DrawText(r: GRect; text: string; c: GColor; align: Alignment; angle: real) := DrawText(r.x,r.y,r.Width,r.Height,text,c,align,angle);
/// Выводит целое в прямоугольник
procedure DrawText(r: GRect; number: integer; c: GColor; align: Alignment; angle: real) := DrawText(r.x,r.y,r.Width,r.Height,number,c,align,angle);
/// Выводит вещественное в прямоугольник
procedure DrawText(r: GRect; number: real; c: GColor; align: Alignment; angle: real) := DrawText(r.x,r.y,r.Width,r.Height,number,c,align,angle);

{function ConvertAlign(align: Alignment): Alignment;
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
end;}

procedure TextOut(x, y: real; text: string; align: Alignment; angle: real) := DrawText(x, y, 0, 0, text,{ConvertAlign(}align{)},angle);
procedure TextOut(x, y: real; text: string; c: GColor; align: Alignment; angle: real) := DrawText(x, y, 0, 0, text, c,{ConvertAlign(}align{)},angle);
procedure TextOut(x, y: real; text: integer; align: Alignment; angle: real) := TextOut(x, y, ''+text,align,angle);
procedure TextOut(x, y: real; text: integer; c: GColor; align: Alignment; angle: real) := TextOut(x, y, ''+text, c,align,angle);
procedure TextOut(x, y: real; text: real; align: Alignment; angle: real) := TextOut(x, y, ''+text,align,angle);
procedure TextOut(x, y: real; text: real; c: GColor; align: Alignment; angle: real) := TextOut(x, y, ''+text, c,align,angle);


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
procedure DrawGraph(f: real-> real; a, b, min, max, x, y, w, h: real);
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

procedure DrawGraph(f: real -> real; a, b, min, max: real; r: GRect) := DrawGraph(f, a, b, min, max, r.X, r.Y, r.Width, r.Height);  

procedure DrawGraph(f: real -> real; a, b, min, max: real) := DrawGraph(f, a, b, min, max, Window.ClientRect);

procedure DrawGraph(f: real -> real; a, b: real; x, y, w, h: real);
begin
  var n := Round(w / 3);
  var q := Partition(a, b, n);
  DrawGraph(f, a, b, q.Min(f), q.Max(f), x, y, w, h)
end;

procedure DrawGraph(f: real -> real; a, b: real; r: GRect) := DrawGraph(f, a, b, r.X, r.Y, r.Width, r.Height);

procedure DrawGraph(f: real -> real; r: GRect) := DrawGraph(f, -5, 5, r);

procedure DrawGraph(f: real -> real; a, b: real) := DrawGraph(f, a, b, 0, 0, Window.Width - 1, Window.Height - 1);

procedure DrawGraph(f: real -> real) := DrawGraph(f, -5, 5);

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

procedure SaveWindowP(canvas: FrameworkElement; filename: string);
begin
  var (scalex,scaley) := ScaleToDevice;
  var (dpiX,dpiY) := (scalex * 96, scaley * 96);
  
  var sz := Size(host.DataContext);
  
  var bmp := new RenderTargetBitmap(Round(sz.Width*scalex), Round(sz.Height*scaley), dpiX, dpiY, PixelFormats.Pbgra32);
  
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

procedure GraphWindowType.Save(fname: string) := Invoke(SaveWindowP,host,fname);

procedure GraphWindowType.Load(fname: string) := DrawImageUnscaled(0,0,fname);

procedure GraphWindowType.Fill(fname: string);
begin
  //FillWindow(fname);
end;

procedure WindowTypeWPF.Save(fname: string) := GraphWindow.Save(fname);

procedure WindowTypeWPF.Load(fname: string) := GraphWindow.Load(fname);

procedure WindowTypeClearP := begin Host.children.Clear; CountVisuals := 0; end;
procedure WindowTypeWPF.Clear := Invoke(WindowTypeClearP);

function XMin := -XOrigin/GlobalScale;
function XMax := (Window.Width-XOrigin)/GlobalScale;
function YMin := -(Window.Height-YOrigin)/GlobalScale;
function YMax := YOrigin/GlobalScale;

procedure DrawGridP;
begin
  if CurrentCoordType = StandardCoords then
    raise new Exception('Рисование координатной сетки возможно только в математическом режиме');
  
  var xfrom := Round(XMin);
  var xto := Round(XMax);
  var yfrom := Round(YMin);
  var yto := Round(YMax);
  
  Range(yfrom,yto).ForEach(y->Line(XMin,y,XMax,y,Colors.LightGray));
  Range(xfrom,xto).ForEach(x->Line(x,YMin,x,YMax,Colors.LightGray));
  Line(XMin,0,XMax,0);
  Line(0,YMin,0,YMax);
  
  //Range(yfrom,yto).Where(y->y<>0).ForEach(y->TextOut(xmin+0.1,y,y,Alignment.LeftCenter));
  //Range(xfrom,xto).Where(x->x<>0).ForEach(x->TextOut(x,ymin+0.05,x,Alignment.CenterBottom));
end;
procedure DrawGrid := Invoke(DrawGridP);

var StandardCoordsPenWidthSave: real;

procedure SetMathematicCoordsScaleP(x0,y0,scale: real);
begin
  if CurrentCoordType = StandardCoords then
    StandardCoordsPenWidthSave := Pen.Width;
  CurrentCoordType := MathematicalCoords;
  XOrigin := x0;
  YOrigin := y0;
  GlobalScale := scale;
  var m: Transform := new MatrixTransform(scale,0,0,-scale,x0,y0);
  Host.RenderTransform := m;
  Pen.Width := StandardCoordsPenWidthSave / scale; 
end;
procedure SetMathematicCoordsScale(x0,y0,scale: real) := Invoke(SetMathematicCoordsScaleP,x0,y0,scale);

procedure SetMathematicCoordsP(x1,x2: real; drawcoords: boolean);
begin
  if CurrentCoordType = StandardCoords then
    StandardCoordsPenWidthSave := Pen.Width;
  Window.Clear;
  CurrentCoordType := MathematicalCoords;
  // x1 0 x2
  // 0 x0 Window.Width
  GlobalScale := Window.Width/(x2-x1);
  XOrigin := -x1*GlobalScale;
  YOrigin := Window.Height/2;
  SetMathematicCoordsScaleP(XOrigin,YOrigin,GlobalScale);
  if drawcoords then
    DrawGridP
end;
procedure SetMathematicCoordsP1(x1,x2,ymin: real; drawcoords: boolean);
begin
  if CurrentCoordType = StandardCoords then
    StandardCoordsPenWidthSave := Pen.Width;
  Window.Clear;
  CurrentCoordType := MathematicalCoords;
  // x1 0 x2
  // 0 x0 Window.Width
  GlobalScale := Window.Width/(x2-x1);
  XOrigin := -x1*GlobalScale;
  // -ymin*scale - сколько надо отступить от низа окна
  // Window.Height + ymin*scale
  YOrigin := Window.Height + ymin*GlobalScale;
  SetMathematicCoordsScaleP(XOrigin,YOrigin,GlobalScale);
  if drawcoords then
    DrawGridP
end;
procedure SetMathematicCoords(x1: real; x2: real; drawcoords: boolean) := Invoke(SetMathematicCoordsP,x1,x2,drawcoords);
procedure SetMathematicCoords(x1,x2,ymin: real; drawcoords: boolean) := Invoke(SetMathematicCoordsP1,x1,x2,ymin,drawcoords);

procedure SetStandardCoordsP(scale: real := 1.0; x0: real := 0; y0: real := 0);
begin
  Window.Clear;
  if CurrentCoordType = MathematicalCoords then
    Pen.Width := StandardCoordsPenWidthSave;
  CurrentCoordType := StandardCoords;
  XOrigin := x0;
  YOrigin := y0;
  GlobalScale := scale;
  var m: Transform := new MatrixTransform(scale,0,0,scale,x0,y0);
  Host.RenderTransform := m;
  //Pen.Width := Pen.Width * scale; // нет!
end;
procedure SetStandardCoords(scale,x0,y0: real) := Invoke(SetStandardCoordsP,scale,x0,y0);
procedure SetStandardCoordsSharpLinesP(x0,y0: real);
begin
  var (sx,sy) := ScaleToDevice;
  if Round(Pen.Width) mod 2 = 1 then
    SetStandardCoordsP(1/sx,(x0+0.5)/sx,(y0+0.5)/sy)
  else SetStandardCoordsP(1/sx,x0/sx,y0/sy) 
end; 
procedure SetStandardCoordsSharpLines(x0,y0: real) := Invoke(SetStandardCoordsSharpLinesP,x0,y0);

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

///----------------------------------------------------------------------

{procedure RenderFrame(s: Object; e: System.EventArgs);
begin
end;}

var OnDraw: procedure := nil;
var OnDraw1: procedure(frame: integer) := nil;

var FrameRate := 60; // кадров в секунду. Можно меньше!
var LastUpdatedTime := new System.TimeSpan(integer.MinValue); 

var FrameNum := 0;

procedure RenderFrame(o: Object; e: System.EventArgs);
begin
  if (OnDraw<>nil) or (OnDraw1<>nil) then
  begin
    var e1 := RenderingEventArgs(e).RenderingTime;
    var dt := e1 - LastUpdatedTime;
    var delta := 1000/Framerate; // через какое время обновлять
    if dt.TotalMilliseconds < delta then
      exit
    else LastUpdatedTime := e1;  
    FrameNum += 1;
    Window.Clear;
    CountVisuals := integer.MinValue; // чтобы не было паузы после 1000 объектов
  end;  
  if OnDraw<>nil then
    OnDraw() 
  else if OnDraw1<>nil then
    OnDraw1(FrameNum);
end;

procedure BeginFrameBasedAnimation(Draw: procedure; frate: integer);
begin
  FrameNum := 0;
  OnDraw := Draw;
  OnDraw1 := nil;
  FrameRate := frate;
end;

procedure BeginFrameBasedAnimation(Draw: procedure(frame: integer); frate: integer);
begin
  FrameNum := 0;
  OnDraw1 := Draw;
  OnDraw := nil;
  FrameRate := frate;
end;

procedure EndFrameBasedAnimation;
begin
  CountVisuals := 0;
  OnDraw := nil;
  OnDraw1 := nil;
  FrameRate := 60;
end;  

{procedure AddGraphWindow;
begin
  host := new MyVisualHost();
  host.ClipToBounds := True;
  host.SizeChanged += (s,e) ->
  begin
    var sz := e.NewSize;
    host.DataContext := sz;
  end;
  // Всегда последнее
  MainDockPanel.children.Add(host);
end;}

var mre := new ManualResetEvent(false);

type 
GraphWPFWindow = class(GMainWindow)
public
  procedure InitMainGraphControl; override;
  begin
    host := new MyVisualHost();
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
    Title := 'Графика WPF';
    var (w,h) := (800,600);
    
    (Width, Height) := (w + wplus, h + hplus);
    WindowStartupLocation := System.Windows.WindowStartupLocation.CenterScreen;
  end;

  procedure InitGlobals; override;
  begin
    Brush := new BrushType;
    Pen := new PenType;
    Font := new FontType;
    Window := new WindowTypeWPF;
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
    
    CompositionTarget.Rendering += RenderFrame;
    
    Loaded += (o,e) -> mre.Set();

    {PreviewMouseDown += (o,e) -> SystemOnMouseDown(o,e);  
    PreviewMouseUp += (o,e) -> SystemOnMouseUp(o,e);  
    PreviewMouseMove += (o,e) -> SystemOnMouseMove(o,e);  
  
    PreviewKeyDown += (o,e)-> SystemOnKeyDown(o,e);
    PreviewKeyUp += (o,e)-> SystemOnKeyUp(o,e);

    Closed += procedure(sender, e) -> begin Halt; end;}
  end;


  /// --- SystemKeyEvents
  (*procedure SystemOnKeyDown(sender: Object; e: System.Windows.Input.KeyEventArgs);
  begin
    if GraphWPF.OnKeyDown <> nil then
      GraphWPF.OnKeyDown(e.Key);
    e.Handled := True;
  end;
  
  procedure SystemOnKeyUp(sender: Object; e: System.Windows.Input.KeyEventArgs);
  begin
    if GraphWPF.OnKeyUp <> nil then
      GraphWPF.OnKeyUp(e.Key);
    e.Handled := True;
  end;    
  
  /// --- SystemMouseEvents
  procedure SystemOnMouseDown(sender: Object; e: System.Windows.Input.MouseButtonEventArgs);
  begin
    var mb := 0;
    var p := e.GetPosition(nil{hvp});
    if e.LeftButton = MouseButtonState.Pressed then
      mb := 1
    else if e.RightButton = MouseButtonState.Pressed then
      mb := 2;
    if GraphWPF.OnMouseDown <> nil then  
     GraphWPF.OnMouseDown(p.x, p.y, mb);
  end;
  
  procedure SystemOnMouseUp(sender: Object; e: MouseButtonEventArgs);
  begin
    var mb := 0;
    var p := e.GetPosition(nil{hvp});
    if e.LeftButton = MouseButtonState.Pressed then
      mb := 1
    else if e.RightButton = MouseButtonState.Pressed then
      mb := 2;
    if GraphWPF.OnMouseUp <> nil then  
      GraphWPF.OnMouseUp(p.x, p.y, mb);
  end;
  
  procedure SystemOnMouseMove(sender: Object; e: MouseEventArgs);
  begin
    var mb := 0;
    var p := e.GetPosition(nil{hvp});
    if e.LeftButton = MouseButtonState.Pressed then
      mb := 1
    else if e.RightButton = MouseButtonState.Pressed then
      mb := 2;
    if GraphWPF.OnMouseMove <> nil then  
      GraphWPF.OnMouseMove(p.x, p.y, mb);
  end;*)
  
end;


{procedure InitApp1;
begin
  app := new Application();
  
  app.Dispatcher.UnhandledException += (o,e) -> begin 
    Println(e.Exception.InnerException); 
    halt; 
  end;
  MainWindow := new GWindow;
  
  MainDockPanelF := new DockPanel;
  MainDockPanelF.LastChildFill := True;
  MainWindow.Content := MainDockPanelF; 

  AddGraphWindow;
  
  host := new MyVisualHost();
  MainWindow.Content := host;

  Brush := new BrushType;
  Pen := new PenType;
  Font := new FontType;
  Window := new WindowType;
  GraphWindow := new GraphWindowType;

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
  MainWindow.SizeChanged += SystemOnResize;
  
  CompositionTarget.Rendering += RenderFrame;
  
  MainWindow.Loaded += (o,e) -> mre.Set();

  app.Run(MainWindow);
end;}

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