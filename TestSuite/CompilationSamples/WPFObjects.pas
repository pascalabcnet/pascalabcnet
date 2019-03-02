// Copyright (©) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///Модуль векторных графических объектов на основе WPF
unit WPFObjects;

interface

uses GraphWPFBase,GraphWPF;

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

//{{{doc: Начало секции 1 }}} 

type 
// -----------------------------------------------------
//>>     Типы модуля WPFObjects # WPFObjects types
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
  GPoint = System.Windows.Point;
  /// Тип кисти
  GBrush = System.Windows.Media.Brush;
  /// Тип стиля шрифта
  FontStyle = (Normal,Bold,Italic,BoldItalic);
  
var host: Canvas;

// -----------------------------------------------------
//>>     Вспомогательные функции WPFObjects # WPFObjects functions 1
// -----------------------------------------------------

/// Возвращает цвет по красной, зеленой и синей составляющей (в диапазоне 0..255)
function RGB(r,g,b: byte): Color;
/// Возвращает цвет по красной, зеленой и синей составляющей и параметру прозрачности (в диапазоне 0..255)
function ARGB(a,r,g,b: byte): Color;
/// Возвращает случайный цвет
function RandomColor: Color;
/// Возвращает серый цвет с интенсивностью b
function GrayColor(b: byte): Color;
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
//{{{--doc: Конец секции 1 }}} 

procedure Invoke(p: ()->());

//{{{doc: Начало секции 2 }}} 
type
  ObjectWPF = class;
// -----------------------------------------------------
//>>     Класс списка графических объектов # Class List of objects
// -----------------------------------------------------
  ///!#
  /// Класс списка графических объектов
  ObjectsType = class
  private
    l := new List<ObjectWPF>;
    d := new Dictionary<FrameworkElement,ObjectWPF>;
    procedure AddP(ob: ObjectWPF); 
    procedure DeleteP(ob: ObjectWPF);
    procedure ToBackP(ob: ObjectWPF); 
    procedure ToFrontP(ob: ObjectWPF);
    function GetItem(i: integer): ObjectWPF := l[i];
    procedure SetItem(i: integer; value: ObjectWPF) := l[i] := value;
    procedure Add(ob: ObjectWPF) := Invoke(AddP,ob);
    procedure Destroy(ob: ObjectWPF) := Invoke(DeleteP,ob);
  public
    /// Перемещает объект на задний план
    procedure ToBack(ob: ObjectWPF) := Invoke(ToBackP,ob);
    /// Перемещает объект на передний план
    procedure ToFront(ob: ObjectWPF) := Invoke(ToFrontP,ob);
    /// Возвращает количество объектов ObjectWPF
    property Count: integer read l.Count;
    /// Возвращает или устанавливает i-тый объект ObjectWPF
    property Items[i: integer]: ObjectWPF read GetItem write SetItem; default;
  end;


// -----------------------------------------------------
//>>     Класс ObjectWPF # Class ObjectWPF 
// -----------------------------------------------------
  /// Перечислимый тип выравнивания текста в свойстве Text или Number
  Alignment = (LeftTop,CenterTop,RightTop,LeftCenter,Center,RightCenter,LeftBottom,CenterBottom,RightBottom);
  ///!#
  /// Базовый класс графических объектов
  ObjectWPF = class
  private
    can: Canvas;
    ob: FrameworkElement;
    gr: Grid; // Grid связан только с текстом
    t: TextBlock;
    rot: RotateTransform;

    ChildrenWPF := new List<ObjectWPF>;
    procedure InitOb(x,y,w,h: real; o: FrameworkElement; SetWH: boolean := True);
  public
    /// Направление движения по оси X. Используется методом Move
    auto property Dx: real;
    /// Направление движения по оси Y. Используется методом Move
    auto property Dy: real;
    /// Отступ графического объекта от левого края 
    property Left: real read InvokeReal(()->Canvas.GetLeft(can)) write Invoke(procedure->Canvas.SetLeft(can,value)); 
    /// Отступ графического объекта от верхнего края 
    property Top: real read InvokeReal(()->Canvas.GetTop(can)) write Invoke(procedure->Canvas.SetTop(can,value)); 
    /// Ширина графического объекта 
    property Width: real read InvokeReal(()->gr.Width) write Invoke(procedure->begin gr.Width := value; ob.Width := value end); virtual;
    /// Высота графического объекта
    property Height: real read InvokeReal(()->gr.Height) write Invoke(procedure->begin gr.Height := value; ob.Height := value end); virtual;
    /// Прямоугольник графического объекта
    property Bounds: GRect read Invoke&<GRect>(()->begin Result := new GRect(Canvas.GetLeft(can),Canvas.GetTop(can),gr.Width,gr.Height); end); 
    /// Текст внутри графического объекта
    property Text: string read InvokeString(()->t.Text) write Invoke(procedure->t.Text := value);
    /// Целое число, выводимое в центре графического объекта. Используется свойство Text
    property Number: integer read Text.ToInteger(0) write Text := Value.ToString; 
  private  
    procedure WTA(value: Alignment);
    begin
      case Value of
    Alignment.LeftTop,Alignment.CenterTop,Alignment.RightTop: t.VerticalAlignment := VerticalAlignment.Top; 
    Alignment.LeftCenter,Alignment.Center,Alignment.RightCenter: t.VerticalAlignment := VerticalAlignment.Center; 
    Alignment.LeftBottom,Alignment.CenterBottom,Alignment.RightBottom: t.VerticalAlignment := VerticalAlignment.Bottom;
      end;
      case Value of
    Alignment.LeftTop,Alignment.LeftCenter,Alignment.LeftBottom: t.HorizontalAlignment := HorizontalAlignment.Left; 
    Alignment.CenterTop,Alignment.Center,Alignment.CenterBottom: t.HorizontalAlignment := HorizontalAlignment.Center; 
    Alignment.RightTop,Alignment.RightCenter,Alignment.RightBottom: t.HorizontalAlignment := HorizontalAlignment.Right;
      end;
    end;
    procedure AddChildP(ch: ObjectWPF);
    procedure DeleteChildP(ch: ObjectWPF);
    function GetInternalGeometry: Geometry; virtual := nil;
    function GetGeometry: Geometry; virtual;
    begin
      Result := GetInternalGeometry;
      var g := new TransformGroup();
      g.Children.Add(rot);
      g.Children.Add(new TranslateTransform(Left,Top));
      Result.Transform := g; // версия
    end;
  public
    /// Видимость графического объекта
    property Visible: boolean 
      read InvokeBoolean(()->ob.Visibility = Visibility.Visible)
      write Invoke(procedure -> if value then ob.Visibility := Visibility.Visible else ob.Visibility := Visibility.Hidden);
    /// Выравнивание текста внутри графического объекта
    property TextAlignment: Alignment write Invoke(WTA,Value);
    /// Размер текста внутри графического объекта
    property FontSize: real read InvokeReal(()->t.FontSize) write Invoke(procedure->t.FontSize := value);
    /// Имя шрифта текста внутри графического объекта
    property FontName: string write Invoke(procedure->t.FontFamily := new FontFamily(value));
    /// Цвет шрифта текста внутри графического объекта
    property FontColor: Color 
      read Invoke&<GColor>(()->(t.Foreground as SolidColorBrush).Color)
      write Invoke(procedure->t.Foreground := new SolidColorBrush(value));
    /// Центр графического объекта
    property Center: Point 
      read Pnt(Left + Width/2, Top + Height/2)
      write MoveTo(Value.X - Width/2, Value.Y - Height/2);
    /// Левый верхний угол графического объекта
    property LeftTop: Point read Pnt(Left,Top);
    /// Левый нижний угол графического объекта
    property LeftBottom: Point read Pnt(Left,Top + Height);
    /// Правый верхний угол графического объекта
    property RightTop: Point read Pnt(Left + Height,Top);
    /// Правый нижний угол графического объекта
    property RightBottom: Point read Pnt(Left + Height,Top + Height);
    /// Угол поворота графического объекта (по часовой стрелке)
    property RotateAngle: real read InvokeReal(()->rot.Angle) write Invoke(procedure->rot.Angle := value);
    /// Центр поворота графического объекта
    property RotateCenter: Point 
      read Invoke&<Point>(()->new Point(rot.CenterX,rot.CenterY))
      write Invoke(procedure->begin rot.CenterX := value.X; rot.CenterY := value.Y; end);
    /// Цвет графического объекта
    property Color: GColor 
      read RGB(0,0,0) 
      write begin end; virtual;
    
    /// Перемещает левый верхний угол графического объекта к точке (x,y)
    procedure MoveTo(x,y: real) := (Self.Left,Self.Top) := (x,y);
    /// Перемещает графический объект в направлении RotateAngle (вверх при RotateAngle=0)
    procedure MoveForward(r: real);
    begin
      Left := Left + r*Cos(Pi/180*(90-RotateAngle));
      Top := Top - r*Sin(Pi/180*(90-RotateAngle));
    end;
    /// Перемещает графический объект на вектор (a,b)
    procedure MoveOn(a,b: real) := MoveTo(Left+a,Top+b);
    /// Перемещает графический объект на вектор (dx,dy)
    procedure Move; virtual := MoveOn(dx,dy);
    /// Поворачивает графический объект по часовой стрелке на угол da
    procedure Rotate(da: real) := RotateAngle += da;
    /// Добавляет к графическому объекту дочерний
    procedure AddChild(ch: ObjectWPF) := Invoke(AddChildP,ch);
    /// Удаляет из графического объекта дочерний
    procedure DeleteChild(ch: ObjectWPF) := Invoke(DeleteChildP,ch);
    /// Удаляет графический объект
    procedure Destroy;
    /// Переносит графический объект на передний план
    procedure ToFront;
    /// Переносит графический объект на задний план
    procedure ToBack;
    /// Декоратор текста объекта
    function WithText(txt: string; size: real := 16; fontname: string := 'Arial'; c: GColor := Colors.Black): ObjectWPF;
    begin
      Text := txt; 
      FontSize := size;
      Self.FontName := fontname;
      Self.FontColor := c;
      Result := Self;
    end;
    /// Декоратор поворота объекта
    function WithRotate(da: real): ObjectWPF;
    begin
      Rotate(da);
      Result := Self;
    end;
  end;
  
// -----------------------------------------------------
//>>     Класс графических объектов с границей # Class BoundedObjectWPF
// -----------------------------------------------------
  /// Класс графических объектов с границей
  BoundedObjectWPF = class(ObjectWPF)
  private
    function Element := ob as Shape;
    procedure InitOb1(x,y,w,h: real; c: GColor; o: FrameworkElement; SetWH: boolean := True);
    begin
      InitOb(x,y,w,h,o,SetWH);
      Color := c;
      //BorderColor := Colors.Black;
    end;
    procedure EF(value: GColor) := Element.Fill := new SolidColorBrush(Value);
    procedure ES(value: GColor) := Element.Stroke := new SolidColorBrush(Value);
    procedure EST(value: real);
    begin
      Element.StrokeThickness := Value;
      if Element.Stroke = nil then
        Element.Stroke := new SolidColorBrush(Colors.Black)
    end;  
    function WithNoBorderP: BoundedObjectWPF;
    begin
      Element.Stroke := nil;
      Result := Self;
    end;
  public
    /// Цвет графического объекта
    property Color: GColor 
      read Invoke&<GColor>(()->(Element.Fill as SolidColorBrush).Color) 
      write Invoke(EF,value); override;
    /// Цвет границы графического объекта
    property BorderColor: GColor 
      read Invoke&<GColor>(()->begin 
        var scb := Element.Stroke as SolidColorBrush;
        Result := scb<>nil ? scb.Color : ARGB(255,0,0,0);
      end)
      write Invoke(ES,value);
    /// Ширина границы графического объекта
    property BorderWidth: real 
      read InvokeReal(()->Element.StrokeThickness)
      write Invoke(EST,value);
    /// Декоратор включения границы объекта
    function WithBorder(w: real := -1): BoundedObjectWPF;
    begin
      BorderColor := BorderColor;
      if (w>=0) then
        BorderWidth := w;
      Result := Self;
    end;
    /// Декоратор выключения границы объекта
    function WithNoBorder: BoundedObjectWPF 
      := Invoke&<BoundedObjectWPF>(WithNoBorderP);
  end;

// -----------------------------------------------------
//>>     Класс EllipseWPF # Class EllipseWPF 
// -----------------------------------------------------
  /// Класс графических объектов "Эллипс"
  EllipseWPF = class(BoundedObjectWPF)
  private
    procedure InitOb2(x,y,w,h: real; c: GColor) := InitOb1(x,y,w,h,c,new System.Windows.Shapes.Ellipse());
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    /// Создает эллипс размера (w,h) заданного цвета с координатами левого верхнего угла (x,y)
    constructor (x,y,w,h: real; c: GColor) := Invoke(InitOb2,x,y,w,h,c);
    /// Создает эллипс размера (w,h) заданного цвета с координатами левого верхнего угла, задаваемыми точкой
    constructor (p: Point; w,h: real; c: GColor) := Invoke(InitOb2,p.x,p.y,w,h,c);
    /// Декоратор включения границы объекта
    function WithBorder(w: real := -1) := inherited WithBorder(w) as EllipseWPF;
    /// Декоратор выключения границы объекта
    function WithNoBorder := inherited WithNoBorder as EllipseWPF;
    /// Декоратор текста объекта
    function WithText(txt: string; size: real := 16; fontname: string := 'Arial'; c: GColor := Colors.Black): EllipseWPF 
      := inherited WithText(txt,size,fontname,c) as EllipseWPF;
    /// Декоратор поворота объекта
    function WithRotate(da: real): EllipseWPF 
      := inherited WithRotate(da) as EllipseWPF;
  end;

// -----------------------------------------------------
//>>     Класс CircleWPF # Class CircleWPF
// -----------------------------------------------------
  /// Класс графических объектов "Окружность"
  CircleWPF = class(BoundedObjectWPF)
  private
    procedure InitOb2(x,y,r: real; c: GColor) := InitOb1(x-r,y-r,2*r,2*r,c,new System.Windows.Shapes.Ellipse());
    procedure WT(value: real) := (ob.Width,ob.Height) := (value,value);
    procedure HT(value: real) := (ob.Width,ob.Height) := (value,value);
    procedure Rad(value: real);
    begin
      //(ob as Ellipse).RenderedGeometry
      Left -= value - ob.Width/2;
      Top -= value - ob.Width/2;
      (ob.Width,ob.Height) := (value*2,value*2);
    end;  
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    /// Создает круг радиуса r заданного цвета с координатами центра (x,y)
    constructor (x,y,r: real; c: GColor) := Invoke(InitOb2,x,y,r,c);
    /// Создает круг радиуса r заданного цвета с центром p
    constructor (p: Point; r: real; c: GColor) := Invoke(InitOb2,p.x,p.y,r,c);
    /// Ширина круга
    property Width: real 
      read InvokeReal(()->ob.Width) 
      write Invoke(WT,Value); override;
    /// Высота круга
    property Height: real 
      read InvokeReal(()->ob.Height) 
      write Invoke(HT,Value); override;
    /// Радиус круга
    property Radius: real 
      read InvokeReal(()->ob.Height/2) 
      write Invoke(Rad,Value);
    /// Декоратор включения границы объекта
    function WithBorder(w: real := -1) := inherited WithBorder(w) as CircleWPF;
    /// Декоратор выключения границы объекта
    function WithNoBorder := inherited WithNoBorder as CircleWPF;
    /// Декоратор текста объекта
    function WithText(txt: string; size: real := 16; fontname: string := 'Arial'; c: GColor := Colors.Black): CircleWPF
      := inherited WithText(txt,size,fontname,c) as CircleWPF;
    /// Декоратор поворота объекта
    function WithRotate(da: real): CircleWPF
      := inherited WithRotate(da) as CircleWPF;
  end;

// -----------------------------------------------------
//>>     Класс RectangleWPF # Class RectangleWPF
// -----------------------------------------------------
  /// Класс графических объектов "Прямоугольник"
  RectangleWPF = class(BoundedObjectWPF)
  private
    procedure InitOb2(x,y,w,h: real; c: GColor);
    begin
      var rr := new Rectangle();
      InitOb1(x,y,w,h,c,rr);
    end;
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    /// Создает прямоугольник размера (w,h) заданного цвета с координатами левого верхнего угла (x,y)
    constructor (x,y,w,h: real; c: GColor) := Invoke(InitOb2,x,y,w,h,c);
    /// Создает прямоугольник размера (w,h) заданного цвета с координатами левого верхнего угла, задаваемыми точкой
    constructor (p: Point; w,h: real; c: GColor) := Invoke(InitOb2,p.x,p.y,w,h,c);
    /// Декоратор включения границы объекта
    function WithBorder(w: real := -1) := inherited WithBorder(w) as RectangleWPF;
    /// Декоратор выключения границы объекта
    function WithNoBorder := inherited WithNoBorder as RectangleWPF;
    /// Декоратор текста объекта
    function WithText(txt: string; size: real := 16; fontname: string := 'Arial'; c: GColor := Colors.Black): RectangleWPF
      := inherited WithText(txt,size,fontname,c) as RectangleWPF;
    /// Декоратор поворота объекта
    function WithRotate(da: real): RectangleWPF 
      := inherited WithRotate(da) as RectangleWPF;
  end;
  
// -----------------------------------------------------
//>>     Класс SquareWPF # Class SquareWPF
// -----------------------------------------------------
  /// Класс графических объектов "Квадрат"
  SquareWPF = class(CircleWPF)
  private
    procedure InitOb2(x,y,w: real; c: GColor) := InitOb1(x,y,w,w,c,new Rectangle());
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    /// Создает квадрат со стороной w заданного цвета с координатами левого верхнего угла (x,y)
    constructor (x,y,w: real; c: GColor) := Invoke(InitOb2,x,y,w,c);
    /// Создает квадрат со стороной w заданного цвета с координатами левого верхнего угла, задаваемыми точкой
    constructor (p: Point; w: real; c: GColor) := Invoke(InitOb2,p.x,p.y,w,c);
    /// Декоратор включения границы объекта
    function WithBorder(w: real := -1) := inherited WithBorder(w) as SquareWPF;
    /// Декоратор выключения границы объекта
    function WithNoBorder := inherited WithNoBorder as SquareWPF;
    /// Декоратор текста объекта
    function WithText(txt: string; size: real := 16; fontname: string := 'Arial'; c: GColor := Colors.Black): SquareWPF
      := inherited WithText(txt,size,fontname,c) as SquareWPF;
    /// Декоратор поворота объекта
    function WithRotate(da: real): SquareWPF
      := inherited WithRotate(da) as SquareWPF;
  end;
  
// -----------------------------------------------------
//>>     Класс RoundRectWPF # Class RoundRectWPF
// -----------------------------------------------------
  /// Класс графических объектов "Прямоугольник со скругленными краями"
  RoundRectWPF = class(BoundedObjectWPF)
    procedure InitOb2(x,y,w,h,r: real; c: GColor);
    begin
      var rr := new Rectangle();
      rr.RadiusX := r;
      rr.RadiusY := r;
      InitOb1(x,y,w,h,c,rr);
    end;
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    /// Создает прямоугольник со скругленными краями размера (w,h) с радиусом скругления r заданного цвета с координатами левого верхнего угла (x,y)
    constructor (x,y,w,h,r: real; c: GColor) := Invoke(InitOb2,x,y,w,h,r,c);
    /// Создает прямоугольник со скругленными краями размера (w,h) с радиусом скругления r заданного цвета с координатами левого верхнего угла, задаваемыми точкой
    constructor (p: Point; w,h,r: real; c: GColor) := Invoke(InitOb2,p.x,p.y,w,h,r,c);
    /// Декоратор включения границы объекта
    function WithBorder(w: real := -1) := inherited WithBorder(w) as RoundRectWPF;
    /// Декоратор выключения границы объекта
    function WithNoBorder := inherited WithNoBorder as RoundRectWPF;
    /// Декоратор текста объекта
    function WithText(txt: string; size: real := 16; fontname: string := 'Arial'; c: GColor := Colors.Black): RoundRectWPF
      := inherited WithText(txt,size,fontname,c) as RoundRectWPF;
    /// Декоратор поворота объекта
    function WithRotate(da: real): RoundRectWPF
      := inherited WithRotate(da) as RoundRectWPF;
    /// Радиус скругления
    property RoundRadius: real 
      read InvokeReal(()->(ob as Rectangle).RadiusX)
      write begin
        var ob1 := ob;
        Invoke(procedure->begin var r := ob1 as Rectangle; r.RadiusX := value; r.Radiusy := value end);
      end;  
  end;
  
// -----------------------------------------------------
//>>     Класс графических объектов RoundSquareWPF # Class RoundSquareWPF
// -----------------------------------------------------
  /// Класс графических объектов "Квадрат со скругленными краями"
  RoundSquareWPF = class(CircleWPF)
    procedure InitOb2(x,y,w,r: real; c: GColor);
    begin
      var rr := new Rectangle();
      rr.RadiusX := r;
      rr.RadiusY := r;
      InitOb1(x,y,w,w,c,rr);
    end;
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    /// Создает квадрат со скругленными краями со стороной w с радиусом скругления r заданного цвета с координатами левого верхнего угла (x,y)
    constructor (x,y,w,r: real; c: GColor) := Invoke(InitOb2,x,y,w,r,c);
    /// Создает квадрат со скругленными краями со стороной w с радиусом скругления r заданного цвета с координатами левого верхнего угла, задаваемыми точкой
    constructor (p: Point; w,r: real; c: GColor) := Invoke(InitOb2,p.x,p.y,w,r,c);
    /// Декоратор включения границы объекта
    function WithBorder(w: real := -1): RoundSquareWPF 
      := inherited WithBorder(w) as RoundSquareWPF;
    /// Декоратор выключения границы объекта
    function WithNoBorder: RoundSquareWPF
      := inherited WithNoBorder as RoundSquareWPF;
    /// Декоратор текста объекта
    function WithText(txt: string; size: real := 16; fontname: string := 'Arial'; c: GColor := Colors.Black): RoundSquareWPF 
      := inherited WithText(txt,size,fontname,c) as RoundSquareWPF;
    /// Декоратор поворота объекта
    function WithRotate(da: real): RoundSquareWPF 
      := inherited WithRotate(da) as RoundSquareWPF;
  end;

// -----------------------------------------------------
//>>     Класс LineWPF # Class LineWPF
// -----------------------------------------------------
  /// Класс графических объектов "Отрезок"
  LineWPF = class(ObjectWPF)
  private
    function Element := ob as System.Windows.Shapes.Line;
    procedure InitOb2(x1,y1,x2,y2: real; c: GColor);
    begin
      var ll := new System.Windows.Shapes.Line();      
      InitOb(min(x1,x2),min(y1,y2),abs(x1-x2),abs(y1-y2),ll,False);
      ll.X1 := x1-Left;
      ll.Y1 := y1-Top;
      ll.X2 := x2-Left;
      ll.y2 := y2-Top;
      Color := c;
    end;
    procedure RecalcXW(x1,x2: real) := (Left,Width) := (min(x1,x2),abs(x1-x2));
    procedure RecalcYH(y1,y2: real) := (Top,Height) := (min(y1,y2),abs(y1-y2));
    procedure ES(value: GColor) := Element.Stroke := new SolidColorBrush(Value);
    procedure EST(value: real) := Element.StrokeThickness := Value;
    procedure WX1(value: real);
    begin
      var xx2 := x2;
      RecalcXW(value,xx2); 
      if value<xx2 then
        (Element.X1,Element.X2) := (0,Width)
      else (Element.X1,Element.X2) := (Width,0);
    end;
    procedure WX2(value: real);
    begin
      var xx1 := x1;
      RecalcXW(xx1,value); 
      if value>xx1 then
        (Element.X1,Element.X2) := (0,Width)
      else (Element.X1,Element.X2) := (Width,0);
    end;
    {procedure WY1(value: real) := begin Element.Y1 := value - Top;  end;
    procedure WY2(value: real) := begin Element.Y2 := value - Top; end;}
    procedure WY1(value: real);
    begin 
      var yy2 := y2;
      RecalcYH(value,yy2); 
      if value<yy2 then
        (Element.Y1,Element.Y2) := (0,Height)
      else (Element.Y1,Element.Y2) := (Height,0);
    end;
    procedure WY2(value: real); 
    begin 
      var yy1 := y1;
      RecalcYH(yy1,value); 
      if value>yy1 then
        (Element.Y1,Element.Y2) := (0,Height)
      else (Element.Y1,Element.Y2) := (Height,0);
    end;
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    /// Создает отрезок, соединяющий точки (x1,y1) и (x2,y2)
    constructor (x1,y1,x2,y2: real; c: GColor) := Invoke(InitOb2,x1,y1,x2,y2,c);
    /// Создает отрезок, соединяющий точки p1 и p2
    constructor (p1,p2: Point; c: GColor) := Invoke(InitOb2,p1.x,p1.y,p2.x,p2.y,c);
    /// Цвет отрезка
    property Color: GColor 
      read Invoke&<GColor>(()->(Element.Stroke as SolidColorBrush).Color) 
      write Invoke(ES,value); override;
    /// Ширина линии отрезка
    property LineWidth: real 
      read InvokeReal(()->Element.StrokeThickness)
      write Invoke(EST,value);
    /// Координата x начальной точки отрезка
    property X1: real read InvokeReal(()->Element.X1 + Left) write Invoke(WX1,value);
    /// Координата x конечной точки отрезка
    property X2: real read InvokeReal(()->Element.X2 + Left) write Invoke(WX2,value);
    /// Координата y начальной точки отрезка
    property Y1: real read InvokeReal(()->Element.Y1 + Top) write Invoke(WY1,value);
    /// Координата y конечной точки отрезка
    property Y2: real read InvokeReal(()->Element.Y2 + Top) write Invoke(WY2,value);
    /// Начальная точка отрезка
    property P1: Point read Pnt(X1,Y1) write begin X1 := Value.X; Y1 := Value.Y end;
    /// Конечная точка отрезка
    property P2: Point read Pnt(X2,Y2) write begin X2 := Value.X; Y2 := Value.Y end;
    /// Ширина отрезка
    property Width: real 
      read InvokeReal(()->gr.Width) 
      write 
      begin
        var gr1 := gr;
        Invoke(procedure->begin gr1.Width := value; end); 
      end; override; 
    /// Высота отрезка
    property Height: real 
      read InvokeReal(()->gr.Height) 
      write 
      begin
        var gr1 := gr;
        Invoke(procedure->begin gr1.Height := value; end); 
      end; override; 
    /// Декоратор текста объекта
    function WithText(txt: string; size: real := 16; fontname: string := 'Arial'; c: GColor := Colors.Black): LineWPF
      := inherited WithText(txt,size,fontname,c) as LineWPF;
    /// Декоратор поворота объекта
    function WithRotate(da: real): LineWPF
      := inherited WithRotate(da) as LineWPF;
    /// Декоратор ширины линии отрезка
    function WithLineWidth(lw: real): LineWPF;
    begin
      LineWidth := lw;
      Result := Self;
    end;
  end;
  
// -----------------------------------------------------
//>>     Класс RegularPolygonWPF # Class RegularPolygonWPF
// -----------------------------------------------------
  /// Класс графических объектов "Правильный многоугольник"
  RegularPolygonWPF = class(BoundedObjectWPF)
  private
    n: integer;
    function Element: System.Windows.Shapes.Polygon := ob as System.Windows.Shapes.Polygon;
    procedure InitOb2(x,y,r: real; n: integer; c: GColor);
    begin 
      InitOb1(x-r,y-r,2*r,2*r,c,CreatePolygon(r,n),false);
      (Self.Left,Self.Top,Self.n) := (x-r,y-r,n);
    end;  
    function ChangePointCollection(r: real; n: integer): PointCollection; 
    begin
      var pp := PartitionPoints(0,2*Pi,n).Select(phi->Pnt(r+r*cos(phi-Pi/2),r+r*sin(phi-Pi/2))).ToArray;      Result := new PointCollection(pp);
    end;
    function CreatePolygon(r: real; n: integer): System.Windows.Shapes.Polygon;
    begin
      var p := new System.Windows.Shapes.Polygon();
      p.Points := ChangePointCollection(r,n);
      Result := p;
    end;
    procedure Rad(value: real); 
    begin
      var delta := value - gr.Width/2;
      Left -= delta;
      Top -= delta;
      (gr.Width,gr.Height) := (value*2,value*2);
      Element.Points := ChangePointCollection(value,n);
    end;  
    procedure Cnt(value: integer);
    begin
      n := value;
      Element.Points := ChangePointCollection(Radius,value);
    end;  
    function GetInternalGeometry: Geometry; override := new EllipseGeometry(Center,Width/2,Height/2);  
  public
    /// Создает правильный многоугольник заданного цвета с координатами центра (x,y) и радиусом описанной окружности r
    constructor (x,y,r: real; n: integer; c: GColor) := Invoke(InitOb2,x,y,r,n,c);
    /// Создает правильный многоугольник заданного цвета с центром в заданной точке и радиусом описанной окружности r
    constructor (p: Point; r: real; n: integer; c: GColor) := Create(p.X,p.Y,r,n,c);
    /// Ширина объекта
    property Width: real 
      read InvokeReal(()->gr.Width) 
      write begin end; override;
    /// Высота объекта
    property Height: real 
      read InvokeReal(()->gr.Height) 
      write begin end; override;
    /// Радиус описанной окрежности
    property Radius: real 
      read InvokeReal(()->gr.Height/2) 
      write Invoke(Rad,Value); virtual;
    /// Количество вершин
    property Count: integer
      read InvokeInteger(()->n) 
      write Invoke(Cnt,Value);
    /// Декоратор включения границы объекта
    function WithBorder(w: real := -1): RegularPolygonWPF
      := inherited WithBorder(w) as RegularPolygonWPF;
    /// Декоратор выключения границы объекта
    function WithNoBorder: RegularPolygonWPF 
      := inherited WithNoBorder as RegularPolygonWPF;
    /// Декоратор текста объекта
    function WithText(txt: string; size: real := 16; fontname: string := 'Arial'; c: GColor := Colors.Black): RegularPolygonWPF  
      := inherited WithText(txt,size,fontname,c) as RegularPolygonWPF;
    /// Декоратор поворота объекта
    function WithRotate(da: real): RegularPolygonWPF  
      := inherited WithRotate(da) as RegularPolygonWPF;
  end;
  
// -----------------------------------------------------
//>>     Класс StarWPF # Class StarWPF
// -----------------------------------------------------
  /// Класс графических объектов "Звезда"
  StarWPF = class(RegularPolygonWPF)
  private
    rint: real;
    procedure InitOb2(x,y,r,rint: real; n: integer; c: GColor);
    begin 
      InitOb1(x-r,y-r,2*r,2*r,c,CreatePolygon(r,rint,n),false);
      (Self.Left,Self.Top,Self.rint,Self.n) := (x-r,y-r,rint,n);
    end;  
    function ChangePointCollection(r,rint: real; n: integer): PointCollection; 
    begin
      var pp1 := PartitionPoints(0,2*Pi,n).Select(phi->Pnt(r+r*cos(phi-Pi/2),r+r*sin(phi-Pi/2)));
      var pp2 := PartitionPoints(0+Pi/n,2*Pi+Pi/n,n).Select(phi->Pnt(r+rint*cos(phi-Pi/2),r+rint*sin(phi-Pi/2)));
      Result := new PointCollection(pp1.Interleave(pp2).ToArray);
    end;
    function CreatePolygon(r,rint: real; n: integer): System.Windows.Shapes.Polygon;
    begin
      var p := new System.Windows.Shapes.Polygon();
      p.Points := ChangePointCollection(r,rint,n);
      Result := p;
    end;
    procedure Rad(value: real);
    begin
      var delta := value - gr.Width/2;
      Left -= delta;
      Top -= delta;
      (gr.Width,gr.Height) := (value*2,value*2);
      Element.Points := ChangePointCollection(value,rint,n);
    end;  
    procedure IntRad(value: real);
    begin
      if value>Radius then
        value := Radius;
      Element.Points := ChangePointCollection(Radius,value,n);
    end;  
    procedure Cnt(value: integer);
    begin
      n := value;
      Element.Points := ChangePointCollection(Radius,rint,value);
    end;  
  public
    /// Создает звезду заданного цвета с координатами центра (x,y), радиусом описанной окружности r и внутренним радиусом rinternal
    constructor (x,y,r,rinternal: real; n: integer; c: GColor);
    begin
      if rinternal<r then
        Invoke(InitOb2,x,y,r,rinternal,n,c)
      else Invoke(InitOb2,x,y,rinternal,r,n,c)  
    end; 
    /// Создает звезду заданного цвета c центром в точке p, радиусом описанной окружности r и внутренним радиусом rinternal
    constructor (p: Point; r,rinternal: real; n: integer; c: GColor) := Create(p.X,p.Y,r,rinternal,n,c);
    /// Радиус описанной окружности
    property Radius: real 
      read InvokeReal(()->gr.Height/2) 
      write Invoke(Rad,Value); override;
    /// Внутренний радиус
    property InternalRadius: real 
      read rint 
      write Invoke(IntRad,Value);
    /// Декоратор включения границы объекта
    function WithBorder(w: real := -1): StarWPF 
      := inherited WithBorder(w) as StarWPF;
    /// Декоратор выключения границы объекта
    function WithNoBorder: StarWPF 
      := inherited WithNoBorder as StarWPF;
    /// Декоратор текста объекта
    function WithText(txt: string; size: real := 16; fontname: string := 'Arial'; c: GColor := Colors.Black): StarWPF 
      := inherited WithText(txt,size,fontname,c) as StarWPF;
    /// Декоратор поворота объекта
    function WithRotate(da: real): StarWPF 
      := inherited WithRotate(da) as StarWPF;
  end;

  PointsArray = array of Point;
  
// -----------------------------------------------------
//>>     Класс PolygonWPF # Class PolygonWPF
// -----------------------------------------------------
  /// Класс графических объектов "Многоугольник"
  PolygonWPF = class(BoundedObjectWPF)
  private
    procedure InitOb2(pp: array of Point; c: GColor);
    begin
      var x1 := pp.Min(p->p.x);
      var x2 := pp.Max(p->p.x);
      var y1 := pp.Min(p->p.y);
      var y2 := pp.Max(p->p.y);
      var a := pp.Select(p->Pnt(p.x-x1,p.y-y1)).ToArray;
      InitOb1(x1,y1,x2-x1,y2-y1,c,CreatePolygon(a),false);
    end;
    function CreatePolygon(pp: array of Point): System.Windows.Shapes.Polygon;
    begin
      var p := new System.Windows.Shapes.Polygon();
      p.Points := new PointCollection(pp);
      Result := p;
    end;
    function GetPointsArrayP: PointsArray;
    begin
      Result := (ob as System.Windows.Shapes.Polygon).Points.Select(p->p).ToArray;
    end;
  public
    /// Создает многоугольник заданного цвета с координатами вершин, заданными массивом точек pp
    constructor (pp: array of Point; c: GColor) := Invoke(InitOb2,pp,c);
    /// Массив вершин
    property Points: array of Point
      read Invoke&<PointsArray>(GetPointsArrayP)
      write begin
        var ob1 := ob as System.Windows.Shapes.Polygon;
        var pp := value;
        // ширина и высота будут некорректно. Надо переопределить на чтение
        var x1 := pp.Min(p->p.x);
        var x2 := pp.Max(p->p.x);
        var y1 := pp.Min(p->p.y);
        var y2 := pp.Max(p->p.y);
        var a := pp.Select(p->Pnt(p.x-x1,p.y-y1)).ToArray;
        MoveTo(x1,y1);
        //(gr.Width,gr.Height) := (x2-x1,y2-y1);
        Invoke(procedure -> ob1.Points := new PointCollection(a));
      end;  
    /// Декоратор включения границы объекта
    function WithBorder(w: real := -1): PolygonWPF
      := inherited WithBorder(w) as PolygonWPF;
    /// Декоратор выключения границы объекта
    function WithNoBorder: PolygonWPF 
      := inherited WithNoBorder as PolygonWPF;
    /// Декоратор текста объекта
    function WithText(txt: string; size: real := 16; fontname: string := 'Arial'; c: GColor := Colors.Black): PolygonWPF 
      := inherited WithText(txt,size,fontname,c) as PolygonWPF;
    /// Декоратор поворота объекта
    function WithRotate(da: real): PolygonWPF
      := inherited WithRotate(da) as PolygonWPF;
  end;

// -----------------------------------------------------
//>>     Класс PictureWPF # Class PictureWPF
// -----------------------------------------------------
  /// Класс графических объектов "Рисунок"
  PictureWPF = class(ObjectWPF)
  private
    function CreateBitmapImage(fname: string) := new BitmapImage(new System.Uri(fname,System.UriKind.Relative)); 
    procedure Rest(x,y,w,h: real; b: BitmapImage);
    begin
      var im := new System.Windows.Controls.Image();
      im.Source := b;
      im.Width := w;
      im.Height := h;

      InitOb(x,y,w,h,im);
    end;
    procedure InitOb3(x,y,w,h: real; fname: string);
    begin
      var b := CreateBitmapImage(fname);
      Rest(x,y,w,h,b);
    end;

    procedure InitOb2(x,y: real; fname: string);
    begin
      var b := CreateBitmapImage(fname);
      Rest(x,y,b.Width,b.Height,b);
    end;
    function GetInternalGeometry: Geometry; override := new RectangleGeometry(Rect(Left,Top,Width,Height));  
  public
    /// Создает рисунок из файла fname с координатами левого верхнего угла (x,y)
    constructor (x,y: real; fname: string) := Invoke(InitOb2,x,y,fname);
    /// Создает рисунок из файла fname с координатами левого верхнего угла (x,y) и размерами (w,h)
    constructor (x,y,w,h: real; fname: string) := Invoke(InitOb3,x,y,w,h,fname);
    /// Создает рисунок из файла fname с координатой левого верхнего угла, заданной точкой p
    constructor (p: Point; fname: string) := Invoke(InitOb2,p.x,p.y,fname);
    /// Создает рисунок из файла fname  с координатой левого верхнего угла, заданной точкой p, и размерами (w,h)
    constructor (p: Point; w,h: real; fname: string) := Invoke(InitOb3,p.x,p.y,w,h,fname);
    /// Декоратор текста объекта
    function WithText(txt: string; size: real := 16; fontname: string := 'Arial'; c: GColor := Colors.Black): PictureWPF  
      := inherited WithText(txt,size,fontname,c) as PictureWPF;
    /// Декоратор поворота объекта
    function WithRotate(da: real): PictureWPF 
      := inherited WithRotate(da) as PictureWPF;
  end;

// -----------------------------------------------------
//>>     Переменные модуля WPFObjects# WPFObjects Variables
// -----------------------------------------------------
/// Главное окно
var Window: WindowType;
/// Графическое окно
var GraphWindow: GraphWindowType;
/// Список графических объектов
var Objects: ObjectsType;

var
// -----------------------------------------------------
//>>     События модуля WPFObjects# WPFObjects events
// -----------------------------------------------------
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

// -----------------------------------------------------
//>>     Функции пересечения# Intersection functions
// -----------------------------------------------------
/// Возвращает графический объект под точкой с координатами (x,y) или nil
function ObjectUnderPoint(x,y: real): ObjectWPF;
/// Возвращает True если графические объекты пересекаются
function ObjectsIntersect(o1,o2: ObjectWPF): boolean;

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;
//{{{--doc: Конец секции 2 }}} 

implementation

function RGB(r,g,b: byte) := Color.Fromrgb(r, g, b);
function ARGB(a,r,g,b: byte) := Color.FromArgb(a, r, g, b);
function RandomColor := RGB(PABCSystem.Random(256), PABCSystem.Random(256), PABCSystem.Random(256));
function GrayColor(b: byte): Color := RGB(b,b,b);
function EmptyColor := ARGB(0,0,0,0);
function clRandom := RandomColor();
function Pnt(x,y: real) := new Point(x,y);
function Rect(x,y,w,h: real) := new System.Windows.Rect(x,y,w,h);
function ColorBrush(c: Color) := new SolidColorBrush(c);

procedure Invoke(p: ()->()) := GraphWPFBase.Invoke(p);

procedure SetLeft(Self: UIElement; l: integer) := Self.SetLeft(l);
procedure SetTop(Self: UIElement; t: integer) := Self.SetTop(t);


function MoveOn(Self: Point; vx,vy: real): Point; extensionmethod;
begin
  Result.X := Self.X + vx;
  Result.Y := Self.Y + vy;
end;

{procedure MoveTo(Self: UIElement; l,t: integer); extensionmethod;
begin
  Canvas.SetLeft(Self,l);
  Canvas.SetTop(Self,t);
end;}

procedure ObjectsType.AddP(ob: ObjectWPF);
begin
  l.Add(ob);
  host.Children.Add(ob.can);
  d.Add(ob.ob,ob);
end;

procedure ObjectsType.DeleteP(ob: ObjectWPF);
begin
  l.Remove(ob);
  host.Children.Remove(ob.can);
  d.Remove(ob.ob);
end;

procedure ObjectsType.ToBackP(ob: ObjectWPF);
begin
  l.Remove(ob);
  l.Insert(0,ob);
  host.Children.Remove(ob.can);
  host.Children.Insert(0,ob.can)
end;

procedure ObjectsType.ToFrontP(ob: ObjectWPF);
begin
  l.Remove(ob);
  l.Add(ob);
  host.Children.Remove(ob.can);
  host.Children.Add(ob.can)
end;

procedure ObjectWPF.InitOb(x,y,w,h: real; o: FrameworkElement; SetWH: boolean);
begin
  can := new Canvas;
  gr := new Grid;
  rot := new RotateTransform(0);
  rot.CenterX := w / 2;
  rot.CenterY := h / 2;
  can.RenderTransform := rot;
  ob := o;
  if SetWH then 
    (ob.Width,ob.Height) := (w,h);
  MoveTo(x,y);
  //gr.Children.Add(ob);
  can.Children.Add(ob);

  (gr.Width,gr.Height) := (w,h);
  t := new TextBlock();
  t.VerticalAlignment := VerticalAlignment.Center;
  t.HorizontalAlignment := HorizontalAlignment.Center;

  gr.Children.Add(t);
  can.Children.Add(gr);

  Objects.Add(Self);  
  //host.Children.Add(can);
  
  FontSize := 16;      
end;

procedure ObjectWPF.AddChildP(ch: ObjectWPF);
begin
  ChildrenWPF.Add(ch);
  //host.Children.Remove(ch.can);
  Objects.Destroy(ch);
  can.Children.Add(ch.can);
end;

procedure ObjectWPF.DeleteChildP(ch: ObjectWPF);
begin
  ChildrenWPF.Remove(ch);
  //host.Children.Remove(ch.gr);
end;

procedure ObjectWPF.Destroy;
begin
  Objects.Destroy(Self);  
end;

procedure ObjectWPF.ToFront;
begin
  Objects.ToFront(Self);  
end;

procedure ObjectWPF.ToBack;
begin
  Objects.ToBack(Self);  
end;

var hitResultsList := new List<DependencyObject>;

function MyHitTestResult(res: HitTestResult): HitTestResultBehavior;
begin
  hitResultsList.Add(res.VisualHit);
  Result := HitTestResultBehavior.Continue;
end;

function ObjectUnderPointP(x,y: real): ObjectWPF;
begin
  hitResultsList.Clear();

  VisualTreeHelper.HitTest(host, nil,
        MyHitTestResult,
        new PointHitTestParameters(Pnt(x,y)));
  
  //hitResultsList.Print;
  foreach var a in hitResultsList do
    foreach var b in Objects.l do
      if b.ob=a then
      begin
        Result := b;
        exit;
      end;
    
  Result := nil;  
end;

type XYHelper = auto class
  x,y: real;
  function f: ObjectWPF := ObjectUnderPointP(x,y);
end;

function ObjectUnderPoint(x,y: real): ObjectWPF
  := Invoke&<ObjectWPF>(XYHelper.Create(x,y).f);


function MyHitTestResult2(res: HitTestResult): HitTestResultBehavior;
begin
  var id := (res as GeometryHitTestResult).IntersectionDetail;

  Result :=  HitTestResultBehavior.Stop;
  case id of
    IntersectionDetail.FullyContains,
    IntersectionDetail.Intersects,
    IntersectionDetail.FullyInside:
    begin
      hitResultsList.Add(res.VisualHit);
      Result := HitTestResultBehavior.Continue;
    end; 
  end;
end;


function ObjectsIntersectP(o1,o2: ObjectWPF): boolean;
begin
  hitResultsList.Clear();
  
  VisualTreeHelper.HitTest(host, nil,
        MyHitTestResult2,
        new GeometryHitTestParameters(o2.GetGeometry));
        
  Result := False;      
  foreach var a in hitResultsList do
      if a=o1.ob then
      begin
        Result := True;
        exit;
      end;
end;

function ObjectsIntersectPL(o: ObjectWPF): List<ObjectWPF>;
begin
  hitResultsList.Clear();
  
  VisualTreeHelper.HitTest(host, nil,
        MyHitTestResult2,
        new GeometryHitTestParameters(o.GetGeometry));
        
  Result := new List<ObjectWPF>;      
  foreach var a in hitResultsList do
  begin
    var aa := a as FrameworkElement;
    if (aa<>o.ob) and Objects.d.ContainsKey(aa) then
      Result.Add(Objects.d[aa])
  end;
end;

type ObHelper = auto class
  o1,o2: ObjectWPF;
  function f: boolean := ObjectsIntersectP(o1,o2);
end;

type OLHelper = auto class
  o: ObjectWPF;
  function f: List<ObjectWPF> := ObjectsIntersectPL(o);
end;


function ObjectsIntersect(o1,o2: ObjectWPF) 
  := Invoke&<boolean>(ObHelper.Create(o1,o2).f);
  
function IntersectionList(Self: ObjectWPF): List<ObjectWPF>; extensionmethod
  := Invoke&<List<ObjectWPF>>(OLHelper.Create(Self).f);

var
  ///--
  __initialized := false;

var
  ///--
  __finalized := false;

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

procedure __InitModule;
begin
  AdditionalInit := procedure ->
  begin
    MainWindow.Title := 'Графика WPF';
    // Свои события. Без этого не работают
    MainWindow.MouseDown += SystemOnMouseDown;
    MainWindow.MouseUp += SystemOnMouseUp;
    MainWindow.MouseMove += SystemOnMouseMove;
    MainWindow.KeyDown += SystemOnKeyDown;
    MainWindow.KeyUp += SystemOnKeyUp;
    MainWindow.TextInput += SystemOnKeyPress;
    MainWindow.SizeChanged += SystemOnResize;
    
    Objects := new ObjectsType;
    Window := GraphWPF.Window;
    GraphWindow := GraphWPF.GraphWindow;

    host := new Canvas();
    {host.SizeChanged += (s,e) ->
    begin
      var sz := e.NewSize;
      host.DataContext := sz;
    end;}
    var g := MainWindow.Content as DockPanel;
    g.children.Add(host); // Слой графики WPF - последний
  end;
  app.Dispatcher.Invoke(AdditionalInit);
end;

///--
procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    GraphWPF.__InitModule__;
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