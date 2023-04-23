// Copyright (©) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
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

//{{{doc: Начало секции 1 }}} 

type 
// -----------------------------------------------------
//>>     Типы модуля GraphWPF # GraphWPF types
// -----------------------------------------------------
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
  /// Тип окна
  GWindow = System.Windows.Window;
  /// Тип пера
  GPen = System.Windows.Media.Pen;
  /// Тип точки
  Point = System.Windows.Point;
  /// Тип точки
  GPoint = System.Windows.Point;
  /// Тип вектора
  Vector = System.Windows.Vector;
  /// Тип кисти
  GBrush = System.Windows.Media.Brush;
  /// Набор предопределенных кистей
  Brushes = System.Windows.Media.Brushes;
  /// Контекст рисования
  DrawingContext = System.Windows.Media.DrawingContext;
  /// Визуальный объект для отрисовки с помощью контекста рисования
  DrawingVisual = System.Windows.Media.DrawingVisual;
  /// Тип стиля шрифта
  FontStyle = (Normal,Bold,Italic,BoldItalic);

  /// Виды системы координат
  CoordType = (MathematicalCoords,StandardCoords);
  /// Константы выравнивания текста относительно точки
  Alignment = (LeftTop,CenterTop,RightTop,LeftCenter,Center,RightCenter,LeftBottom,CenterBottom,RightBottom);
  
//{{{--doc: Конец секции 1 }}} 

function GetFontFamily(name: string): FontFamily;

  
//{{{doc: Начало секции 2 }}} 

// -----------------------------------------------------
//>>     Класс BrushType # BrushType class
// -----------------------------------------------------
type
  ///!#
  /// Параметры рисования
  Parameters = static class
  public
    static ArrowSizeAlong: real := 10*4/5;
    static ArrowSizeAcross: real := 4*4/5;
  end;
  ///!#
  /// Тип кисти
  BrushType = class
  private
    c := Colors.White;
    function BrushConstruct := GetBrush(c);
  public  
    /// Цвет кисти
    property Color: GColor read c write c;
  end;
  
// -----------------------------------------------------
//>>     Класс PenType # PenType class
// -----------------------------------------------------
  ///!#
  /// Тип пера
  PenType = class
  private
    c: Color := Colors.Black;
    th: real := 1;
    fx,fy: real;
    rc: boolean := false;
  public
    ///--
    function PenConstruct: GPen;
    begin
      Result := new GPen(GetBrush(c),th);
      Result.LineJoin := PenLineJoin.Round;
      if rc then 
      begin
        Result.StartLineCap := PenLineCap.Round;
        Result.EndLineCap := PenLineCap.Round;
      end
      else
      begin
        Result.StartLineCap := PenLineCap.Flat;
        Result.EndLineCap := PenLineCap.Flat;
      end;
      Result.Freeze;
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
    /// Скругление пера на концах линий
    property RoundCap: boolean read rc write rc;
  end;

// -----------------------------------------------------
//>>     Класс FontType # FontType class
// -----------------------------------------------------
  ///!#
  /// Тип шрифта
  FontOptions = class
  private
    tf := new Typeface('Arial');
    sz: real := 14;
    c: GColor := Colors.Black;
    procedure SetNameP(s: string) := tf := new Typeface(GetFontFamily(s),FontStyles.Normal,FontWeights.Normal,FontStretches.Normal); 
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
      tf := new Typeface(GetFontFamily(Name),s,w,FontStretches.Normal); 
    end;
    procedure SetFS(fs: FontStyle) := Invoke(SetFSP,fs);
    property BrushClone: GBrush read GetBrush(c);
    property TypefaceClone: Typeface read tf;
  public
    /// Цвет шрифта
    property Color: GColor read c write c;
    /// Имя шрифта
    property Name: string read GetName write SetName;
    /// Размер шрифта в единицах по 1/96 дюйма
    property Size: real read sz write sz;
    /// Стиль шрифта
    property Style: FontStyle write SetFS;
    /// Декоратор стиля шрифта
    function WithStyle(fs: FontStyle): FontOptions;
    begin
      Result := new FontOptions;
      Result.sz := sz;
      Result.Color := c;
      Result.Style := fs;
    end;
    /// Декоратор цвета шрифта
    function WithColor(c: GColor): FontOptions;
    begin
      Result := new FontOptions;
      Result.tf := tf;
      Result.sz := sz;
      Result.Color := c;
    end;
    /// Декоратор размера шрифта
    function WithSize(sz: real): FontOptions;
    begin
      Result := new FontOptions;
      Result.tf := tf;
      Result.sz := sz;
      Result.Color := c;
    end;
    /// Декоратор стиля шрифта
    function WithName(name: string): FontOptions;
    begin
      Result := new FontOptions;
      Result.sz := sz;
      Result.Color := c;
      Result.tf := new Typeface(GetFontFamily(name),tf.Style,tf.Weight,FontStretches.Normal);
    end;
  end;
  
// -----------------------------------------------------
//>>     Класс GraphWindowType # GraphWPF GraphWindowType class
// -----------------------------------------------------
  ///!#
  /// Тип графического окна
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
    /// Клиентский прямоугольник графического окна
    function ClientRect: GRect;
    /// Центр графического окна
    function Center: Point;
    /// Сохраняет содержимое графического окна в файл с именем fname
    procedure Save(fname: string);
    /// Восстанавливает содержимое графического окна из файла с именем fname
    procedure Load(fname: string);
    /// Заполняет содержимое графического окна обоями из файла с именем fname
    procedure Fill(fname: string);
    /// Возвращает случайную точку в границах экрана. Необязательный параметр margin задаёт минимальный отступ от границы 
    function RandomPoint(margin: real := 0): Point;
    /// Очищает графическое окно белым цветом
    procedure Clear; 
  end;
  
  // Специфический тип окна для модуля GraphWPF
// -----------------------------------------------------
//>>     Класс WindowTypeWPF # WindowTypeWPF class
// -----------------------------------------------------
  /// Тип графического окна WindowTypeWPF
  WindowTypeWPF = class(WindowType)
  public
    /// Сохраняет содержимое графического окна в файл с именем fname
    procedure Save(fname: string);
    /// Восстанавливает содержимое графического окна из файла с именем fname
    procedure Load(fname: string);
    /// Очищает графическое окно белым цветом
    procedure Clear; override;
    /// Очищает графическое окно цветом c
    procedure Clear(c: Color); override;
  end;

// -----------------------------------------------------
//>>     Класс Vector # Vector class
// -----------------------------------------------------
  
//{{{--doc: Конец секции 2 }}} 

  // Класс Vector
  {Vector =  class
  public
    x,y: real;
    /// Создаёт вектор с указанными координатами
    constructor(vx,vy: real) := (x,y) := (vx,vy);
    
    static function operator*(v: Vector; r: real): Vector := new Vector(r*v.x,r*v.y);
    static function operator*(r: real; v: Vector): Vector := new Vector(r*v.x,r*v.y);
    static function operator+(v: Vector; p: Point): Point := new Point(p.x+v.x,p.y+v.y);
    static function operator+(p: Point; v: Vector): Point := new Point(p.x+v.x,p.y+v.y);
  end;}
 
 
//{{{doc: Начало секции 3 }}} 

// -----------------------------------------------------
//>>     Графические примитивы # GraphWPF primitives
// -----------------------------------------------------
/// Рисует пиксел в точке (x,y) цветом c
procedure SetPixel(x,y: real; c: Color);
/// Рисует прямоугольник пикселей размера (w,h), задаваемых отображением f, начиная с левого верхнего угла с координатами (x,y)
procedure SetPixels(x,y: real; w,h: integer; f: (integer,integer)->Color);
/// Рисует двумерный массив пикселей pixels начиная с левого верхнего угла с координатами (x,y)
procedure DrawPixels(x,y: real; pixels: array [,] of Color);
/// Рисует прямоугольную область (px,py,pw,ph) двумерного массива пикселей pixels начиная с левого верхнего угла с координатами (x,y)
procedure DrawPixels(x,y: real; pixels: array [,] of Color; px,py,pw,ph: integer);

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

/// Рисует эллипс с центром в точке p и радиусами rx и ry
procedure Ellipse(p: Point; rx,ry: real);
/// Рисует контур эллипса с центром в точке p и радиусами rx и ry
procedure DrawEllipse(p: Point; rx,ry: real);
/// Рисует внутренность эллипса с центром в точке p и радиусами rx и ry
procedure FillEllipse(p: Point; rx,ry: real);
/// Рисует эллипс с центром в точке p, радиусами rx и ry и цветом внутренности c
procedure Ellipse(p: Point; rx,ry: real; c: Color);
/// Рисует контур эллипса с центром в точке p, радиусами rx и ry и цветом c
procedure DrawEllipse(p: Point; rx,ry: real; c: Color);
/// Рисует внутренность эллипса с центром в точке p, радиусами rx и ry и цветом c
procedure FillEllipse(p: Point; rx,ry: real; c: Color);

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

/// Рисует окружность с центром в точке p и радиусом r
procedure Circle(p: Point; r: real);
/// Рисует контур окружности с центром в точке p и радиусом r
procedure DrawCircle(p: Point; r: real);
/// Рисует внутренность окружности с центром в точке p и радиусом r
procedure FillCircle(p: Point; r: real);
/// Рисует окружность с центром в точке p, радиусом r и цветом c
procedure Circle(p: Point; r: real; c: Color);
/// Рисует контур окружности с центром в точке p, радиусом r и цветом c
procedure DrawCircle(p: Point; r: real; c: Color);
/// Рисует внутренность окружности с центром в точке p, радиусом r и цветом c
procedure FillCircle(p: Point; r: real; c: Color);


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
/// Рисует сектор окружности с центром в точке (x,y) и радиусом r, заключенный между двумя лучами, образующими углы angle1 и angle2 с осью OX, цветом c
procedure Pie(x, y, r, angle1, angle2: real; c: Color);
/// Рисует контур сектора окружности с центром в точке (x,y) и радиусом r, заключенного между двумя лучами, образующими углы angle1 и angle2 с осью OX, цветом c
procedure DrawSector(x, y, r, angle1, angle2: real; c: Color);
/// Рисует внутренность сектора окружности с центром в точке (x,y) и радиусом r, заключенного между двумя лучами, образующими углы angle1 и angle2 с осью OX, цветом c
procedure FillSector(x, y, r, angle1, angle2: real; c: Color);

/// Рисует отрезок прямой от точки (x,y) до точки (x1,y1)
procedure Line(x,y,x1,y1: real);
/// Рисует отрезок прямой от точки (x,y) до точки (x1,y1) цветом c
procedure Line(x,y,x1,y1: real; c: Color);
/// Рисует отрезок прямой от точки p до точки p1
procedure Line(p,p1: Point);
/// Рисует отрезок прямой от точки p до точки p1 цветом c
procedure Line(p,p1: Point; c: Color);
/// Рисует отрезки, заданные массивом пар точек 
procedure Lines(a: array of (Point,Point));
/// Рисует отрезки, заданные массивом пар точек, цветом c 
procedure Lines(a: array of (Point,Point); c: Color);
/// Устанавливает текущую позицию рисования в точку (x,y)

/// Рисует отрезок прямой от точки (x,y) до точки (x1,y1) со стрелкой на конце
procedure Arrow(x,y,x1,y1: real);
/// Рисует отрезок прямой от точки (x,y) до точки (x1,y1) цветом c со стрелкой на конце 
procedure Arrow(x,y,x1,y1: real; c: Color);
/// Рисует отрезок прямой от точки p до точки p1 со стрелкой на конце
procedure Arrow(p,p1: Point);
/// Рисует отрезок прямой от точки p до точки p1 цветом c со стрелкой на конце
procedure Arrow(p,p1: Point; c: Color);

procedure MoveTo(x,y: real);
/// Рисует отрезок от текущей позиции до точки (x,y). Текущая позиция переносится в точку (x,y)
procedure LineTo(x,y: real);
/// Перемещает текущую позицию рисования на вектор (dx,dy)
procedure MoveRel(dx,dy: real);
/// Рисует отрезок от текущей позиции до точки, смещённой на вектор (dx,dy). Текущая позиция переносится в новую точку
procedure LineRel(dx,dy: real);
/// Перемещает текущую позицию рисования на вектор (dx,dy)
procedure MoveBy(dx,dy: real);
/// Рисует отрезок от текущей позиции до точки, смещённой на вектор (dx,dy). Текущая позиция переносится в новую точку
procedure LineBy(dx,dy: real);
///--
procedure MoveOn(dx,dy: real);
///--
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

// -----------------------------------------------------
//>>     Класс Bitmap # Bitmap class
// -----------------------------------------------------
///!#
/// Класс битового образа
type Bitmap = class
private 
  bsource: TransformedBitmap;
  procedure SetScaleX(scx: real);
  function GetScaleX: real;
  procedure SetScaleY(scy: real);
  function GetScaleY: real;
public
  constructor Create(fname: string);
/// Отразить битовый образ относительно горизонтальной оси
  procedure FlipHorizontal;
/// Отразить битовый образ относительно вертикальной оси
  procedure FlipVertical;
/// Коэффициент масштабирования по оси X
  property ScaleX: real read GetScaleX write SetScaleX;
/// Коэффициент масштабирования по оси Y
  property ScaleY: real read GetScaleY write SetScaleY;
/// Возвращает клон битового образа
  function Clone: Bitmap;
end;

// -----------------------------------------------------
//>>     Функции для вывода изображений и видео # GraphWPF functions for images and video
// -----------------------------------------------------
/// Рисует изображение из файла fname в позиции (x,y)
procedure DrawImage(x,y: real; fname: string);
/// Рисует изображение из файла fname в позиции (x,y) размера w на h
procedure DrawImage(x,y,w,h: real; fname: string);
/// Рисует изображение из битмапа в позиции (x,y)
procedure DrawImage(x,y: real; b: Bitmap);
/// Рисует изображение из битмапа в позиции (x,y) размера w на h
procedure DrawImage(x,y,w,h: real; b: Bitmap);
/// Рисует немасштабированное изображение из файла fname в позиции (x,y)
procedure DrawImageUnscaled(x,y: real; fname: string);
/// Выводит видео из файла fname в позицию (x,y) в прямоугольник размером (w,h)
procedure DrawVideo(x,y,w,h: real; fname: string);

/// Ширина изображения в пикселах
function ImageWidth(fname: string): integer;
/// Высота изображения в пикселах
function ImageHeight(fname: string): integer;
/// Размер изображения в пикселах
function ImageSize(fname: string): (integer,integer);

// -----------------------------------------------------
//>>     Вспомогательные функции GraphWPF # GraphWPF service functions
// -----------------------------------------------------
/// Возвращает цвет по красной, зеленой и синей составляющей (в диапазоне 0..255)
function RGB(r,g,b: byte): Color;
/// Возвращает цвет по красной, зеленой и синей составляющей и параметру прозрачности (в диапазоне 0..255)
function ARGB(a,r,g,b: byte): Color;
/// Возвращает серый цвет с интенсивностью b
function GrayColor(b: byte): Color;
/// Возвращает случайный цвет
function RandomColor: Color;
/// Возвращает полностью прозрачный цвет
function EmptyColor: Color;
/// Возвращает случайный цвет
function clRandom: Color;
/// Возвращает точку с координатами (x,y)
function Pnt(x,y: real): GPoint;
/// Возвращает прямоугольник с координатами угла (x,y), шириной w и высотой h
function Rect(x,y,w,h: real): GRect;
/// Возвращает однотонную цветную кисть, заданную цветом
function ColorBrush(c: Color): GBrush;
/// Возвращает однотонную цветную кисть случайного цвета
function RandomColorBrush: GBrush;
/// Возвращает однотонное цветное перо, заданное цветом
function ColorPen(c: Color): GPen;
/// Возвращает однотонное цветное перо, заданное цветом и толщиной
function ColorPen(c: Color; w: real): GPen;
/// Процедура ускорения вывода. Обновляет экран после всех изменений
procedure Redraw(d: ()->());
/// Функция генерации случайной точки в границах экрана. Необязательный параметр w задаёт минимальный отступ от границы
function RandomPoint(w: real := 0): Point;
/// Функция генерации массива случайных точек в границах экрана. Необязательный параметр w задаёт минимальный отступ от границы
function RandomPoints(n: integer; w: real := 0): array of Point;
/// Создаёт вектор с координатами vx,vy
function Vect(vx,vy: real): Vector;


// -----------------------------------------------------
//>>     Процедуры покадровой анимации # GraphWPF FrameBasedAnimation functions
// -----------------------------------------------------
/// Начинает анимацию, основанную на кадре. Перед рисованием каждого кадра содержимое окна стирается, затем вызывается процедура Draw
procedure BeginFrameBasedAnimation(Draw: procedure; frate: integer := 61);
/// Начинает анимацию, основанную на кадре. Перед рисованием каждого кадра содержимое окна стирается, затем вызывается процедура Draw с параметром, равным номеру кадра
procedure BeginFrameBasedAnimation(Draw: procedure(frame: integer); frate: integer := 61);
/// Начинает анимацию, основанную на кадре, и передаёт в каждый обработчик кадра время dt, прошедшее с момента последней перерисовки
procedure BeginFrameBasedAnimationTime(Draw: procedure(dt: real));
/// Завершает анимацию, основанную на кадре
procedure EndFrameBasedAnimation;

// -----------------------------------------------------
//>>     Функции для вывода текста # GraphWPF text functions
// -----------------------------------------------------

/// Выводит текстовое представление объекта в прямоугольник размера (w,h) с координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; text: object; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит текстовое представление объекта указанным цветом в прямоугольник (x,y,w,h)
procedure DrawText(x, y, w, h: real; text: object; color: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит текстовое представление объекта в указанный прямоугольник
procedure DrawText(rect: GRect; text: object; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит текстовое представление объекта в указанный прямоугольник указанным цветом
procedure DrawText(rect: GRect; text: object; color: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
/// Выводит строку в прямоугольник (x,y,w,h) указанным шрифтом
procedure DrawText(x, y, w, h: real; text: object; f: FontOptions; align: Alignment; angle: real);


/// Выводит текстовое представление объекта в позицию (x,y)
procedure TextOut(x, y: real; text: object; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
/// Выводит текстовое представление объекта в позицию (x,y) указанным цветом
procedure TextOut(x, y: real; text: object; color: GColor; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
/// Выводит текстовое представление объекта в позицию (x,y) указанным шрифтом
procedure TextOut(x, y: real; text: object; f: FontOptions; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
/// Выводит текстовое представление объекта в позицию pos
procedure TextOut(pos: Point; text: object; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
/// Выводит текстовое представление объекта в позицию pos указанным цветом
procedure TextOut(pos: Point; text: object; color: GColor; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
/// Выводит текстовое представление объекта в позицию pos указанным шрифтом
procedure TextOut(pos: Point; text: object; f: FontOptions; align: Alignment := Alignment.LeftTop; angle: real := 0.0);


/// Ширина текста при выводе
function TextWidth(text: string): real;
/// Высота текста при выводе
function TextHeight(text: string): real;
/// Размер текста при выводе
function TextSize(text: string): Size;

/// Ширина текста при выводе заданным шрифтом
function TextWidth(text: string; f: FontOptions): real;
/// Высота текста при выводе заданным шрифтом
function TextHeight(text: string; f: FontOptions): real;
/// Размер текста при выводе заданным шрифтом
function TextSize(text: string; f: FontOptions): Size;

// -----------------------------------------------------
//>>     Функции для вывода графиков # GraphWPF graph functions
// -----------------------------------------------------
/// Рисует график функции f, заданной на отрезке [a,b] по оси абсцисс и на отрезке [min,max] по оси ординат, в прямоугольнике, задаваемом параметрами x,y,w,h 
procedure DrawGraph(f: real -> real; a, b, min, max, x, y, w, h: real; title: string := '');
/// Рисует график функции f, заданной на отрезке [a,b] по оси абсцисс и на отрезке [min,max] по оси ординат, в прямоугольнике, задаваемом параметрами x,y,w,h. Два последних параметра задают шаг сетки по OX и OY
procedure DrawGraph(f: real -> real; a, b, min, max, x, y, w, h: real; XTicks: real; YTicks: real; title: string := '');
/// Рисует график функции f, заданной на отрезке [a,b] по оси абсцисс и на отрезке [min,max] по оси ординат, в прямоугольнике r
procedure DrawGraph(f: real -> real; a, b, min, max: real; r: GRect; title: string := '');
/// Рисует график функции f, заданной на отрезке [a,b] по оси абсцисс и на отрезке [min,max] по оси ординат, в прямоугольнике r. Два последних параметра задают шаг сетки по OX и OY
procedure DrawGraph(f: real -> real; a, b, min, max: real; r: GRect; XTicks, YTicks: real; title: string := '');
/// Рисует график функции f, заданной на отрезке [a,b] по оси абсцисс и на отрезке [min,max] по оси ординат, на полное графическое окно
procedure DrawGraph(f: real -> real; a, b, min, max: real; title: string := '');
/// Рисует график функции f, заданной на отрезке [a,b], в прямоугольнике, задаваемом параметрами x,y,w,h 
procedure DrawGraph(f: real -> real; a, b: real; x, y, w, h: real; title: string := '');
/// Рисует график функции f, заданной на отрезке [a,b], в прямоугольнике r 
procedure DrawGraph(f: real -> real; a, b: real; r: GRect; title: string := '');
/// Рисует график функции f, заданной на отрезке [-5,5], в прямоугольнике r 
procedure DrawGraph(f: real -> real; r: GRect; title: string := '');
/// Рисует график функции f, заданной на отрезке [a,b], на полное графическое окно 
procedure DrawGraph(f: real -> real; a, b: real; title: string := '');
/// Рисует график функции f, заданной на отрезке [-5,5], на полное графическое окно  
procedure DrawGraph(f: real -> real; title: string := '');

// -----------------------------------------------------
//>>     Функции для настройки системы координат # GraphWPF coordinate system functions
// -----------------------------------------------------
/// Устанавливает математическую систему координат с диапазоном [x1,x2] по оси OX. 
procedure SetMathematicCoords(x1: real := -10; x2: real := 10; drawgrid: boolean := true);
/// Устанавливает математическую систему координат с диапазоном [x1,x2] по оси OX и минимальной координатой ymin по оси OY
procedure SetMathematicCoords(x1,x2,ymin: real; drawgrid: boolean := true);
/// Устанавливает стандартную систему координат (ось OY направлена вниз) с центром (x0,y0)
procedure SetStandardCoords(scale: real := 1.0; x0: real := 0; y0: real := 0);
/// Устанавливает стандартную систему координат с центром (x0,y0). Изображение не масштабируется к разрешению монитора и линии получаются чёткими
procedure SetStandardCoordsSharpLines(x0: real := 0; y0: real := 0);
/// Рисует сетку системы координат
procedure DrawGrid;

/// Минимальная отображаемая x-координата
function XMin: real;
/// Максимальная отображаемая x-координата
function XMax: real;
/// Минимальная отображаемая y-координата
function YMin: real;
/// Максимальная отображаемая y-координата
function YMax: real;

// -----------------------------------------------------
//>>     Переменные модуля GraphWPF # GraphWPF variables
// -----------------------------------------------------
/// Текущая кисть
var Brush: BrushType;
/// Текущее перо
var Pen: PenType;
/// Текущий шрифт
var Font: FontOptions;
/// Главное окно
var Window: WindowTypeWPF;
/// Графическое окно
var GraphWindow: GraphWindowType;

// -----------------------------------------------------
//>>     События модуля GraphWPF # GraphWPF events
// -----------------------------------------------------
/// Событие нажатия на кнопку мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 1, если нажата левая кнопка мыши, и 2, если нажата правая кнопка мыши
var OnMouseDown: procedure(x, y: real; mousebutton: integer);
/// Событие отжатия кнопки мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 1, если отжата левая кнопка мыши, и 2, если отжата правая кнопка мыши
var OnMouseUp: procedure(x, y: real; mousebutton: integer);
/// Событие перемещения мыши. (x,y) - координаты курсора мыши в момент наступления события, mousebutton = 0, если кнопка мыши не нажата, 1, если нажата левая кнопка мыши, и 2, если нажата правая кнопка мыши
var OnMouseMove: procedure(x, y: real; mousebutton: integer);
/// Событие прокрутки колёсика мыши. delta - величина прокрутки: delta > 0 - от пользователя, delta < 0 - к пользователю
var OnMouseWheel: procedure(delta: real);
/// Событие нажатия клавиши
var OnKeyDown: procedure(k: Key);
/// Событие отжатия клавиши
var OnKeyUp: procedure(k: Key);
/// Событие нажатия символьной клавиши
var OnKeyPress: procedure(ch: char);
/// Событие изменения размера графического окна
var OnResize: procedure;
/// Событие, происходящее при закрытии основного окна
var OnClose: procedure;
/// Событие перерисовки графического окна. Параметр dt обозначает количество миллисекунд с момента последнего вызова OnDrawFrame
var OnDrawFrame: procedure(dt: real);


//{{{--doc: Конец секции 3 }}} 

// Для WPFObjects
var AdditionalInit: procedure;
function GetMouseArgs(e: MouseEventArgs): (Point,integer);

// -----------------------------------------------------
//>>     Процедуры рисования, использующие DrawingContext # DrawingContext procedures
// -----------------------------------------------------
/// Процедура быстрого рисования последовательности команд, использующих DrawingContext
procedure FastDraw(commands: DrawingContext->());

/// Рисование отрезка с помощью DrawingContext (совместно с FastDraw)
procedure DrawLineDC(dc: DrawingContext; x,y,x1,y1: real; c: Color; w: real);
/// Рисование отрезка с помощью DrawingContext (совместно с FastDraw)
procedure DrawLineDC(dc: DrawingContext; x,y,x1,y1: real; p: GPen);
/// Рисование эллипса с помощью DrawingContext (совместно с FastDraw)
procedure DrawEllipseDC(dc: DrawingContext; x,y,rx,ry: real; cbrush,cpen: Color; w: real);
/// Рисование эллипса с помощью DrawingContext (совместно с FastDraw)
procedure DrawEllipseDC(dc: DrawingContext; x,y,rx,ry: real; b: GBrush; p: GPen);
/// Рисование прямоугольника с помощью DrawingContext (совместно с FastDraw)
procedure DrawRectangleDC(dc: DrawingContext; x,y,width,height: real; cbrush,cpen: Color; w: real);
/// Рисование прямоугольника с помощью DrawingContext (совместно с FastDraw)
procedure DrawRectangleDC(dc: DrawingContext; x,y,width,height: real; b: GBrush; p: GPen);
/// Рисование полигона с помощью DrawingContext (совместно с FastDraw)
procedure DrawPolygonDC(dc: DrawingContext; pnt: array of Point; cbrush,cpen: Color; w: real);
/// Рисование полигона с помощью DrawingContext (совместно с FastDraw)
procedure DrawPolygonDC(dc: DrawingContext; pnt: array of Point; b: GBrush; p: GPen);
/// Рисование ломаной с помощью DrawingContext (совместно с FastDraw)
procedure DrawPolylineDC(dc: DrawingContext; pnt: array of Point; cpen: Color; w: real);
/// Рисование ломаной с помощью DrawingContext (совместно с FastDraw)
procedure DrawPolylineDC(dc: DrawingContext; pnt: array of Point; p: GPen);
/// Рисование текста с помощью DrawingContext (совместно с FastDraw)
procedure TextOutDC(dc: DrawingContext; x,y: real; text: string; align: Alignment := Alignment.LeftTop; angle: real := 0; f: FontOptions := nil);
/// Рисование текста в прямоугольнике с помощью DrawingContext (совместно с FastDraw)
procedure DrawTextDC(dc: DrawingContext; x,y,w,h: real; text: string; align: Alignment := Alignment.Center; angle: real := 0; f: FontOptions := nil);

/// Рисование графика с помощью DrawingContext (совместно с FastDraw)
procedure DrawGraphDC(dc: DrawingContext; f: real -> real; a, b, min, max, x, y, w, h, XTicks, YTicks: real; title: string := '');
/// Рисование графика с помощью DrawingContext (совместно с FastDraw)
procedure DrawGraphDC(dc: DrawingContext; f: real -> real; a, b, min, max, x, y, w, h: real; title: string := '');
/// Рисование графика с помощью DrawingContext (совместно с FastDraw)
procedure DrawGraphDC(dc: DrawingContext; f: real -> real; a, b: real; x, y, w, h: real; title: string := '');

/// Ширина текста (совместно с FastDraw)
function TextWidthP(text: string): real;
/// Высота текста (совместно с FastDraw)
function TextHeightP(text: string): real; 
/// Размер текста (совместно с FastDraw)
function TextSizeP(text: string): Size;
/// Ширина текста (совместно с FastDraw)
function TextWidthPFont(text: string; f: FontOptions): real;
/// Высота текста (совместно с FastDraw)
function TextHeightPFont(text: string; f: FontOptions): real; 
/// Размер текста (совместно с FastDraw)
function TextSizePFont(text: string; f: FontOptions): Size;
/// Создает визуальный объект и добавляет его к отображаемым объектам  
function CreateVisual: DrawingVisual;
/// Удаляет визуальный объект из отображаемых
procedure RemoveVisual(visual: DrawingVisual);
/// Рисует на визуальном объекте с помощью контекста рисования
procedure DrawOnVisual(visual: DrawingVisual; proc: DrawingContext->());
/// Рисует все визуальные объекты на битмапе рендеринга и очищает список визуальных объектов
procedure FlushDrawingToBitmap;


//procedure AddToHost(v: Visual);
//procedure RemoveFromHost(v: Visual);

function GetDC: DrawingContext;
function CreateRenderTargetBitmap: RenderTargetBitmap;

var AdditionalDrawOnDC: procedure(dc: DrawingContext);

//procedure ReleaseDC(dc: DrawingContext);
//procedure FastClear(var dc: DrawingContext);
//procedure HostToRenderBitmap;

procedure __InitModule__;
procedure __FinalizeModule__;

(*
// Выводит строку в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; text: string; align: Alignment := Alignment.Center; angle: real := 0.0);
// Выводит строку в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; text: string; c: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
// Выводит целое в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: integer; align: Alignment := Alignment.Center; angle: real := 0.0);
// Выводит вещественное в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: real; align: Alignment := Alignment.Center; angle: real := 0.0);
// Выводит строку в прямоугольник
procedure DrawText(r: GRect; text: string; align: Alignment := Alignment.Center; angle: real := 0.0);
// Выводит целое в прямоугольник
procedure DrawText(r: GRect; number: integer; align: Alignment := Alignment.Center; angle: real := 0.0);
// Выводит вещественное в прямоугольник
procedure DrawText(r: GRect; number: real; align: Alignment := Alignment.Center; angle: real := 0.0);
// Выводит целое в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: integer; c: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
// Выводит вещественное в прямоугольник к координатами левого верхнего угла (x,y)
procedure DrawText(x, y, w, h: real; number: real; c: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
// Выводит строку в прямоугольник
procedure DrawText(r: GRect; text: string; c: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
// Выводит целое в прямоугольник
procedure DrawText(r: GRect; number: integer; c: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
// Выводит вещественное в прямоугольник
procedure DrawText(r: GRect; number: real; c: GColor; align: Alignment := Alignment.Center; angle: real := 0.0);
// Выводит строку в прямоугольник к координатами левого верхнего угла (x,y) указанным шрифтом
procedure DrawText(x, y, w, h: real; text: string; f: FontOptions; align: Alignment; angle: real);

// Выводит строку в позицию (x,y)
procedure TextOut(x, y: real; text: string; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
// Выводит строку в позицию (x,y) цветом c
procedure TextOut(x, y: real; text: string; c: GColor; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
// Выводит целое в позицию (x,y)
procedure TextOut(x, y: real; text: integer; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
// Выводит целое в позицию (x,y) цветом c
procedure TextOut(x, y: real; text: integer; c: GColor; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
// Выводит вещественное в позицию (x,y)
procedure TextOut(x, y: real; text: real; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
// Выводит вещественное в позицию (x,y) цветом c
procedure TextOut(x, y: real; text: real; c: GColor; align: Alignment := Alignment.LeftTop; angle: real := 0.0);
// Выводит строку в позицию (x,y) указанным шрифтом
procedure TextOut(x, y: real; text: string; f: FontOptions; align: Alignment := Alignment.LeftTop; angle: real := 0.0);}
*)

implementation

var FontFamiliesDict := new Dictionary<string,FontFamily>;

function GetFontFamily(name: string): FontFamily;
begin
  if not (name in FontFamiliesDict) then
  begin
    var b := new FontFamily(name);
    FontFamiliesDict[name] := b;
    Result := b
  end
  else Result := FontFamiliesDict[name];
end;

procedure Redraw(d: ()->()) := app.Dispatcher.Invoke(d);
function getApp: Application := app;

function RGB(r,g,b: byte) := Color.Fromrgb(r, g, b);
function ARGB(a,r,g,b: byte) := Color.FromArgb(a, r, g, b);
function GrayColor(b: byte): Color := RGB(b, b, b);
function RandomColor := RGB(PABCSystem.Random(256), PABCSystem.Random(256), PABCSystem.Random(256));
function EmptyColor: Color := ARGB(0,0,0,0);
function clRandom := RandomColor();
function Pnt(x,y: real) := new Point(x,y);
function Rect(x,y,w,h: real) := new System.Windows.Rect(x,y,w,h);
function ColorBrush(c: Color) := GetBrush(c);
function RandomColorBrush := GetBrush(RandomColor);
function ColorPen(c: Color): GPen;
begin
  Result := new GPen(GetBrush(c),Pen.Width);
  Result.Freeze
end;
function ColorPen(c: Color; w: real): GPen;
begin
  Result := new GPen(GetBrush(c),w);
  Result.Freeze
end;

function RandomPoint(w: real): Point := Pnt(Random(w,Window.Width-w),Random(w,Window.Height-w));

function RandomPoints(n: integer; w: real): array of Point;
begin
  Result := new Point[n];
  foreach var i in 0..n-1 do
    Result[i] := RandomPoint(w);
end;

function Vect(vx,vy: real): Vector := new Vector(vx,vy);

procedure InvokeVisual(d: System.Delegate; params args: array of object);
begin
  app.Dispatcher.Invoke(d,args)
end;

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

var 
  Host: MyVisualHost;
  Host1: Canvas;
  RTbmap: RenderTargetBitmap;
  rtbmapIsCleared := True;

var 
  XOrigin := 0.0;
  YOrigin := 0.0;
  GlobalScale := 1.0;
  CurrentCoordType: CoordType := StandardCoords;

procedure AddToHost(v: Visual);
begin
  Host.children.Add(v);
end;

procedure RemoveFromHost(v: Visual);
begin
  Host.children.Remove(v);
end;

function GetDC: DrawingContext;
begin
  var visual := new DrawingVisual();
  Host.children.Add(visual);
  Result := visual.RenderOpen();
end;

procedure HostToRenderBitmap;
begin
  rtBmap.Render(host);
  rtbmapIsCleared := False;
  host.Children.Clear;
end;

procedure ReleaseDC(dc: DrawingContext);
begin
  dc.Close;
  if host.Children.Count > 1000 then
    HostToRenderBitmap
end;

/// Создает визуальный объект и добавляет его к отображаемым объектам  
function CreateVisual: DrawingVisual;
begin
  var visual: DrawingVisual;
  Redraw(()->begin visual := new DrawingVisual; AddToHost(visual) end );
  Result := visual;
end;

/// Удаляет визуальный объект из отображаемых
procedure RemoveVisual(visual: DrawingVisual) := Redraw(()->RemoveFromHost(visual));

/// Рисует на визуальном объекте с помощью контекста рисования
procedure DrawOnVisual(visual: DrawingVisual; proc: DrawingContext->());
begin
  Redraw(()->begin
    var dc := visual.RenderOpen();
    proc(dc);
    dc.Close;
  end);
end;

/// Рисует все визуальные объекты на битмапе рендеринга и очищает список визуальных объектов
procedure FlushDrawingToBitmap := Redraw(()->HostToRenderBitmap());


procedure FastDraw(commands: DrawingContext->());
begin
  Invoke(()->
  begin
    var dc := GetDC;
    commands(dc);  
    ReleaseDC(dc);
  end);
end;

procedure FastClear(var dc: DrawingContext);
begin
  ReleaseDC(dc);
  Window.Clear;
  dc := GetDC;
end;

function GetDC(t: Transform): DrawingContext;
begin
  var visual := new DrawingVisual();
  visual.Transform := t;
  Host.children.Add(visual);
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
  ReleaseDC(dc);
end;

type VE = auto class
  g: ()->Geometry;
end;

procedure DrawGeometryP(g: VE);
begin
  var dc := GetDC();
  dc.DrawGeometry(Brush.BrushConstruct,Pen.PenConstruct,g.g());
  ReleaseDC(dc);
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
  ReleaseDC(dc);
end;

procedure SetPixelP(x,y: real; c: Color);
begin
  var dc := GetDC();
  dc.DrawRectangle(GetBrush(c), nil, Rect(x,y,1,1));
  ReleaseDC(dc);
end;


procedure SetPixelsP(x,y: real; w,h: integer; f: (integer,integer)->Color);
begin
  var dc := GetDC();
  for var ix:=0 to w-1 do
  for var iy:=0 to h-1 do
  begin
    dc.DrawRectangle(GetBrush(f(ix,iy)), nil, Rect(x+ix,y+iy,1,1));
  end;  
  ReleaseDC(dc);
end;
  
procedure LinePFull(x,y,x1,y1: real; p: GPen);
begin
  var dc := GetDC();
  dc.DrawLine(p, Pnt(x,y), Pnt(x1,y1));
  ReleaseDC(dc);
end;

var RusCultureInfo := new System.Globalization.CultureInfo('ru-ru');

function FormText(text: string) := 
  new FormattedText(text, RusCultureInfo, FlowDirection.LeftToRight, 
                    Font.tf, Font.Size, Font.BrushClone);
  
function FormTextFont(text: string; f: FontOptions): FormattedText;
begin
  var tf := new Typeface(GetFontFamily(f.Name),f.tf.Style,f.tf.Weight,f.tf.Stretch);
  Result := new FormattedText(text,RusCultureInfo, FlowDirection.LeftToRight, tf, f.Size, f.BrushClone);
end; 
    
function FormTextC(text: string; c: GColor): FormattedText;
begin 
  Result := new FormattedText(text,RusCultureInfo, FlowDirection.LeftToRight, Font.TypefaceClone, Font.Size, GetBrush(c));
end;                    
                    
function TextWidthP(text: string) := FormText(text).Width;
function TextHeightP(text: string) := FormText(text).Height;

function TextWidthPFont(text: string; f: FontOptions) := FormTextFont(text,f).Width;
function TextHeightPFont(text: string; f: FontOptions) := FormTextFont(text,f).Height;


type TextV = auto class
  text: string;
  function TextWidth := TextWidthP(text);
  function TextHeight := TextHeightP(text);
  function TextSize: Size;
  begin
    var ft := FormText(text);
    Result := new Size(ft.Width,ft.Height);
  end;
  function TextWidthFont(f: FontOptions) := TextWidthP(text);
  function TextHeightFont(f: FontOptions) := TextHeightP(text);
  function TextSizeFont(f: FontOptions): Size;
  begin
    var ft := FormTextFont(text,f);
    Result := new Size(ft.Width,ft.Height);
  end;
end;

function TextSizeP(text: string): Size := TextV.Create(text).TextSizeFont(Font);
function TextSizePFont(text: string; f: FontOptions): Size := TextV.Create(text).TextSizeFont(f);

procedure TextOutDCHelper(dc: DrawingContext; x,y: real; ft: FormattedText; angle,x0,y0: real);
begin
  var RT := new RotateTransform(angle,x0,y0);
  dc.PushTransform(RT);
  if CurrentCoordType = StandardCoords then
  begin
    dc.DrawText(ft,new Point(x,y));
  end  
  else   
  begin
    var m := Host.RenderTransform.Value;
    var mt := new MatrixTransform(1/m.M11,0,0,1/m.M22,x,y);
    dc.PushTransform(mt);

    dc.DrawText(ft,new Point(0,0));

    dc.Pop();
  end;  
  dc.Pop();
end;

procedure DrawTextHelper(var x, y, x0, y0: real; w, h: real; sz: Size; align: Alignment := Alignment.Center; f: FontOptions := nil); forward;

procedure TextOutDC(dc: DrawingContext; x,y: real; text: string; align: Alignment; angle: real; f: FontOptions);
begin
  var (x0,y0) := (x,y);
  
  if f = nil then f := Font;
  var ft := FormTextFont(text,f);
  var sz := new Size(ft.Width,ft.Height);

  DrawTextHelper(x, y, x0, y0, 0, 0, sz, align, f);
  TextOutDCHelper(dc,x,y,ft,angle,x0,y0);
end;

procedure DrawTextDC(dc: DrawingContext; x,y,w,h: real; text: string; align: Alignment; angle: real; f: FontOptions);
begin
  var (x0,y0) := (x,y);

  if f = nil then f := Font;
  var ft := FormTextFont(text,f);
  var sz := new Size(ft.Width,ft.Height);

  DrawTextHelper(x, y, x0, y0, w, h, sz, align, f);
  TextOutDCHelper(dc,x,y,ft,angle,x0,y0);
end;

procedure TextPFull(x,y: real; text: string; angle,x0,y0: real; f: FontOptions);
begin
  var dc := GetDC();
  var ft := FormTextFont(text,f);
  TextOutDCHelper(dc,x,y,ft,angle,x0,y0);
  ReleaseDC(dc);
end;

procedure TextPFull(x,y: real; text: string; angle,x0,y0: real; c: Color);
begin
  TextPFull(x,y,text,angle,x0,y0,Font.WithColor(c));
end;

procedure TextPFull(x,y: real; text: string; angle,x0,y0: real);
begin
  TextPFull(x,y,text,angle,x0,y0,Font);
end;


var dpic := new Dictionary<string, BitmapSource>;

function GetBitmapImage(fname: string): BitmapSource;
begin
  if not dpic.ContainsKey(fname) then 
  begin
    var b := new BitmapImage();
    var s := System.IO.File.OpenRead(fname);
    b.BeginInit();
    b.CacheOption := BitmapCacheOption.OnLoad;
    b.StreamSource := s;
    b.EndInit();
    s.Close();    
    //dpic[fname] := new BitmapImage(new System.Uri(fname,System.UriKind.Relative));
    dpic[fname] := b;
  end;  
  Result := dpic[fname];
end;

// -----------------------------------------------------
//>>     Класс Bitmap
// -----------------------------------------------------
///!#
constructor Bitmap.Create(fname: string);
begin
  Invoke(()->begin
    bsource := new TransformedBitmap(GetBitmapImage(fname),new ScaleTransform());
  end);
end;

function Bitmap.Clone: Bitmap;
begin
  Result := Invoke&<Bitmap>(()->
  begin
    var b := new Bitmap();
    b.bsource := new TransformedBitmap(bsource.Source,bsource.Transform);
    Result := b;
  end);  
end;

procedure Bitmap.FlipHorizontal := ScaleY := -ScaleY;

procedure Bitmap.FlipVertical := ScaleX := -ScaleX;

procedure Bitmap.SetScaleX(scx: real);
begin
  Invoke(()->begin
    var sct := bsource.Transform as ScaleTransform;
    sct.ScaleX := scx;
    bsource := new TransformedBitmap(bsource.Source,sct);
  end);
end;

function Bitmap.GetScaleX: real;
begin
  Result := InvokeReal(()->(bsource.Transform as ScaleTransform).ScaleX);
end;

function Bitmap.GetScaleY: real;
begin
  Result := InvokeReal(()->(bsource.Transform as ScaleTransform).ScaleY);
end;

procedure Bitmap.SetScaleY(scy: real);
begin
  Invoke(()->begin
    var sct := bsource.Transform as ScaleTransform;
    sct.ScaleY := scy;
    bsource := new TransformedBitmap(bsource.Source,sct);
  end);
end;


procedure DrawPixelsP(x,y:real; px,py,pw,ph: integer; a: array [,] of Color);
begin
  var (scalex,scaley) := ScaleToDevice;
  var bitmap := new WriteableBitmap(pw, ph, 96*scalex, 96*scaley, PixelFormats.Bgra32, nil);
  
  var stride := pw*4; // stride - это размер одной строки в байтах
  var size := stride*ph;
  
  
//  var pixels := new byte[w*h*4];
//  var p := 0;
//  for var dy := ay to ay+h-1 do
//    for var dx := ax to ax+w-1 do
//    begin
//      pixels[p] := a[dx,dy].B; p += 1;
//      pixels[p] := a[dx,dy].G; p += 1;
//      pixels[p] := a[dx,dy].R; p += 1;
//      pixels[p] := a[dx,dy].A; p += 1;
//    end;
//  bitmap.WritePixels(new Int32Rect(0, 0, w, h), pixels, stride, 0);
  
  
  //так на 10-20% быстрее
  var pixels := System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
  var curr_ptr := pixels;
  for var dy := py to py+ph-1 do
    for var dx := px to px+pw-1 do
    begin
      var c := a[dx,dy];
      PByte(curr_ptr.ToPointer)^ := c.B; curr_ptr := curr_ptr + 1;
      PByte(curr_ptr.ToPointer)^ := c.G; curr_ptr := curr_ptr + 1;
      PByte(curr_ptr.ToPointer)^ := c.R; curr_ptr := curr_ptr + 1;
      PByte(curr_ptr.ToPointer)^ := c.A; curr_ptr := curr_ptr + 1;
    end;
  bitmap.WritePixels(new Int32Rect(0, 0, pw, ph), pixels, size, stride);
  System.Runtime.InteropServices.Marshal.FreeHGlobal(pixels);
  
  var dc := GetDC();
  dc.DrawImage(bitmap, Rect(x, y, pw, ph));
  ReleaseDC(dc);
end;

procedure DrawImagePB(x,y: real; b: Bitmap);
begin
  var dc := GetDC();
  var img := b.bsource;
  dc.DrawImage(img, Rect(x, y, img.PixelWidth, img.PixelHeight));
  ReleaseDC(dc);
end;

procedure DrawImageP(x,y: real; fname: string);
begin
  var dc := GetDC();
  var img := GetBitmapImage(fname);
  dc.DrawImage(img, Rect(x, y, img.PixelWidth, img.PixelHeight));
  ReleaseDC(dc);
end;

procedure DrawImageWHPB(x,y,w,h: real; b: Bitmap);
begin
  var dc := GetDC();
  var img := b.bsource;
  dc.DrawImage(img, Rect(x, y, w, h));
  ReleaseDC(dc);
end;

procedure DrawImageWHP(x,y,w,h: real; fname: string);
begin
  var dc := GetDC();
  var img := GetBitmapImage(fname);
  dc.DrawImage(img, Rect(x, y, w, h));
  ReleaseDC(dc);
end;

procedure DrawImageUnscaledP(x,y: real; fname: string);
begin
  var dc := GetDC();
  var (scalex,scaley) := ScaleToDevice;
  var img := GetBitmapImage(fname);
  dc.DrawImage(img, Rect(x, y, img.PixelWidth/scalex, img.PixelHeight/scaley));
  ReleaseDC(dc);
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
  ReleaseDC(dc);
end;

procedure DrawPolygonOrPolylineDC(dc: DrawingContext; b: GBrush; p: GPen; points: array of Point; draw_polygon: boolean);
begin
  var geo := new StreamGeometry();
  geo.FillRule := FillRule.EvenOdd;

  var context: StreamGeometryContext := geo.Open();
  context.BeginFigure(Pnt(points[0].X,points[0].Y), true, draw_polygon);
  context.PolyLineTo(points.Select(p->Pnt(p.x,p.y)).Skip(1).ToArray(), true, false);
  context.Close;   
  
  dc.DrawGeometry(b, p, geo);
end;

procedure DrawPolygonOrPolyline(Self: DrawingContext; b: GBrush; p: GPen; points: array of Point; draw_polygon: boolean); extensionmethod;
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
  
procedure operator+=(var p: Point; v: Vector); extensionmethod := p := new Point(p.x+v.x,p.y+v.y);
  
procedure PolyLinePFull(points: array of Point; p: GPen);
begin
  var dc := GetDC();
  dc.DrawPolyline(p, points);
  ReleaseDC(dc);
end;

procedure PolygonPFull(points: array of Point; b: GBrush; p: GPen);
begin
  var dc := GetDC();
  dc.DrawPolygon(b, p, points);
  ReleaseDC(dc);
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
  ReleaseDC(dc);
end;

/// Нормированный вектор
function Norm(Self: Vector): Vector; extensionmethod := Self/Self.Length;

/// Вектор, ортогональный данному
function Ortog(Self: Vector): Vector; extensionmethod := Vect(Self.Y, -Self.X);

procedure ArrowPFull(x,y,x1,y1: real; p: GPen);
begin
  var szAlong := Parameters.ArrowSizeAlong / GlobalScale;
  var szAcross := Parameters.ArrowSizeACross / GlobalScale;
  
  var (p1,p2) := (Pnt(x,y),Pnt(x1,y1));
  var v := p2 - p1;
  var (vnorm, vortognorm) := (v.Norm, v.Ortog.Norm);
  
  var p3 := p2 - vnorm * szAlong + vortognorm * szAcross;
  var p4 := p2 - vnorm * szAlong - vortognorm * szAcross;
  
  var dc := GetDC();
  dc.DrawLine(p, p1, p2);
  
  dc.DrawPolygon(Brushes.Black,p,Arr(p2,p3,p4));
  ReleaseDC(dc);
end;

procedure SetPixel(x,y: real; c: Color) := InvokeVisual(SetPixelP, x, y, c);

procedure SetPixels(x,y: real; w,h: integer; f: (integer,integer)->Color)
  := InvokeVisual(SetPixelsP, x, y, w, h, f);
  
procedure DrawPixels(x,y: real; pixels: array [,] of Color) := InvokeVisual(DrawPixelsP,x,y,0,0,pixels.GetLength(0),pixels.GetLength(1),pixels);

procedure DrawPixels(x,y: real; pixels: array [,] of Color; px,py,pw,ph: integer) := InvokeVisual(DrawPixelsP,x,y,px,py,pw,ph,pixels);

procedure ArcPFull(x, y, r, angle1, angle2: real; p: GPen) := ArcSectorPFull(x, y, r, angle1, angle2, nil, p, false);

procedure SectorPFull(x, y, r, angle1, angle2: real; b: GBrush; p: GPen) := ArcSectorPFull(x, y, r, angle1, angle2, b, p, true);

procedure EllipseP(x,y,r1,r2: real) := EllipsePFull(x,y,r1,r2,Brush.BrushConstruct,Pen.PenConstruct);
procedure DrawEllipseP(x,y,r1,r2: real) := EllipsePFull(x,y,r1,r2,nil,Pen.PenConstruct);
procedure FillEllipseP(x,y,r1,r2: real) := EllipsePFull(x,y,r1,r2,Brush.BrushConstruct,nil);
procedure EllipsePC(x,y,r1,r2: real; c: GColor) := EllipsePFull(x,y,r1,r2,GetBrush(c),Pen.PenConstruct);
procedure DrawEllipsePC(x,y,r1,r2: real; c: GColor) := EllipsePFull(x,y,r1,r2,nil,ColorPen(c));
procedure FillEllipsePC(x,y,r1,r2: real; c: GColor) := EllipsePFull(x,y,r1,r2,GetBrush(c),nil);

procedure RectangleP(x,y,w,h: real) := RectanglePFull(x,y,w,h,Brush.BrushConstruct,Pen.PenConstruct);
procedure DrawRectangleP(x,y,w,h: real) := RectanglePFull(x,y,w,h,nil,Pen.PenConstruct);
procedure FillRectangleP(x,y,w,h: real) := RectanglePFull(x,y,w,h,Brush.BrushConstruct,nil);
procedure RectanglePC(x,y,r1,r2: real; c: GColor) := RectanglePFull(x,y,r1,r2,GetBrush(c),Pen.PenConstruct);
procedure DrawRectanglePC(x,y,r1,r2: real; c: GColor) := RectanglePFull(x,y,r1,r2,nil,ColorPen(c));
procedure FillRectanglePC(x,y,r1,r2: real; c: GColor) := RectanglePFull(x,y,r1,r2,GetBrush(c),nil);

procedure ArcP(x, y, r, angle1, angle2: real) := ArcPFull(x, y, r, angle1, angle2, Pen.PenConstruct);
procedure ArcPC(x, y, r, angle1, angle2: real; c: GColor) := ArcPFull(x, y, r, angle1, angle2, ColorPen(c));

procedure SectorP(x, y, r, angle1, angle2: real) := SectorPFull(x, y, r, angle1, angle2, Brush.BrushConstruct, Pen.PenConstruct);
procedure DrawSectorP(x, y, r, angle1, angle2: real) := SectorPFull(x, y, r, angle1, angle2, nil, Pen.PenConstruct);
procedure FillSectorP(x, y, r, angle1, angle2: real) := SectorPFull(x, y, r, angle1, angle2, Brush.BrushConstruct, nil);
procedure SectorPC(x, y, r, angle1, angle2: real; c: GColor) := SectorPFull(x, y, r, angle1, angle2, GetBrush(c), Pen.PenConstruct);
procedure DrawSectorPC(x, y, r, angle1, angle2: real; c: GColor) := SectorPFull(x, y, r, angle1, angle2, nil, ColorPen(c));
procedure FillSectorPC(x, y, r, angle1, angle2: real; c: GColor) := SectorPFull(x, y, r, angle1, angle2, GetBrush(c), nil);

procedure LineP(x,y,x1,y1: real) := LinePFull(x,y,x1,y1,Pen.PenConstruct);
procedure LinePC(x,y,x1,y1: real; c: GColor) := LinePFull(x,y,x1,y1,ColorPen(c));
procedure PolyLineP(points: array of Point) := PolyLinePFull(points,Pen.PenConstruct);
procedure PolyLinePC(points: array of Point; c: GColor) := PolyLinePFull(points,ColorPen(c));

procedure ArrowP(x,y,x1,y1: real) := ArrowPFull(x,y,x1,y1,Pen.PenConstruct);
procedure ArrowPC(x,y,x1,y1: real; c: GColor) := ArrowPFull(x,y,x1,y1,ColorPen(c));

procedure PolygonP(points: array of Point) := PolygonPFull(points,Brush.BrushConstruct,Pen.PenConstruct);
procedure DrawPolygonP(points: array of Point) := PolygonPFull(points,nil,Pen.PenConstruct);
procedure FillPolygonP(points: array of Point) := PolygonPFull(points,Brush.BrushConstruct,nil);
procedure PolygonPC(points: array of Point; c: GColor) := PolygonPFull(points,ColorBrush(c),Pen.PenConstruct);
procedure DrawPolygonPC(points: array of Point; c: GColor) := PolygonPFull(points,nil,ColorPen(c));
procedure FillPolygonPC(points: array of Point; c: GColor) := PolygonPFull(points,ColorBrush(c),nil);

procedure DrawTextP(x,y: real; text: string; angle,x0,y0: real) := TextPFull(x,y,text,angle,x0,y0);
procedure DrawTextPC(x,y: real; text: string; angle,x0,y0: real; c: GColor) := TextPFull(x,y,text,angle,x0,y0,c);
procedure DrawTextPFont(x,y: real; text: string; angle,x0,y0: real; f: FontOptions) := TextPFull(x,y,text,angle,x0,y0,f);

procedure EllipseNew(x,y,r1,r2: real) 
  := InvokeVisual(DrawGeometryP,VE.Create(()->EllipseGeometry.Create(Pnt(x,y),r1,r2)));

// Реализация примитивов
procedure Ellipse(x,y,rx,ry: real) := InvokeVisual(EllipseP,x,y,rx,ry);
procedure DrawEllipse(x,y,rx,ry: real) := InvokeVisual(DrawEllipseP,x,y,rx,ry);
procedure FillEllipse(x,y,rx,ry: real) := InvokeVisual(FillEllipseP,x,y,rx,ry);
procedure Ellipse(x,y,rx,ry: real; c: GColor) := InvokeVisual(EllipsePC,x,y,rx,ry,c);
procedure DrawEllipse(x,y,rx,ry: real; c: GColor) := InvokeVisual(DrawEllipsePC,x,y,rx,ry,c);
procedure FillEllipse(x,y,rx,ry: real; c: GColor) := InvokeVisual(FillEllipsePC,x,y,rx,ry,c);

procedure Ellipse(p: Point; rx,ry: real) := Ellipse(p.X,p.Y,rx,ry);
procedure DrawEllipse(p: Point; rx,ry: real) := DrawEllipse(p.X,p.Y,rx,ry);
procedure FillEllipse(p: Point; rx,ry: real) := FillEllipse(p.X,p.Y,rx,ry);
procedure Ellipse(p: Point; rx,ry: real; c: Color) := Ellipse(p.X,p.Y,rx,ry,c);
procedure DrawEllipse(p: Point; rx,ry: real; c: Color) := DrawEllipse(p.X,p.Y,rx,ry,c);
procedure FillEllipse(p: Point; rx,ry: real; c: Color) := FillEllipse(p.X,p.Y,rx,ry,c);

procedure Circle(x,y,r: real) := InvokeVisual(EllipseP,x,y,r,r);
procedure DrawCircle(x,y,r: real) := InvokeVisual(DrawEllipseP,x,y,r,r);
procedure FillCircle(x,y,r: real) := InvokeVisual(FillEllipseP,x,y,r,r);
procedure Circle(x,y,r: real; c: GColor) := InvokeVisual(EllipsePC,x,y,r,r,c);
procedure DrawCircle(x,y,r: real; c: GColor) := InvokeVisual(DrawEllipsePC,x,y,r,r,c);
procedure FillCircle(x,y,r: real; c: GColor) := InvokeVisual(FillEllipsePC,x,y,r,r,c);

procedure Circle(p: Point; r: real) := Circle(p.X,p.Y,r);
procedure DrawCircle(p: Point; r: real) := DrawCircle(p.X,p.Y,r);
procedure FillCircle(p: Point; r: real) := FillCircle(p.X,p.Y,r);
procedure Circle(p: Point; r: real; c: Color) := Circle(p.X,p.Y,r,c);
procedure DrawCircle(p: Point; r: real; c: Color) := DrawCircle(p.X,p.Y,r,c);
procedure FillCircle(p: Point; r: real; c: Color) := FillCircle(p.X,p.Y,r,c);


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
procedure Pie(x, y, r, angle1, angle2: real; c: GColor) := InvokeVisual(SectorPC,x, y, r, angle1, angle2, c);
procedure DrawSector(x, y, r, angle1, angle2: real; c: GColor) := InvokeVisual(DrawSectorPC,x, y, r, angle1, angle2, c);
procedure FillSector(x, y, r, angle1, angle2: real; c: GColor) := InvokeVisual(FillSectorPC,x, y, r, angle1, angle2, c);

procedure Line(x,y,x1,y1: real) := InvokeVisual(LineP,x,y,x1,y1);
procedure Line(x,y,x1,y1: real; c: GColor) := InvokeVisual(LinePC,x,y,x1,y1,c);
procedure Line(p,p1: Point) := Line(p.x,p.y,p1.x,p1.y);
procedure Line(p,p1: Point; c: Color) := Line(p.x,p.y,p1.x,p1.y,c);
procedure Lines(a: array of (Point,Point)) := foreach var i in a.Indices do Line(a[i].Item1,a[i].Item2);
procedure Lines(a: array of (Point,Point); c: Color) := foreach var i in a.Indices do Line(a[i].Item1,a[i].Item2,c);

procedure Arrow(x,y,x1,y1: real) := InvokeVisual(ArrowP,x,y,x1,y1);
procedure Arrow(x,y,x1,y1: real; c: GColor) := InvokeVisual(ArrowPC,x,y,x1,y1,c);
procedure Arrow(p,p1: Point) := Arrow(p.x,p.y,p1.x,p1.y);
procedure Arrow(p,p1: Point; c: Color) := Arrow(p.x,p.y,p1.x,p1.y,c);


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
procedure MoveBy(dx,dy: real) := MoveRel(dx,dy);
procedure LineBy(dx,dy: real) := LineRel(dx,dy);

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

procedure DrawImage(x,y: real; b: Bitmap) := InvokeVisual(DrawImagePB,x,y,b);
procedure DrawImage(x,y,w,h: real; b: Bitmap) := InvokeVisual(DrawImageWHPB,x,y,w,h,b);

procedure DrawImageUnscaled(x,y: real; fname: string) := InvokeVisual(DrawImageUnscaledP,x,y,fname);
procedure DrawVideo(x,y,w,h: real; fname: string) := InvokeVisual(DrawVideoP,x,y,w,h,fname);

/// Ширина текста при выводе
function TextWidth(text: string) := InvokeReal(TextV.Create(text).TextWidth);
/// Высота текста при выводе
function TextHeight(text: string) := InvokeReal(TextV.Create(text).TextHeight);
/// Размер текста при выводе
function TextSize(text: string): Size := Invoke&<Size>(TextV.Create(text).TextSize);

function TextWidth(text: string; f: FontOptions): real := InvokeReal(()->TextV.Create(text).TextWidthFont(f));

function TextHeight(text: string; f: FontOptions): real := InvokeReal(()->TextV.Create(text).TextHeightFont(f)); 

function TextSize(text: string; f: FontOptions): Size := Invoke&<Size>(()->TextV.Create(text).TextSizeFont(f));

procedure TextOutHelper(x,y: real; text: string; angle: real; x0,y0: real) := InvokeVisual(DrawTextP,x,y,text,angle,x0,y0);
//procedure TextOut(x,y: real; number: integer) := TextOut(x,y,'' + number);
//procedure TextOut(x,y: real; number: real) := TextOut(x,y,'' + number);
procedure TextOutHelper(x,y: real; text: string; angle: real; c: GColor; x0,y0: real) := InvokeVisual(DrawTextPC,x,y,text,angle,x0,y0,c);
//procedure TextOut(x,y: real; number: integer; c: GColor) := TextOut(x,y,'' + number,c);
//procedure TextOut(x,y: real; number: real; c: GColor) := TextOut(x,y,'' + number,c);
procedure TextOutHelper(x,y: real; text: string; angle: real; x0,y0: real; f: FontOptions) := InvokeVisual(DrawTextPFont,x,y,text,angle,x0,y0,f);

procedure DrawTextHelper(var x, y, x0, y0: real; w, h: real; sz: Size; align: Alignment; f: FontOptions);
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
  var dw,dh: real;
  if CurrentCoordType = StandardCoords then
    (dw,dh) := ((w-sz.Width)/2,(h-sz.Height)/2)
  else 
  begin
    var (szw,szh) := (sz.Width/GlobalScale,sz.Height/GlobalScale);
    (dw,dh) := ((w-szw)/2,(h-szh)/2);
    dh := h-dh;
  end;
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

procedure DrawText(x, y, w, h: real; text: object; align: Alignment; angle: real);
begin
  var (x0,y0) := (x,y);
  
  var textstr := text.ToString;
  
  var ft := FormTextFont(textstr,Font);
  var sz := new Size(ft.Width,ft.Height);

  DrawTextHelper(x, y, x0, y0, w, h, sz, align);
  TextOutHelper(x,y,textstr,angle,x0,y0)
end;

procedure DrawText(x, y, w, h: real; text: object; color: GColor; align: Alignment; angle: real);
begin
  var (x0,y0) := (x,y);

  var textstr := text.ToString;
  
  var ft := FormTextFont(textstr,Font);
  var sz := new Size(ft.Width,ft.Height);

  DrawTextHelper(x, y, x0, y0, w, h, sz, align);
  TextOutHelper(x,y,textstr,angle,color,x0,y0)
end;

procedure DrawText(rect: GRect; text: object; align: Alignment; angle: real)
  := DrawText(rect.x,rect.y,rect.Width,rect.Height,text,align,angle);

procedure DrawText(rect: GRect; text: object; color: GColor; align: Alignment; angle: real)
  := DrawText(rect.x,rect.y,rect.Width,rect.Height,text,align,angle);

procedure DrawText(x, y, w, h: real; text: object; f: FontOptions; align: Alignment; angle: real);
begin
  var (x0,y0) := (x,y);
  
  var textstr := text.ToString;
  
  var ft := FormTextFont(textstr,Font);
  var sz := new Size(ft.Width,ft.Height);

  DrawTextHelper(x, y, x0, y0, w, h, sz, align, f);
  TextOutHelper(x,y,textstr,angle,x0,y0,f)
end;

{
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
}

(*
procedure TextOut(x, y: real; text: string; align: Alignment; angle: real) := DrawText(x, y, 0, 0, text,{ConvertAlign(}align{)},angle);
procedure TextOut(x, y: real; text: string; c: GColor; align: Alignment; angle: real) := DrawText(x, y, 0, 0, text, c,{ConvertAlign(}align{)},angle);
procedure TextOut(x, y: real; text: integer; align: Alignment; angle: real) := TextOut(x, y, ''+text,align,angle);
procedure TextOut(x, y: real; text: integer; c: GColor; align: Alignment; angle: real) := TextOut(x, y, ''+text, c,align,angle);
procedure TextOut(x, y: real; text: real; align: Alignment; angle: real) := TextOut(x, y, ''+text,align,angle);
procedure TextOut(x, y: real; text: real; c: GColor; align: Alignment; angle: real) := TextOut(x, y, ''+text, c,align,angle);
*)
procedure TextOut(x, y: real; text: object; align: Alignment; angle: real) 
  := DrawText(x, y, 0, 0, text.ToString, align, angle);
procedure TextOut(x, y: real; text: object; color: GColor; align: Alignment; angle: real) 
  := DrawText(x, y, 0, 0, text.ToString, color, align, angle);
procedure TextOut(x, y: real; text: object; f: FontOptions; align: Alignment; angle: real) 
  := DrawText(x, y, 0, 0, text.ToString, f, align, angle);
procedure TextOut(pos: Point; text: object; align: Alignment; angle: real) 
  := TextOut(pos.x, pos.y, text, align, angle);
procedure TextOut(pos: Point; text: object; color: GColor; align: Alignment; angle: real) 
  := TextOut(pos.x, pos.y, text, color, align, angle);
procedure TextOut(pos: Point; text: object; f: FontOptions; align: Alignment; angle: real) 
  := TextOut(pos.x, pos.y, text, f, align, angle);

type
  FS = auto class
    mx, my, a, b, min, max: real;
    x, y, w, h, XTicks, YTicks: real;
    f: real -> real;
    title: string;
    marginY := 6;
    marginX := 6;
    XTicksPrecision := 3;
    YTicksPrecision := 5;
    spaceBetweenTextAndGraph := 6;
    
    constructor (a, b, min, max, x, y, w, h: real; f: real -> real; XTicks: real := 1; YTicks: real := 1; title: string := '');
    begin
      Self.a := a.Round(5); Self.b := b.Round(5); 
      Self.min := min.Round(5); Self.max := max.Round(5);
      Self.x := x; Self.y := y; Self.w := w; Self.h := h; 
      Self.f := f; Self.XTicks := XTicks; Self.YTicks := YTicks;
      Self.title := title;
    end;
    
    function Ticks(d: real): real;
    begin
      var n := Floor(log10(d));
      var p := Power(10,n);
      var r := d / p;
      // r = 1 .. 10
      if r >= 5 then
        Result := p
      else if r >= 2 then  
        Result := p/2
      else Result := p/5
    end;
    
    procedure CorrectBounds;
    begin
      //var digits := 5;
      if real.IsNaN(XTicks) then
        XTicks := Ticks(b-a);
      if real.IsNaN(YTicks) then
        YTicks := Ticks(max-min);
      var th := TextHeightP('0');
      var tw := GetRY0.Step(YTicks).TakeWhile(ry -> ry <= max).Select(y -> TextWidthP(y.Round(YTicksPrecision).ToString)).Max;
      var dd := TextWidthP(b.Round(xTicksPrecision).ToString)/2;

      //var tw := TextWidth('-99.9');
      w -= marginX * 2 + spaceBetweenTextAndGraph + tw + dd;
      h -= marginY * 2 + spaceBetweenTextAndGraph + th;
      x += marginX + spaceBetweenTextAndGraph + tw; 
      y += marginY;
      if title<>'' then
      begin
        y += spaceBetweenTextAndGraph + th;
        h -= spaceBetweenTextAndGraph + th;
      end;
      mx := w / (b - a);
      my := h / (max - min);
    end;
    
    function Apply(xx: real) := Pnt(x + mx * (xx - a), y + my * (max - f(xx)));
    function RealToScreenX(xx: real) := x + mx * (xx - a);
    function RealToScreenY(yy: real) := y + h - my * (yy - min); // ?
    function ScreenToRealX(xx: real) := (xx - x) / mx + a;
    function ScreenToRealY(yy: real) := (y + h - yy) / my + min;
    
    function GetRX0: real;
    begin
      var xt := XTicks * Trunc(Abs(a)/XTicks);
      var rx0: real; 
      if a <= 0 then 
        rx0 := -xt 
      else rx0 := xt + XTicks;
      Result := rx0;
    end;
    
    function GetRY0: real;
    begin
      var yt := YTicks * Trunc(Abs(min)/YTicks);
      var ry0: real;
      if min <= 0 then 
        ry0 := -yt 
      else ry0 := yt + YTicks;
      Result := ry0;
    end;

    procedure DrawDC(dc: DrawingContext);
    var AxisColor := GrayColor(112);
    begin
      DrawRectangleDC(dc, x, y, w, h, ColorBrush(Colors.White), nil);
      CorrectBounds; // без Invoke!
      var sx := mx * XTicks;

      var rx0 := GetRX0;

      var x0 := RealToScreenX(rx0);
      var GridPen := ColorPen(Colors.LightGray);
      var AxisPen := ColorPen(AxisColor);
      while x0<=x+w+0.000001 do
      begin
        if Abs(rx0)<0.000001 then
          DrawLineDC(dc,x0,y,x0,y+h,AxisPen) 
        else DrawLineDC(dc,x0,y,x0,y+h,GridPen);
        TextOutDC(dc,x0,y+h+4,rx0.Round(XTicksPrecision).ToString,Alignment.CenterTop);
        x0 += sx;
        rx0 += XTicks;
      end;
      
      var sy := my * YTicks;
      
      var ry0 := GetRY0;
      
      if title<>'' then
        TextOutDC(dc,x+w/2,y-spaceBetweenTextAndGraph,Title,Alignment.CenterBottom,0,Font.WithSize(Font.Size*1.15));
      var y0 := RealToScreenY(ry0);
      
      while y0>=y-0.000001 do
      begin
        if Abs(ry0)<0.000001 then
          DrawLineDC(dc,x,y0,x+w,y0,AxisPen)
        else DrawLineDC(dc,x,y0,x+w,y0,GridPen);  
        TextOutDC(dc,x-4,y0,ry0.Round(YTicksPrecision).ToString,Alignment.RightCenter);
        y0 -= sy;
        ry0 += YTicks;
      end;
      
      DrawRectangleDC(dc, x, y, w, h, nil, ColorPen(Colors.Black));
    
      var n := Round(w / 1);
      if n<=0 then exit;
      var pp := PartitionPoints(a, b, n);
      var fff: real -> Point := xx -> Pnt(x + mx * (xx - a), y + my * (max - f(xx).Clamp(min,max)));
      //pp.Select(x->(x,f(x))).PrintLines;
      DrawPolylineDC(dc,pp.Select(fff).ToArray,ColorPen(Colors.Black));
    end;
    
    procedure Draw := FastDraw(DrawDC);
  end;

/// Рисует график функции f, заданной на отрезке [a,b] по оси абсцисс и на отрезке [min,max] по оси ординат, в прямоугольнике, задаваемом параметрами x,y,w,h 
procedure DrawGraph(f: real -> real; a, b, min, max, x, y, w, h: real; title: string);
begin
  DrawGraph(f,a,b,min,max,x,y,w,h,real.NaN,real.NaN,title); 
end;

/// Рисует график функции f, заданной на отрезке [a,b] по оси абсцисс и на отрезке [min,max] по оси ординат, в прямоугольнике, задаваемом параметрами x,y,w,h. Два последних параметра задают шаг сетки по OX и OY
procedure DrawGraph(f: real -> real; a, b, min, max, x, y, w, h, XTicks, YTicks: real; title: string);
begin
  var fso := new FS(a,b,min,max,x,y,w,h,f,XTicks,YTicks,title);
  fso.Draw;
end;

procedure DrawGraph(f: real -> real; a, b, min, max: real; r: GRect; title: string) 
  := DrawGraph(f, a, b, min, max, r.X, r.Y, r.Width, r.Height, title);
procedure DrawGraph(f: real -> real; a, b, min, max: real; r: GRect; XTicks, YTicks: real; title: string) 
  := DrawGraph(f, a, b, min, max, r.X, r.Y, r.Width, r.Height, XTicks, YTicks, title);  
procedure DrawGraph(f: real -> real; a, b, min, max: real; title: string) 
  := DrawGraph(f, a, b, min, max, Window.ClientRect, title);

procedure DrawGraphDC(dc: DrawingContext; f: real -> real; a, b, min, max, x, y, w, h, XTicks, YTicks: real; title: string);
begin
  var fso := new FS(a,b,min,max,x,y,w,h,f,XTicks,YTicks,title);
  fso.DrawDC(dc);
end;

procedure DrawGraphDC(dc: DrawingContext; f: real -> real; a, b, min, max, x, y, w, h: real; title: string)
  := DrawGraphDC(dc,f,a,b,min,max,x,y,w,h,real.NaN,real.NaN,title); 

procedure DrawGraphDC(dc: DrawingContext; f: real -> real; a, b: real; x, y, w, h: real; title: string);
begin
  var n := Round(w / 1);
  if n<=0 then exit;
  var q := PartitionPoints(a, b, n).ToArray;
  var mi := q.Min(f);
  var ma := q.Max(f);
  DrawGraphDC(dc, f, a, b, mi, ma, x, y, w, h, title);
end;

procedure DrawGraph(f: real -> real; a, b: real; x, y, w, h: real; title: string)
  := FastDraw(dc->DrawGraphDC(dc,f,a,b,x,y,w,h,title));

procedure DrawGraph(f: real -> real; a, b: real; r: GRect; title: string) := DrawGraph(f, a, b, r.X, r.Y, r.Width, r.Height, title);
procedure DrawGraph(f: real -> real; r: GRect; title: string) := DrawGraph(f, -5, 5, r, title);
procedure DrawGraph(f: real -> real; a, b: real; title: string) := DrawGraph(f, a, b, 0, 0, GraphWindow.Width - 1, GraphWindow.Height - 1, title);
procedure DrawGraph(f: real -> real; title: string) := DrawGraph(f, -5, 5, title);

function GraphWindowTypeGetLeftP: real;
begin
  Result := 0;
  foreach var p in MainDockPanel.Children do
    if (p is FrameworkElement) and (p<>host1) then
    begin
      var d := DockPanel.GetDock(FrameworkElement(p));
      if d=Dock.Left then
      begin
        var f := FrameworkElement(p).Width;
        if real.IsNaN(f) then
          continue;
        Result += f;
      end;    
    end;
end;

function GraphWindowTypeGetTopP: real;
begin
  Result := 0;
  foreach var p in MainDockPanel.Children do
    if (p is FrameworkElement) and (p<>host1) then
    begin
      var d := DockPanel.GetDock(FrameworkElement(p));
      if d=Dock.Top then
      begin
        var f := FrameworkElement(p).Height;
        if real.IsNaN(f) then
          continue;
        Result += f;
      end;    
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
    if (p is FrameworkElement) and (p<>host1) then
    begin
      var d := DockPanel.GetDock(FrameworkElement(p));
      if (d=Dock.Left) or (d=Dock.Right) then
      begin
        var f := FrameworkElement(p).Width;
        //Print(f);
        if real.IsNaN(f) then
          continue;
        Result -= f;
      end;  
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
    if (p is FrameworkElement) and (p<>host1) then
    begin
      var d := DockPanel.GetDock(FrameworkElement(p));
      if (d=Dock.Top) or (d=Dock.Bottom) then
      begin  
        var f := FrameworkElement(p).Height;
        if real.IsNaN(f) then
          continue;
        Result -= f;
      end;  
    end;
end;
function GraphWindowType.GetHeight := InvokeReal(GraphWindowTypeGetHeightP);

procedure SaveWindowP(canvas: FrameworkElement; filename: string);
begin
  HostToRenderBitmap;
  var (scalex,scaley) := ScaleToDevice;
  var (dpiX,dpiY) := (scalex * 96, scaley * 96);
  
  var sz := Size(host.DataContext);
  
  if sz.Width = 0 then 
    sz.Width := GraphWindow.Width;
  if sz.Height = 0 then 
    sz.Height := GraphWindow.Height;
  
  var bmp := new RenderTargetBitmap(Round(sz.Width*scalex), Round(sz.Height*scaley), dpiX, dpiY, PixelFormats.Pbgra32);

  var myvis := new DrawingVisual();
  var dc := myvis.RenderOpen;
  var r := Rect(0,0,Window.Width,Window.Height);
  var mm := (canvas as MyVisualHost).RenderTransform.Value;
  var m1 := mm;
  mm.Invert;
  r.Transform(mm);
  dc.DrawRectangle(Brushes.White,nil,r);
  
  var rr: GRect;
  if CurrentCoordType = MathematicalCoords then
    rr := Rect(r.Left,r.Top,rtbmap.Width/m1.M11,rtbmap.Height/-m1.M22)
  else rr := Rect(0,0,rtbmap.Width,rtbmap.Height);
  dc.DrawImage(rtbmap,rr);
  
  if AdditionalDrawOnDC <> nil then
    AdditionalDrawOnDC(dc);
  dc.Close;
  (canvas as MyVisualHost).children.Insert(0,myvis);
  
  bmp.Render(canvas);
  
  dpic[filename] := bmp;

  (canvas as MyVisualHost).children.RemoveAt(0);
  
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

procedure GraphWindowType.Clear;
begin
  Window.Clear;
end;


function GraphWindowType.RandomPoint(margin: real): Point := Pnt(Random(margin,Width-margin),Random(margin,Height-margin));

function GraphWindowType.Center: Point := Pnt(Width/2,Height/2);

function GraphWindowType.ClientRect: GRect := Rect(0,0,Width,Height);


procedure WindowTypeWPF.Save(fname: string) := GraphWindow.Save(fname);

procedure WindowTypeWPF.Load(fname: string) := GraphWindow.Load(fname);

procedure WindowTypeClearP;
begin 
  Host.children.Clear; 
  if not rtbmapIsCleared then
  begin
    rtbmap.Clear; 
    rtbmapIsCleared := True;
  end;  
end;

procedure WindowTypeClearPC(c: Color);
begin 
  Host.children.Clear; 
  if not rtbmapIsCleared then
  begin
    rtbmap.Clear; 
    rtbmapIsCleared := True;
  end;  
  FillRectangle(0,0,Window.Width,Window.Height,c)
end;

procedure WindowTypeWPF.Clear := Invoke(WindowTypeClearP);

procedure WindowTypeWPF.Clear(c: Color) := Invoke(WindowTypeClearPC,c);


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

procedure SetMathematicCoordsP(x1,x2: real; drawgrid: boolean);
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
  if drawgrid then
    DrawGridP
end;

procedure SetMathematicCoordsP1(x1,x2,ymin: real; drawgrid: boolean);
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
  if drawgrid then
    DrawGridP
end;

procedure SetMathematicCoords(x1: real; x2: real; drawgrid: boolean) := Invoke(SetMathematicCoordsP,x1,x2,drawgrid);
procedure SetMathematicCoords(x1,x2,ymin: real; drawgrid: boolean) := Invoke(SetMathematicCoordsP1,x1,x2,ymin,drawgrid);

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

function GetMouseArgs(e: MouseEventArgs): (Point,integer);
begin
  var mb := 0;
  var p := e.GetPosition(host);
  if e.LeftButton = MouseButtonState.Pressed then
    mb := 1
  else if e.RightButton = MouseButtonState.Pressed then
    mb := 2;
  Result := (p,mb);  
end;

/// --- SystemMouseEvents
procedure SystemOnMouseDown(sender: Object; e: MouseButtonEventArgs);
begin
  var (p,mb) := GetMouseArgs(e);
  if OnMouseDown <> nil then  
    OnMouseDown(p.x, p.y, mb);
end;

procedure SystemOnMouseUp(sender: Object; e: MouseButtonEventArgs);
begin
  var (p,mb) := GetMouseArgs(e);
  if OnMouseUp <> nil then  
    OnMouseUp(p.x, p.y, mb);
end;

procedure SystemOnMouseMove(sender: Object; e: MouseEventArgs);
begin
  var (p,mb) := GetMouseArgs(e);
  if OnMouseMove <> nil then  
    OnMouseMove(p.x, p.y, mb);
end;

procedure SystemOnMouseWheel(sender: Object; e: MouseWheelEventArgs);
begin
  var delta := e.Delta;
  if OnMouseWheel <> nil then  
    OnMouseWheel(delta);
end;

/// --- SystemKeyEvents
procedure SystemOnKeyDown(sender: Object; e: KeyEventArgs) := 
  if OnKeyDown<>nil then
    OnKeyDown(e.Key);

procedure SystemOnKeyUp(sender: Object; e: KeyEventArgs) := 
  if OnKeyUp<>nil then
    OnKeyUp(e.Key);
    
procedure SystemOnKeyPress(sender: Object; e: TextCompositionEventArgs) := 
  if (OnKeyPress<>nil) and (e.Text<>nil) and (e.Text.Length>0) then
    OnKeyPress(e.Text[1]);
    
procedure SystemOnResize(sender: Object; e: SizeChangedEventArgs) := 
  if OnResize<>nil then
    OnResize();
  
//----------------------------------------------------------------------
// Процедуры с DrawingContext
//----------------------------------------------------------------------

procedure DrawLineDC(dc: DrawingContext; x,y,x1,y1: real; c: Color; w: real)
  := dc.DrawLine(ColorPen(c,w),Pnt(x,y),Pnt(x1,y1));
  
procedure DrawLineDC(dc: DrawingContext; x,y,x1,y1: real; p: GPen)
  := dc.DrawLine(p,Pnt(x,y),Pnt(x1,y1));  
  
procedure DrawEllipseDC(dc: DrawingContext; x,y,rx,ry: real; cbrush,cpen: Color; w: real)
  := dc.DrawEllipse(ColorBrush(cbrush),ColorPen(cpen,w),Pnt(x,y),rx,ry);

procedure DrawEllipseDC(dc: DrawingContext; x,y,rx,ry: real; b: GBrush; p: GPen)
  := dc.DrawEllipse(b,p,Pnt(x,y),rx,ry);
  
procedure DrawRectangleDC(dc: DrawingContext; x,y,width,height: real; cbrush,cpen: Color; w: real)
  := dc.DrawRectangle(ColorBrush(cbrush),ColorPen(cpen,w),Rect(x,y,width,height));

procedure DrawRectangleDC(dc: DrawingContext; x,y,width,height: real; b: GBrush; p: GPen)
  := dc.DrawRectangle(b,p,Rect(x,y,width,height));

// Не в интерфейсе
procedure DrawPolygonDC(dc: DrawingContext; b: GBrush; p: GPen; points: array of Point)
  := DrawPolygonOrPolylineDC(dc,b,p,points,true);

// Не в интерфейсе
procedure DrawPolylineDC(dc: DrawingContext; p: GPen; points: array of Point)
  := DrawPolygonOrPolylineDC(dc,nil,p,points,false);

procedure DrawPolygonDC(dc: DrawingContext; pnt: array of Point; cbrush,cpen: Color; w: real)
  := DrawPolygonDC(dc,ColorBrush(cbrush),ColorPen(cpen,w),pnt);

procedure DrawPolygonDC(dc: DrawingContext; pnt: array of Point; b: GBrush; p: GPen)
  := DrawPolygonDC(dc,b,p,pnt);

procedure DrawPolylineDC(dc: DrawingContext; pnt: array of Point; cpen: Color; w: real)
  := DrawPolylineDC(dc,ColorPen(cpen,w),pnt);

procedure DrawPolylineDC(dc: DrawingContext; pnt: array of Point; p: GPen)
  := DrawPolylineDC(dc,p,pnt);

///----------------------------------------------------------------------

var OnDraw: procedure;
var OnDraw1: procedure(frame: integer);

var FrameRate := 61; // кадров в секунду. Можно меньше!
var LastUpdatedTime := new System.TimeSpan(integer.MinValue); 

var FrameNum := 0;

procedure RenderFrame(o: Object; e: System.EventArgs);
begin
  if (OnDraw<>nil) or (OnDraw1<>nil) or (OnDrawFrame<>nil) then
  begin
    var e1 := RenderingEventArgs(e).RenderingTime;
    if LastUpdatedTime.Ticks = integer.MinValue then // первый раз
      LastUpdatedTime := e1;
    var dt := e1 - LastUpdatedTime;
    var delta := 1000/Framerate; // через какое время обновлять
    if OnDrawFrame<>nil then 
      delta := 0; // перерисовывать когда придёт время
    if dt.TotalMilliseconds < delta then
      exit
    else LastUpdatedTime := e1;  
    FrameNum += 1;
    Window.Clear;
    if OnDraw<>nil then
      OnDraw() 
    else if OnDraw1<>nil then
      OnDraw1(FrameNum)
    else if OnDrawFrame<>nil then
      OnDrawFrame(dt.Milliseconds/1000);
  end;  
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

procedure BeginFrameBasedAnimationTime(Draw: procedure(dt: real));
begin
  OnDrawFrame := Draw;
end;

procedure EndFrameBasedAnimation;
begin
  //CountVisuals := 0;
  OnDraw := nil;
  OnDraw1 := nil;
  FrameRate := 61;
end;  

function CreateRenderTargetBitmap: RenderTargetBitmap;
begin
  var dpiXProperty := typeof(SystemParameters).GetProperty('DpiX', BindingFlags.NonPublic or BindingFlags.Static);
  var dpiYProperty := typeof(SystemParameters).GetProperty('Dpi', BindingFlags.NonPublic or BindingFlags.Static);
  
  var dpiX := integer(dpiXProperty.GetValue(nil, nil));
  var dpiY := integer(dpiYProperty.GetValue(nil, nil));

  var (scalex, scaley) := (dpiX/96,dpiY/96);
  Result := new RenderTargetBitmap(Round(SystemParameters.PrimaryScreenWidth * scalex), Round(SystemParameters.PrimaryScreenHeight * scaley), dpiX, dpiY, PixelFormats.Pbgra32);
end;

var mre := new ManualResetEvent(false);

type 
GraphWPFWindow = class(GMainWindow)
public
  procedure InitMainGraphControl; override;
  begin
    var g := Content as DockPanel;

    host1 := new Canvas;
    host := new MyVisualHost();
    // Попытка отсекать рисование частей объектов за пределами host. Актуально при наличии панелей.
    // К сожалению, почему-то при использовании WPFObjects объекты GraphWPF становятся не видны
    // Выход - в WPFObjects host1.ClipToBounds := False
    host1.ClipToBounds := True;
    host1.SizeChanged += (s,e) ->
    begin
      var sz := e.NewSize;
      host.DataContext := sz;
    end;
    // Всегда последнее
    
    RTbmap := CreateRenderTargetBitmap;

    var im := new Image();
    im.Source := RTbmap;
    
    // Рисуем на host
    // Когда накопится много объектов, переносим их на im
    host1.Children.Add(im); 
    host1.Children.Add(host);
    
    g.children.Add(host1);
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
    Font := new FontOptions;
    Window := new WindowTypeWPF;
    GraphWindow := new GraphWindowType;
  end;
  
  procedure InitHandlers; override;
  begin
    AdditionalDrawOnDC := procedure(dc) -> begin end;
    Closed += (sender,e) -> begin 
      if OnClose<>nil then
        OnClose;
      Halt; 
    end;
    MouseDown += SystemOnMouseDown;
    MouseUp += SystemOnMouseUp;
    MouseMove += SystemOnMouseMove;
    MouseWheel += SystemOnMouseWheel;
    KeyDown += SystemOnKeyDown;
    KeyUp += SystemOnKeyUp;
    TextInput += SystemOnKeyPress;
    SizeChanged += SystemOnResize;
    
    CompositionTarget.Rendering += RenderFrame;
    
    Loaded += (o,e) -> mre.Set();
  end;

end;

procedure InitApp;
begin
  app := new Application;
  
  app.Dispatcher.UnhandledException += (o, e) -> begin
    Println(e.Exception.Message); 
    if e.Exception.InnerException <> nil then
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
    GraphWPFBase.__InitModule__;
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