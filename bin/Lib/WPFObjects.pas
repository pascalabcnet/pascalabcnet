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
  
var host: Canvas;

/// Возвращает цвет по красной, зеленой и синей составляющей (в диапазоне 0..255)
function RGB(r,g,b: byte): Color;
/// Возвращает цвет по красной, зеленой и синей составляющей и параметру прозрачности (в диапазоне 0..255)
function ARGB(a,r,g,b: byte): Color;
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
  
  Alignment = (LeftTop,CenterTop,RightTop,LeftCenter,Center,RightCenter,LeftBottom,CenterBottom,RightBottom);
  ObjectWPF = class;
  ///!#
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
  public
    procedure Add(ob: ObjectWPF) := Invoke(AddP,ob);
    procedure Destroy(ob: ObjectWPF) := Invoke(DeleteP,ob);
    procedure ToBack(ob: ObjectWPF) := Invoke(ToBackP,ob);
    procedure ToFront(ob: ObjectWPF) := Invoke(ToFrontP,ob);
    property Count: integer read l.Count;
    property Items[i: integer]: ObjectWPF read GetItem write SetItem; default;
  end;

  ///!#
  ObjectWPF = class
  private
    can: Canvas;
    ob: FrameworkElement;
    gr: Grid; // Grid связан только с текстом
    t: TextBlock;
    r: RotateTransform;
    ChildrenWPF := new List<ObjectWPF>;
    procedure InitOb(x,y,w,h: real; o: FrameworkElement; SetWH: boolean := True);
  public
    dx,dy: real;
    property Left: real read InvokeReal(()->Canvas.GetLeft(can)) write Invoke(procedure->Canvas.SetLeft(can,value)); 
    property Top: real read InvokeReal(()->Canvas.GetTop(can)) write Invoke(procedure->Canvas.SetTop(can,value)); 
    property Width: real read InvokeReal(()->gr.Width) write Invoke(procedure->begin gr.Width := value; ob.Width := value end); virtual;
    property Height: real read InvokeReal(()->gr.Height) write Invoke(procedure->begin gr.Height := value; ob.Height := value end); virtual;
    property Text: string read InvokeString(()->t.Text) write Invoke(procedure->t.Text := value);
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
      g.Children.Add(r);
      g.Children.Add(new TranslateTransform(Left,Top));
      Result.Transform := g; // версия
    end;
  public
    property TextAlignment: Alignment write Invoke(WTA,Value);
    property FontSize: real read InvokeReal(()->t.FontSize) write Invoke(procedure->t.FontSize := value);
    property FontName: string write Invoke(procedure->t.FontFamily := new FontFamily(value));
    property FontColor: Color 
      read Invoke&<GColor>(()->(t.Foreground as SolidColorBrush).Color)
      write Invoke(procedure->t.Foreground := new SolidColorBrush(value));
    property Center: Point 
      read Pnt(Left + Width/2, Top + Height/2)
      write MoveTo(Value.X - Width/2, Value.Y - Height/2);
    property LeftTop: Point read Pnt(Left,Top);
    property LeftBottom: Point read Pnt(Left,Top + Height);
    property RightTop: Point read Pnt(Left + Height,Top);
    property RightBottom: Point read Pnt(Left + Height,Top + Height);

    property RotateAngle: real read InvokeReal(()->r.Angle) write Invoke(procedure->r.Angle := value);
    property RotateCenter: Point 
      read Invoke&<Point>(()->new Point(r.CenterX,r.CenterY))
      write Invoke(procedure->begin r.CenterX := value.X; r.CenterY := value.Y; end);
    property Color: GColor 
      read RGB(0,0,0) 
      write begin end; virtual;
    
    procedure MoveTo(x,y: real) := (Self.Left,Self.Top) := (x,y);
    procedure MoveForward(r: real);
    begin
      Left := Left + r*Cos(Pi/180*(90-RotateAngle));
      Top := Top - r*Sin(Pi/180*(90-RotateAngle));
    end;

    procedure MoveOn(dx,dy: real) := MoveTo(Left+dx,Top+dy);
    procedure Move := MoveOn(dx,dy);
    procedure Rotate(da: real) := RotateAngle += da;
    procedure AddChild(ch: ObjectWPF) := Invoke(AddChildP,ch);
    procedure DeleteChild(ch: ObjectWPF) := Invoke(DeleteChildP,ch);
    procedure Destroy;
    procedure ToFront;
    procedure ToBack;
  end;
  
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
    procedure EST(value: real) := Element.StrokeThickness := Value;
  public
    property Color: GColor 
      read Invoke&<GColor>(()->(Element.Fill as SolidColorBrush).Color) 
      write Invoke(EF,value); override;
    property BorderColor: GColor 
      read Invoke&<GColor>(()->begin 
        var scb := Element.Stroke as SolidColorBrush;
        Result := scb<>nil ? scb.Color : ARGB(0,255,255,255);
      end)
      write Invoke(ES,value);
    property BorderWidth: real 
      read InvokeReal(()->Element.StrokeThickness)
      write Invoke(EST,value);
  end;
  
  EllipseWPF = class(BoundedObjectWPF)
  private
    procedure InitOb2(x,y,w,h: real; c: GColor) := InitOb1(x,y,w,h,c,new Ellipse());
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    constructor (x,y,w,h: real; c: GColor) := Invoke(InitOb2,x,y,w,h,c);
  end;

  CircleWPF = class(BoundedObjectWPF)
  private
    procedure InitOb2(x,y,r: real; c: GColor) := InitOb1(x-r,y-r,2*r,2*r,c,new Ellipse());
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
    constructor (x,y,r: real; c: GColor) := Invoke(InitOb2,x,y,r,c);
    constructor (p: Point; r: real; c: GColor) := Invoke(InitOb2,p.x,p.y,r,c);
    property Width: real 
      read InvokeReal(()->ob.Width) 
      write Invoke(WT,Value); override;
    property Height: real 
      read InvokeReal(()->ob.Height) 
      write Invoke(HT,Value); override;
    property Radius: real 
      read InvokeReal(()->ob.Height/2) 
      write Invoke(Rad,Value);
  end;

  RectangleWPF = class(BoundedObjectWPF)
  private
    procedure InitOb2(x,y,w,h: real; c: GColor);
    begin
      var rr := new Rectangle();
      InitOb1(x,y,w,h,c,rr);
    end;
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    constructor (x,y,w,h: real; c: GColor) := Invoke(InitOb2,x,y,w,h,c);
  end;
  
  SquareWPF = class(CircleWPF)
  private
    procedure InitOb2(x,y,w: real; c: GColor) := InitOb1(x,y,w,w,c,new Rectangle());
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    constructor (x,y,w: real; c: GColor) := Invoke(InitOb2,x,y,w,c);
  end;
  
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
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    constructor (x,y,w,r: real; c: GColor) := Invoke(InitOb2,x,y,w,r,c);
  end;

  LineWPF = class(ObjectWPF)
  private
    function Element := ob as Line;
    procedure InitOb2(x1,y1,x2,y2: real; c: GColor);
    begin
      var ll := new Line();      
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
    procedure WX1(value: real) := begin 
      var xx2 := x2;
      RecalcXW(value,xx2); 
      if value<xx2 then
        (Element.X1,Element.X2) := (0,Width)
      else (Element.X1,Element.X2) := (Width,0);
    end;
    procedure WX2(value: real) := begin 
      var xx1 := x1;
      RecalcXW(xx1,value); 
      if value>xx1 then
        (Element.X1,Element.X2) := (0,Width)
      else (Element.X1,Element.X2) := (Width,0);
    end;
    procedure WY1(value: real) := begin Element.Y1 := value - Top; {RecalcWHXY;} end;
    procedure WY2(value: real) := begin Element.Y2 := value - Top; {RecalcWHXY;} end;
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    constructor (x1,y1,x2,y2: real; c: GColor) := Invoke(InitOb2,x1,y1,x2,y2,c);
    property Color: GColor 
      read Invoke&<GColor>(()->(Element.Stroke as SolidColorBrush).Color) 
      write Invoke(ES,value); override;
    property LineWidth: real 
      read InvokeReal(()->Element.StrokeThickness)
      write Invoke(EST,value);
    property X1: real read InvokeReal(()->Element.X1 + Left) write Invoke(WX1,value);
    property X2: real read InvokeReal(()->Element.X2 + Left) write Invoke(WX2,value);
    property Y1: real read InvokeReal(()->Element.Y1 + Top) write Invoke(WY1,value);
    property Y2: real read InvokeReal(()->Element.Y2 + Top) write Invoke(WY2,value);
    property P1: Point read Pnt(X1,Y1) write begin X1 := Value.X; Y1 := Value.Y end;
  end;
  
  RegularPolygonWPF = class(BoundedObjectWPF)
  private
    n: integer;
    function Element: Polygon := ob as Polygon;
    procedure InitOb2(x,y,r: real; n: integer; c: GColor);
    begin 
      InitOb1(x-r,y-r,2*r,2*r,c,CreatePolygon(r,n),false);
      (Self.Left,Self.Top,Self.n) := (x-r,y-r,n);
    end;  
    function ChangePointCollection(r: real; n: integer): PointCollection; 
    begin
      var pp := Partition(0,2*Pi,n).Select(phi->Pnt(r+r*cos(phi-Pi/2),r+r*sin(phi-Pi/2))).ToArray;
      Result := new PointCollection(pp);
    end;
    function CreatePolygon(r: real; n: integer): Polygon;
    begin
      var p := new Polygon();
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
    constructor (x,y,r: real; n: integer; c: GColor) := Invoke(InitOb2,x,y,r,n,c);
    constructor (p: Point; r: real; n: integer; c: GColor) := Create(p.X,p.Y,r,n,c);
    property Width: real 
      read InvokeReal(()->gr.Width) 
      write begin end; override;
    property Height: real 
      read InvokeReal(()->gr.Height) 
      write begin end; override;
    property Radius: real 
      read InvokeReal(()->gr.Height/2) 
      write Invoke(Rad,Value); virtual;
    property Count: integer
      read InvokeInteger(()->n) 
      write Invoke(Cnt,Value);
  end;
  
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
      var pp1 := Partition(0,2*Pi,n).Select(phi->Pnt(r+r*cos(phi-Pi/2),r+r*sin(phi-Pi/2)));
      var pp2 := Partition(0+Pi/n,2*Pi+Pi/n,n).Select(phi->Pnt(r+rint*cos(phi-Pi/2),r+rint*sin(phi-Pi/2)));
      Result := new PointCollection(pp1.Interleave(pp2).ToArray);
    end;
    function CreatePolygon(r,rint: real; n: integer): Polygon;
    begin
      var p := new Polygon();
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
    constructor (x,y,r,rinternal: real; n: integer; c: GColor);
    begin
      if rinternal<r then
        Invoke(InitOb2,x,y,r,rinternal,n,c)
      else Invoke(InitOb2,x,y,rinternal,r,n,c)  
    end; 
    constructor (p: Point; r,rinternal: real; n: integer; c: GColor) := Create(p.X,p.Y,r,rinternal,n,c);
    property Radius: real 
      read InvokeReal(()->gr.Height/2) 
      write Invoke(Rad,Value); override;
    property InternalRadius: real 
      read rint 
      write Invoke(IntRad,Value);
  end;

  PolygonWPF = class(BoundedObjectWPF)
  private
    procedure InitOb2(pp: array of Point; c: GColor);
    begin
      var x1 := pp.Min(p->p.x);
      var x2 := pp.Max(p->p.x);
      var y1 := pp.Min(p->p.y);
      var y2 := pp.Max(p->p.y);
      var a := pp.Select(p->Pnt(p.x-x1,p.y-y1)).ToArray;
      InitOb1(x1,y1,x2-x1,y2-y1,c,CreatePolygon(a));
    end;
    function CreatePolygon(pp: array of Point): Polygon;
    begin
      var p := new Polygon();
      p.Points := new PointCollection(pp);
      Result := p;
    end;
  public
    constructor (pp: array of Point; c: GColor) := Invoke(InitOb2,pp,c);
  end;

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
    constructor (x,y: real; fname: string) := Invoke(InitOb2,x,y,fname);
    constructor (x,y,w,h: real; fname: string) := Invoke(InitOb3,x,y,w,h,fname);
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

  Objects := new ObjectsType;

function ObjectUnderPoint(x,y: real): ObjectWPF;
function ObjectsIntersect(o1,o2: ObjectWPF): boolean;

implementation

function RGB(r,g,b: byte) := Color.Fromrgb(r, g, b);
function ARGB(a,r,g,b: byte) := Color.FromArgb(a, r, g, b);
function RandomColor := RGB(PABCSystem.Random(256), PABCSystem.Random(256), PABCSystem.Random(256));
function EmptyColor := ARGB(0,0,0,0);
function clRandom := RandomColor();
function Pnt(x,y: real) := new Point(x,y);
function Rect(x,y,w,h: real) := new System.Windows.Rect(x,y,w,h);
function ColorBrush(c: Color) := new SolidColorBrush(c);

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
  r := new RotateTransform(0);
  r.CenterX := w / 2;
  r.CenterY := h / 2;
  can.RenderTransform := r;
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

//---------------------------------------------------------------------------  
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