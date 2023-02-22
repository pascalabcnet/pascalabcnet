// Copyright (©) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
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
  /// Тип размера
  GSize = System.Windows.Size;
  /// Тип точки
  Point = System.Windows.Point;
  /// Тип точки
  GPoint = System.Windows.Point;
  /// Тип окна
  GWindow = System.Windows.Window;
  /// Тип пера
  GPen = System.Windows.Media.Pen;
  /// Тип кисти
  GBrush = System.Windows.Media.Brush;
  /// Тип стиля шрифта
  FontStyle = (Normal,Bold,Italic,BoldItalic);
  Alignment = GraphWPF.Alignment;
  
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
function ColorBrush(c: Color): SolidColorBrush;
/// Возвращает случайную точку графического окна. Необязательный параметр z задаёт отступ от края  
function RandomPoint(z: real := 0): GPoint;
/// Процедура ускорения вывода. Обновляет экран после всех изменений
procedure Redraw(p: ()->());
//{{{--doc: Конец секции 1 }}} 

//{{{doc: Начало секции 2 }}} 
type
  ObjectWPF = class;
// -----------------------------------------------------
//>>     Класс списка графических объектов # Class List of objects
// -----------------------------------------------------
  ///!#
  /// Класс списка графических объектов
  ObjectsType = class(IEnumerable<ObjectWPF>)
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
    procedure Destroy(ob: ObjectWPF);
    begin
      if not l.Contains(ob) then
        raise new Exception('Объект отсутствует в списке Objects');
      Invoke(DeleteP,ob);
    end;  
  public
    /// Перемещает графический объект на задний план
    procedure ToBack(ob: ObjectWPF);
    begin
      if not l.Contains(ob) then
        raise new Exception('На задний план нельзя переносить дочерние объекты');
      Invoke(ToBackP,ob);
    end;
    /// Перемещает графический объект на передний план
    procedure ToFront(ob: ObjectWPF);
    begin
      if not l.Contains(ob) then
        raise new Exception('На передний план нельзя переносить дочерние объекты');
      Invoke(ToFrontP,ob);
    end;
    /// Возвращает количество графических объектов 
    property Count: integer read l.Count;
    /// Возвращает или устанавливает i-тый графический объект 
    property Items[i: integer]: ObjectWPF read GetItem write SetItem; default;
  
    function Seq: sequence of ObjectWPF := l;
    function GetEnumerator: IEnumerator<ObjectWPF>;
    begin
      Result := l.GetEnumerator;
    end;
    function System.Collections.IEnumerable.GetEnumerator: System.Collections.IEnumerator;
    begin
      Result := l.GetEnumerator;
    end;
    /// Очистить список графических объектов 
    procedure Clear;
    begin
      for var i:=Count-1 downto 0 do
        Destroy(Items[i]);
    end;
    /// Удалить все графические объекты, удовлетворяющие условию
    procedure DestroyAll(condition: ObjectWPF -> boolean);
    begin
      for var i := Count - 1 downto 0 do
        if condition(Items[i]) then
          Destroy(Items[i]);
    end;
    /// Возвращает инвертированный список графических объектов
    function Reverse: IEnumerable<ObjectWPF> := (Self as IEnumerable<ObjectWPF>).Reverse;
  end;


// -----------------------------------------------------
//>>     Класс ObjectWPF # Class ObjectWPF 
// -----------------------------------------------------
  ///!#
  /// Базовый класс графических объектов
  ObjectWPF = class
  private
    can: Canvas;
    ob: FrameworkElement;
    gr: Grid; // Grid связан только с текстом
    t: TextBlock;
    ChildrenWPF := new List<ObjectWPF>;
    procedure InitOb(x,y,w,h: real; o: FrameworkElement; SetWH: boolean := True; Hidden: boolean := False);
  protected  
    transfgroup: TransformGroup;
    rot: RotateTransform;
    sca: ScaleTransform;
    transl: TranslateTransform;
  public
    /// Направление движения по оси X. Используется методом Move
    auto property Dx: real;
    /// Направление движения по оси Y. Используется методом Move
    auto property Dy: real;
    /// Направление движения. Используется методом Move
    property Direction: (real,real) read (Dx,Dy) write (Dx,Dy) := (value[0],value[1]);
    /// Скорость движения в направлении Direction. Используется методом Move
    auto property Velocity: real := 300;
    /// Отступ графического объекта от левого края окна
    property Left: real read InvokeReal(()->transl.X{Canvas.GetLeft(can)}) write Invoke(procedure->transl.X := value{Canvas.SetLeft(can,value)}); 
    /// Отступ графического объекта от верхнего края окна
    property Top: real read InvokeReal(()->transl.Y{Canvas.GetTop(can)}) write Invoke(procedure->transl.Y := value{Canvas.SetTop(can,value)}); 
    /// Отступ правтого края графического объекта от левого края окна
    property Right: real read InvokeReal(()->transl.X + gr.Width{Canvas.GetLeft(can)}) write Invoke(procedure->transl.X := value - gr.Width{Canvas.SetLeft(can,value)}); 
    /// Отступ низа графического объекта от верхнего края окна
    property Bottom: real read InvokeReal(()->transl.Y + gr.Height{Canvas.GetTop(can)}) write Invoke(procedure->transl.Y := value - gr.Height{Canvas.SetTop(can,value)}); 
    /// Ширина графического объекта 
    property Width: real read InvokeReal(()->gr.Width) write Invoke(procedure->begin gr.Width := value; ob.Width := value end); virtual;
    /// Высота графического объекта
    property Height: real read InvokeReal(()->gr.Height) write Invoke(procedure->begin gr.Height := value; ob.Height := value end); virtual;
    /// Отмасштабированная ширина графического объекта 
    property ScaledWidth: real read Width*ScaleFactor;
    /// Отмасштабированная высота графического объекта
    property ScaledHeight: real read Height*ScaleFactor;
    /// Размер графического объекта
    property Size: GSize read Invoke&<GSize>(()->new GSize(gr.Width,gr.Height)) 
      write Invoke(procedure->begin gr.Width := value.Width; ob.Width := value.Width; gr.Height := value.Height; ob.Height := value.Height end); virtual;
    /// Отмасштабированный размер графического объекта
    property ScaledSize: GSize read new GSize(ScaledWidth,ScaledHeight);
    /// Прямоугольник графического объекта
    property Bounds: GRect read Invoke&<GRect>(()->begin Result := new GRect(transl.X,transl.Y,gr.Width,gr.Height); end); 
    /// Текст внутри графического объекта
    property Text: string read InvokeString(()->t.Text) write Invoke(procedure->t.Text := value); virtual;
    /// Целое число, выводимое в центре графического объекта. Используется свойство Text
    property Number: integer read Text.ToInteger(0) write Text := Value.ToString; 
    /// Вещественное число, выводимое в центре графического объекта. Используется свойство Text
    property RealNumber: real read Text.ToReal(0.0) write Text := string.Format('{0:f1}',Value).Replace(',','.');
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
    procedure AddChildP(ch: ObjectWPF; al: Alignment);
    procedure DeleteChildP(ch: ObjectWPF);
    function GetInternalGeometry: Geometry; virtual := nil;
  public
    function GetGeometry: Geometry; virtual;
    begin
      Result := GetInternalGeometry;
      var g := new TransformGroup();
      g.Children.Add(rot);
      g.Children.Add(sca);
      g.Children.Add(transl);
      //g.Children.Add(new TranslateTransform(Left,Top));
      Result.Transform := g; 
    end;
    /// Видимость графического объекта
    property Visible: boolean 
      read InvokeBoolean(()->ob.Visibility = Visibility.Visible)
      write Invoke(procedure -> if value then begin gr.Visibility := Visibility.Visible; ob.Visibility := Visibility.Visible end else begin gr.Visibility := Visibility.Hidden; ob.Visibility := Visibility.Hidden end);
    /// Выравнивание текста внутри графического объекта
    property TextAlignment: Alignment write Invoke(WTA,Value);
    /// Размер шрифта текста внутри графического объекта
    property FontSize: real read InvokeReal(()->t.FontSize) write Invoke(procedure->t.FontSize := value); virtual;
    /// Имя шрифта текста внутри графического объекта
    property FontName: string write Invoke(procedure->t.FontFamily := new FontFamily(value)); virtual;
    /// Цвет шрифта текста внутри графического объекта
    property FontColor: Color 
      read Invoke&<GColor>(()->(t.Foreground as SolidColorBrush).Color)
      write Invoke(procedure->t.Foreground := ColorBrush(value));
    /// Центр графического объекта
    property Center: Point 
      read Pnt(Left + Width/2, Top + Height/2)
      write MoveTo(Value.X - Width/2, Value.Y - Height/2);
    /// Левый верхний угол графического объекта
    property LeftTop: Point read Pnt(Left,Top);
    /// Левый нижний угол графического объекта
    property LeftBottom: Point read Pnt(Left,Top + Height);
    /// Правый верхний угол графического объекта
    property RightTop: Point read Pnt(Left + Width,Top);
    /// Правый нижний угол графического объекта
    property RightBottom: Point read Pnt(Left + Width,Top + Height);
    /// Центральная верхняя точка графического объекта
    property CenterTop: Point read transfgroup.Transform(Pnt(Width/2,0));
    /// Центральная нижняя точка графического объекта
    property CenterBottom: Point read Pnt(Left + Width/2,Top + Height);
    /// Угол поворота графического объекта (по часовой стрелке)
    property RotateAngle: real read InvokeReal(()->rot.Angle) write Invoke(procedure->begin rot.CenterX := Width/2; rot.CenterY := Height/2; rot.Angle := value end);
    /// Множитель масштабирования объекта  
    property ScaleFactor: real read InvokeReal(()->sca.ScaleX) write Invoke(()->begin sca.CenterX := Width/2; sca.CenterY := Height/2; (sca.ScaleX, sca.ScaleY) := (value,value); end);
    // Центр поворота графического объекта - запретил, т.к. это будет сбивать координаты объекта
    {property RotateCenter: Point 
      read Invoke&<Point>(()->new Point(rot.CenterX,rot.CenterY))
      write Invoke(procedure->begin rot.CenterX := value.X; rot.CenterY := value.Y; end);}
    /// Цвет графического объекта
    property Color: GColor 
      read RGB(0,0,0) 
      write begin end; virtual;
    
    /// Перемещает левый верхний угол графического объекта к точке (x,y)
    procedure MoveTo(x,y: real) := (Self.Left,Self.Top) := (x,y);
    /// Перемещает графический объект в направлении RotateAngle (вверх при RotateAngle=0)
    procedure MoveForward(r: real);
    begin
      var a := Pi/180*(90-RotateAngle);
      MoveBy(r*Cos(a),-r*Sin(a));
    end;
    /// Перемещает графический объект на вектор (a,b)
    procedure MoveBy(a,b: real) := MoveTo(Left+a,Top+b);
    /// Перемещает графический объект на вектор (a,b)
    procedure MoveBy(v: (real,real)) := MoveTo(Left+v[0],Top+v[1]);
    ///--
    procedure MoveOn(a,b: real) := MoveTo(Left+a,Top+b);
    ///--
    procedure MoveOn(v: (real,real)) := MoveTo(Left+v[0],Top+v[1]);
    /// Перемещает графический объект на вектор (dx,dy)
    procedure Move; virtual := MoveBy(dx,dy);
    /// Перемещает графический объект вдоль вектора Direction со скоростью Velocity за время dt
    procedure MoveTime(dt: real); virtual;
    begin
      var len := Sqrt(dx*dx+dy*dy);
      if len = 0 then
        exit;
      var dvx := dx/len*Velocity;
      var dvy := dy/len*Velocity;
      MoveBy(dvx*dt,dvy*dt);
    end;
    /// Поворачивает графический объект по часовой стрелке на угол a
    procedure Rotate(a: real) := RotateAngle += a;
    /// Поворачивает графический объект так чтобы он "смотрел" на точку (x,y)
    procedure RotateToPoint(x,y: real);
    begin
      x -= Center.X;
      y -= Center.y;
      // Надо определить угол
      var phi := 0.0;
      if x>0 then
      begin
        phi := ArcTan(y/x)*180/Pi+90
      end  
      else if x<0 then   
      begin
        phi := (Pi+ArcTan(y/x))*180/Pi+90;
      end;  
      RotateAngle := phi;  
    end;
    /// Масштабирует графический объект в r раз относительно текущего размера
    procedure Scale(r: real) := ScaleFactor *= r;
  
  private
    procedure AnimMoveByP(a,b,sec: real);
    begin
      var (x,y) := (transl.X,transl.Y);
      var ax := new DoubleAnimation(a + x, System.TimeSpan.FromSeconds(sec));
      var ay := new DoubleAnimation(b + y, System.TimeSpan.FromSeconds(sec));
      ax.FillBehavior := FillBehavior.Stop;
      ay.FillBehavior := FillBehavior.Stop;
      var transl1 := transl;
      ax.Completed += (o,e) -> begin
        transl1.X := x + a; // transl - ошибка с лямбдами!
      end;  
      ay.Completed += (o,e) -> begin
        transl1.Y := y + b;
      end;  
      transl.BeginAnimation(TranslateTransform.XProperty, ax, HandoffBehavior.Compose);
      transl.BeginAnimation(TranslateTransform.YProperty, ay, HandoffBehavior.Compose);
    end;
  private
    procedure AnimMoveToP(x,y,sec: real);
    begin
      var ax := new DoubleAnimation(x, System.TimeSpan.FromSeconds(sec));
      var ay := new DoubleAnimation(y, System.TimeSpan.FromSeconds(sec));
      ax.FillBehavior := FillBehavior.Stop;
      ay.FillBehavior := FillBehavior.Stop;
      var transl1 := transl;
      ax.Completed += (o,e) -> begin
        transl1.X := x;
      end;  
      ay.Completed += (o,e) -> begin
        transl1.Y := y;
      end;  
      transl.BeginAnimation(TranslateTransform.XProperty, ax, HandoffBehavior.Compose);
      transl.BeginAnimation(TranslateTransform.YProperty, ay, HandoffBehavior.Compose);
    end;
    procedure AnimMoveEndP;
    begin
      var animation := new DoubleAnimation();
      animation.BeginTime := nil;
      transl.BeginAnimation(TranslateTransform.XProperty, animation);
      transl.BeginAnimation(TranslateTransform.YProperty, animation);
    end;
    procedure AnimRotateP(a,sec: real);
    begin
      var an := new DoubleAnimation(a + rot.Angle, System.TimeSpan.FromSeconds(sec));
      an.FillBehavior := FillBehavior.Stop;
      an.Completed += (o,e) -> begin
        rot.Angle := a + rot.Angle;
      end;  
      rot.BeginAnimation(RotateTransform.AngleProperty, an, HandoffBehavior.Compose);
    end;
    procedure AnimScaleP(a,sec: real);
    begin
      var an := new DoubleAnimation(a, System.TimeSpan.FromSeconds(sec));
      sca.CenterX := Width / 2;
      sca.CenterY := Height / 2;
      an.FillBehavior := FillBehavior.Stop;
      an.Completed += (o,e) -> begin
        sca.ScaleX := a;
        sca.ScaleY := a;
      end;  
      sca.BeginAnimation(ScaleTransform.ScaleXProperty, an, HandoffBehavior.Compose);
      sca.BeginAnimation(ScaleTransform.ScaleYProperty, an, HandoffBehavior.Compose);
    end;
  public
    /// Анимирует перемещение графического объекта на вектор (a,b) в течение sec секунд
    procedure AnimMoveBy(a,b: real; sec: real := 1) := Invoke(AnimMoveByP,a,b,sec);
    ///--
    procedure AnimMoveOn(a,b: real; sec: real := 1) := AnimMoveBy(a,b,sec);
    /// Анимирует перемещение графического объекта в направлении RotateAngle (вверх при RotateAngle=0)
    procedure AnimMoveForward(r: real);
    begin
      var a := Pi/180*(90-RotateAngle);
      AnimMoveBy(r*Cos(a),-r*Sin(a));
    end;
    /// Анимирует перемещение графического объекта к точке (x,y) в течение sec секунд
    procedure AnimMoveTo(x,y: real; sec: real := 1) := Invoke(AnimMoveToP,x,y,sec);
    /// Завершает анимацию перемещения
    procedure AnimMoveEnd := Invoke(AnimMoveEndP);
    /// Анимирует вращение графического объекта на угол a в течение sec секунд
    procedure AnimRotate(a: real; sec: real := 1) := Invoke(AnimRotateP,a,sec);
    /// Анимирует масштабирование графического объекта на величину a в течение sec секунд
    procedure AnimScale(a: real; sec: real := 1) := Invoke(AnimScaleP,a,sec);
    /// Добавляет к графическому объекту дочерний
    procedure AddChild(ch: ObjectWPF; al: Alignment := Alignment.LeftTop);
    /// Удаляет из графического объекта дочерний
    procedure DeleteChild(ch: ObjectWPF);
    begin
      if not ChildrenWPF.Contains(ch) then
        raise new Exception('Удаляемый объект не является дочерним для данного');
      Invoke(DeleteChildP,ch);
    end;  
    /// Удаляет графический объект
    procedure Destroy;
    /// Переносит графический объект на передний план
    procedure ToFront;
    /// Переносит графический объект на задний план
    procedure ToBack;
    /// Определяет, пересекается ли объект с объектом ob
    function Intersects(ob: ObjectWPF): boolean;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real; fontname: string; c: GColor): ObjectWPF;
    begin
      Text := txt; 
      FontSize := size;
      Self.FontName := fontname;
      Self.FontColor := c;
      Result := Self;
    end;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real := 16; fontname: string := 'Arial'): ObjectWPF := SetText(txt,size,fontname,Colors.Black);
    /// Декоратор поворота объекта
    function SetRotate(da: real): ObjectWPF;
    begin
      Rotate(da);
      Result := Self;
    end;
    /// Объект находится вне границ графического окна
    function OutOfGraphWindow: boolean;
    begin
      Result := (Left < 0) or (Top < 0) or (Right > GraphWindow.Width) or (Bottom > GraphWindow.Height);
    end;
    /// Tag хранит любые присоединённые характеристики объекта
    auto property Tag: object;
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
    procedure EF(value: GColor) := Element.Fill := ColorBrush(Value);
    procedure EST(value: real);
    begin
      Element.StrokeThickness := Value;
      if Element.Stroke = nil then
        Element.Stroke := ColorBrush(Colors.Black)
    end;  
    function RemoveBorderP: BoundedObjectWPF;
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
      write Invoke(()->(Element.Stroke := ColorBrush(Value)));
    /// Ширина границы графического объекта
    property BorderWidth: real 
      read InvokeReal(()->Element.StrokeThickness)
      write Invoke(EST,value);
    /// Декоратор включения границы объекта
    function SetBorder(w: real; c: GColor): BoundedObjectWPF;
    begin
      //if c<>BorderColor then
      BorderColor := c;
      //if w<>BorderWidth then  
      BorderWidth := w;
      Result := Self;
    end;
    /// Декоратор включения границы объекта
    function SetBorder(w: real := 1): BoundedObjectWPF := SetBorder(w,Colors.Black);
    /// Декоратор выключения границы объекта
    function RemoveBorder: BoundedObjectWPF 
      := Invoke&<BoundedObjectWPF>(RemoveBorderP);
  end;

// -----------------------------------------------------
//>>     Класс EllipseWPF # Class EllipseWPF 
// -----------------------------------------------------
  /// Класс графических объектов "Эллипс"
  EllipseWPF = class(BoundedObjectWPF)
  private
    procedure InitOb2(x,y,r1,r2: real; c: GColor) := InitOb1(x-r1,y-r2,2*r1,2*r2,c,new System.Windows.Shapes.Ellipse());
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
    procedure RadX(value: real);
    begin
      Left -= value - gr.Width/2;
      gr.Width := value*2;
      ob.Width := value*2;
    end;  
    procedure RadY(value: real);
    begin
      Top -= value - gr.Width/2;
      gr.Height := value*2;
      ob.Height := value*2;
    end;  
  public
    /// Создает эллипс с центром в точке (x,y), радиусами (rx,ry) и цветом внутренности с
    constructor (x,y,rx,ry: real; c: GColor) := Invoke(InitOb2,x,y,rx,ry,c);
    /// Создает эллипс с центром в точке (x,y), радиусами (rx,ry) и цветом внутренности с, с границей ширины borderWidth и цвета borderColor
    constructor (x,y,rx,ry: real; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,x,y,rx,ry,c); if borderWidth > 0 then SetBorder(borderWidth,borderColor); end;
    /// Создает эллипс с центром в точке (x,y), радиусами (rx,ry) и цветом внутренности с, с границей ширины borderWidth и цвета borderColor
    constructor (x,y,rx,ry: real; c: GColor; borderWidth: real) := Create(x,y,rx,ry,c,borderWidth,Colors.Black);
    /// Создает эллипс с центром в точке p, радиусами (rx,ry) и цветом внутренности с
    constructor (p: Point; rx,ry: real; c: GColor) := Invoke(InitOb2,p.x,p.y,rx,ry,c);
    /// Создает эллипс с центром в точке p, радиусами (rx,ry) и цветом внутренности с, с границей ширины borderWidth и цвета borderColor
    constructor (p: Point; rx,ry: real; c: GColor; borderWidth: real; borderColor: GColor := Colors.Black) := begin Invoke(InitOb2,p.x,p.y,rx,ry,c); if borderWidth > 0 then SetBorder(borderWidth,borderColor); end;
    /// Декоратор включения границы объекта
    function SetBorder(w: real; c: GColor) := inherited SetBorder(w,c) as EllipseWPF;
    /// Декоратор включения границы объекта
    function SetBorder(w: real := 1) := SetBorder(w,Colors.Black);
    /// Декоратор выключения границы объекта
    function RemoveBorder := inherited RemoveBorder as EllipseWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real; fontname: string; c: GColor): EllipseWPF 
      := inherited SetText(txt,size,fontname,c) as EllipseWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real := 16; fontname: string := 'Arial'): EllipseWPF 
      := SetText(txt,size,fontname,Colors.Black);
    /// Декоратор поворота объекта
    function SetRotate(da: real): EllipseWPF := inherited SetRotate(da) as EllipseWPF;
    /// Радиус эллипса по оси OX
    property RadiusX: real 
      read InvokeReal(()->ob.Width/2) 
      write Invoke(RadX,Value);
    /// Радиус эллипса по оси OY
    property RadiusY: real 
      read InvokeReal(()->ob.Height/2) 
      write Invoke(RadY,Value);
  end;

// -----------------------------------------------------
//>>     Класс CircleWPF # Class CircleWPF
// -----------------------------------------------------
  /// Класс графических объектов "Окружность"
  CircleWPF = class(BoundedObjectWPF)
  private
    procedure InitOb2(x,y,r: real; c: GColor) := InitOb1(x-r,y-r,2*r,2*r,c,new System.Windows.Shapes.Ellipse());
    procedure WT(value: real) := begin (ob.Width,ob.Height) := (value,value); (gr.Width,gr.Height) := (value,value); end;
    procedure HT(value: real) := begin (ob.Width,ob.Height) := (value,value); (gr.Width,gr.Height) := (value,value); end;
    
    procedure Rad(value: real);
    begin
      //(ob as Ellipse).RenderedGeometry
      Left -= value - gr.Width/2;
      Top -= value - gr.Width/2;
      (gr.Width,gr.Height) := (value*2,value*2);
      (ob.Width,ob.Height) := (value*2,value*2);
    end;  
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;
  public
    /// Создает круг радиуса r заданного цвета с координатами центра (x,y)
    constructor (x,y,r: real; c: GColor) := Invoke(InitOb2,x,y,r,c);
    /// Создает круг радиуса r заданного цвета с координатами центра (x,y), с границей ширины borderWidth
    constructor (x,y,r: real; c: GColor; borderWidth: real) := Create(x,y,r,c,borderWidth,Colors.Black);
    /// Создает круг радиуса r заданного цвета с координатами центра (x,y), с границей ширины borderWidth и цвета borderColor
    constructor (x,y,r: real; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,x,y,r,c); SetBorder(borderWidth,borderColor); end;
    /// Создает круг радиуса r заданного цвета с центром p
    constructor (p: Point; r: real; c: GColor) := Invoke(InitOb2,p.x,p.y,r,c);
    /// Создает круг радиуса r заданного цвета с центром p, с границей ширины borderWidth 
    constructor (p: Point; r: real; c: GColor; borderWidth: real) := Create(p,r,c,borderWidth,Colors.Black);
    /// Создает круг радиуса r заданного цвета с центром p, с границей ширины borderWidth и цвета borderColor
    constructor (p: Point; r: real; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,p.x,p.y,r,c); SetBorder(borderWidth,borderColor); end;
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
    function SetBorder(w: real; c: GColor) := inherited SetBorder(w,c) as CircleWPF;
    /// Декоратор включения границы объекта
    function SetBorder(w: real := 1) := SetBorder(w,Colors.Black);
    /// Декоратор выключения границы объекта
    function RemoveBorder := inherited RemoveBorder as CircleWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real; fontname: string; c: GColor): CircleWPF
      := inherited SetText(txt,size,fontname,c) as CircleWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real := 16; fontname: string := 'Arial'): CircleWPF 
      := SetText(txt, size, fontname, Colors.Black);
    /// Декоратор поворота объекта
    function SetRotate(da: real): CircleWPF := inherited SetRotate(da) as CircleWPF;
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
    /// Создает прямоугольник размера (w,h) заданного цвета с координатами левого верхнего угла (x,y), с границей ширины borderWidth и цвета borderColor
    constructor (x,y,w,h: real; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,x,y,w,h,c); SetBorder(borderWidth,borderColor); end;
    /// Создает прямоугольник размера (w,h) заданного цвета с координатами левого верхнего угла (x,y), с границей ширины borderWidth
    constructor (x,y,w,h: real; c: GColor; borderWidth: real) := Create(x,y,w,h,c,borderWidth,Colors.Black);
    /// Создает прямоугольник размера (w,h) заданного цвета с координатами левого верхнего угла, задаваемыми точкой p
    constructor (p: Point; w,h: real; c: GColor) := Invoke(InitOb2,p.x,p.y,w,h,c);
    /// Создает прямоугольник размера (w,h) заданного цвета с координатами левого верхнего угла, задаваемыми точкой p, с границей ширины borderWidth и цвета borderColor
    constructor (p: Point; w,h: real; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,p.x,p.y,w,h,c); SetBorder(borderWidth,borderColor); end;
    /// Создает прямоугольник размера (w,h) заданного цвета с координатами левого верхнего угла, задаваемыми точкой p, с границей ширины borderWidth
    constructor (p: Point; w,h: real; c: GColor; borderWidth: real) := Create(p,w,h,c,borderWidth,Colors.Black);
    /// Декоратор включения границы объекта
    function SetBorder(w: real := 1; c: GColor := Colors.Black) := inherited SetBorder(w,c) as RectangleWPF;
    /// Декоратор выключения границы объекта
    function RemoveBorder := inherited RemoveBorder as RectangleWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real; fontname: string; c: GColor): RectangleWPF
      := inherited SetText(txt,size,fontname,c) as RectangleWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real := 16; fontname: string := 'Arial'): RectangleWPF 
      := SetText(txt,size,fontname,Colors.Black);
    /// Декоратор поворота объекта
    function SetRotate(da: real): RectangleWPF := inherited SetRotate(da) as RectangleWPF;
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
    /// Создает квадрат со стороной w заданного цвета с координатами левого верхнего угла (x,y), с границей ширины borderWidth
    constructor (x,y,w: real; c: GColor; borderWidth: real) := Create(x,y,w,c,borderWidth,Colors.Black);
    /// Создает квадрат со стороной w заданного цвета с координатами левого верхнего угла (x,y), с границей ширины borderWidth и цвета borderColor
    constructor (x,y,w: real; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,x,y,w,c); SetBorder(borderWidth,borderColor); end;
    /// Создает квадрат со стороной w заданного цвета с координатами левого верхнего угла, задаваемыми точкой p
    constructor (p: Point; w: real; c: GColor) := Invoke(InitOb2,p.x,p.y,w,c);
    /// Создает квадрат со стороной w заданного цвета с координатами левого верхнего угла, задаваемыми точкой p, с границей ширины borderWidth и цвета borderColor
    constructor (p: Point; w: real; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,p.x,p.y,w,c); SetBorder(borderWidth,borderColor); end;
    /// Создает квадрат со стороной w заданного цвета с координатами левого верхнего угла, задаваемыми точкой p, с границей ширины borderWidth
    constructor (p: Point; w: real; c: GColor; borderWidth: real) 
      := Create(p,w,c,borderWidth,Colors.Black);
    /// Декоратор включения границы объекта
    function SetBorder(w: real; c: GColor) := inherited SetBorder(w,c) as SquareWPF;
    /// Декоратор включения границы объекта
    function SetBorder(w: real := 1) := SetBorder(w,Colors.Black);
    /// Декоратор выключения границы объекта
    function RemoveBorder := inherited RemoveBorder as SquareWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real; fontname: string; c: GColor): SquareWPF
      := inherited SetText(txt,size,fontname,c) as SquareWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real := 16; fontname: string := 'Arial'): SquareWPF
      := SetText(txt,size,fontname,Colors.Black);
    /// Декоратор поворота объекта
    function SetRotate(da: real): SquareWPF
      := inherited SetRotate(da) as SquareWPF;
  end;
  
// -----------------------------------------------------
//>>     Класс RoundRectWPF # Class RoundRectWPF
// -----------------------------------------------------
  /// Класс графических объектов "Прямоугольник со скругленными краями"
  RoundRectWPF = class(BoundedObjectWPF)
    function Element := ob as System.Windows.Shapes.Rectangle;
    procedure InitOb2(x,y,w,h,r: real; c: GColor);
    begin
      var rr := new Rectangle();
      rr.RadiusX := r;
      rr.RadiusY := r;
      InitOb1(x,y,w,h,c,rr);
    end;
    function GetInternalGeometry: Geometry; override := Element.RenderedGeometry;
  public
    /// Создает прямоугольник со скругленными краями размера (w,h) с радиусом скругления r заданного цвета с координатами левого верхнего угла (x,y)
    constructor (x,y,w,h,r: real; c: GColor) := Invoke(InitOb2,x,y,w,h,r,c);
    /// Создает прямоугольник со скругленными краями размера (w,h) с радиусом скругления r заданного цвета с координатами левого верхнего угла (x,y), с границей ширины borderWidth и цвета borderColor
    constructor (x,y,w,h,r: real; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,x,y,w,h,r,c); SetBorder(borderWidth,borderColor); end;
    /// Создает прямоугольник со скругленными краями размера (w,h) с радиусом скругления r заданного цвета с координатами левого верхнего угла (x,y), с границей ширины borderWidth
    constructor (x,y,w,h,r: real; c: GColor; borderWidth: real) := Create(x,y,w,h,r,c,borderWidth,Colors.Black);
    /// Создает прямоугольник со скругленными краями размера (w,h) с радиусом скругления r заданного цвета с координатами левого верхнего угла, задаваемыми точкой p
    constructor (p: Point; w,h,r: real; c: GColor) := Invoke(InitOb2,p.x,p.y,w,h,r,c);
    /// Создает прямоугольник со скругленными краями размера (w,h) с радиусом скругления r заданного цвета с координатами левого верхнего угла, задаваемыми точкой p, с границей ширины borderWidth и цвета borderColor
    constructor (p: Point; w,h,r: real; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,p.x,p.y,w,h,r,c); SetBorder(borderWidth,borderColor); end;
    /// Создает прямоугольник со скругленными краями размера (w,h) с радиусом скругления r заданного цвета с координатами левого верхнего угла, задаваемыми точкой p, с границей ширины borderWidth
    constructor (p: Point; w,h,r: real; c: GColor; borderWidth: real) 
      := Create(p,w,h,r,c,borderWidth,Colors.Black);
    /// Декоратор включения границы объекта
    function SetBorder(w: real; c: GColor) := inherited SetBorder(w,c) as RoundRectWPF;
    /// Декоратор выключения границы объекта
    function SetBorder(w: real := 1) := SetBorder(w,Colors.Black);
    /// Декоратор выключения границы объекта
    function RemoveBorder := inherited RemoveBorder as RoundRectWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real; fontname: string; c: GColor): RoundRectWPF
      := inherited SetText(txt,size,fontname,c) as RoundRectWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real := 16; fontname: string := 'Arial'): RoundRectWPF
      := SetText(txt,size,fontname,Colors.Black);
    /// Декоратор поворота объекта
    function SetRotate(da: real): RoundRectWPF := inherited SetRotate(da) as RoundRectWPF;
    /// Радиус скругления
    property RoundRadius: real 
      read InvokeReal(()->(ob as Rectangle).RadiusX)
      write begin
        Invoke(procedure->begin var r := Self.Element; r.RadiusX := value; r.Radiusy := value end);
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
    /// Создает квадрат со скругленными краями со стороной w с радиусом скругления r заданного цвета с координатами левого верхнего угла (x,y), с границей ширины borderWidth и цвета borderColor
    constructor (x,y,w,r: real; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,x,y,w,r,c); SetBorder(borderWidth,borderColor); end;
    /// Создает квадрат со скругленными краями со стороной w с радиусом скругления r заданного цвета с координатами левого верхнего угла (x,y), с границей ширины borderWidth
    constructor (x,y,w,r: real; c: GColor; borderWidth: real) 
      := Create(x,y,w,r,c,borderWidth,Colors.Black);
    /// Создает квадрат со скругленными краями со стороной w с радиусом скругления r заданного цвета с координатами левого верхнего угла, задаваемыми точкой p
    constructor (p: Point; w,r: real; c: GColor) := Invoke(InitOb2,p.x,p.y,w,r,c);
    /// Создает квадрат со скругленными краями со стороной w с радиусом скругления r заданного цвета с координатами левого верхнего угла, задаваемыми точкой p, с границей ширины borderWidth и цвета borderColor
    constructor (p: Point; w,r: real; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,p.x,p.y,w,r,c); SetBorder(borderWidth,borderColor); end;
    /// Создает квадрат со скругленными краями со стороной w с радиусом скругления r заданного цвета с координатами левого верхнего угла, задаваемыми точкой p, с границей ширины borderWidth
    constructor (p: Point; w,r: real; c: GColor; borderWidth: real) 
      := Create(p,w,r,c,borderWidth,Colors.Black);
    /// Декоратор включения границы объекта
    function SetBorder(w: real; c: GColor): RoundSquareWPF 
      := inherited SetBorder(w,c) as RoundSquareWPF;
    /// Декоратор включения границы объекта
    function SetBorder(w: real := 1): RoundSquareWPF := SetBorder(w,Colors.Black);
    /// Декоратор выключения границы объекта
    function RemoveBorder: RoundSquareWPF
      := inherited RemoveBorder as RoundSquareWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real; fontname: string; c: GColor): RoundSquareWPF 
      := inherited SetText(txt,size,fontname,c) as RoundSquareWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real := 16; fontname: string := 'Arial'): RoundSquareWPF 
      := SetText(txt, size, fontname, Colors.Black);
    /// Декоратор поворота объекта
    function SetRotate(da: real): RoundSquareWPF := inherited SetRotate(da) as RoundSquareWPF;
  end;

  MyText = class(FrameworkElement)
    public
      text: string;
      sz: real;
      ft: FormattedText;
      name: string;
      c: Color;
      bc: Color := EmptyColor;
      constructor (txt,name: string; sz: real; col: Color);
      begin
        text := txt; Self.sz := sz; Self.name := name; Self.c := col; 
        RecreateFormText;
      end;
      procedure RecreateFormText;
      begin
        ft := new FormattedText(text,new System.Globalization.CultureInfo('ru-ru'), 
                System.Windows.FlowDirection.LeftToRight,
                new Typeface(name), sz, ColorBrush(c));
        Width := ft.Width;
        Height := ft.Height;
      end;
      procedure OnRender(dc: DrawingContext); override;
      begin
        if bc<>EmptyColor then
          dc.DrawRectangle(ColorBrush(bc),nil,Rect(0,0,Width,Height));
        dc.DrawText(ft,new Point(0,0));
      end;
  end;

// -----------------------------------------------------
//>>     Класс TextWPF # Class TextWPF
// -----------------------------------------------------
  /// Класс графических объектов "Текст"
  TextWPF = class(ObjectWPF)
  private
    function Element := ob as MyText;

    procedure InitOb2(x,y,sz: real; txt: string; c: GColor);
    begin
      var rr := new MyText(txt,'Arial',sz,c);
      InitOb(x,y,rr.Width,rr.Height,rr);
    end;
    function GetInternalGeometry: Geometry; override;
    begin
      var r := Rect(0,0,Width,Height);
      Result := new RectangleGeometry(r);
    end;  
  public
    /// Создает текст заданного цвета с координатами левого верхнего угла (x,y)
    constructor (x,y: real; txt: string; c: GColor := Colors.Black) := Invoke(InitOb2,x,y,16,txt,c);
    /// Создает текст заданного цвета с координатами левого верхнего угла (x,y) и размером шрифта sz
    constructor (x,y,sz: real; txt: string; c: GColor) := begin Invoke(InitOb2,x,y,sz,txt,c); FontSize := sz; end;
    /// Создает текст заданного цвета с координатами левого верхнего угла (x,y) и размером шрифта sz
    constructor (x,y,sz: real; txt: string) := Create(x,y,sz,txt,Colors.Black);
    /// Размер шрифта
    property FontSize: real read InvokeReal(()->Self.Element.sz) write 
      Invoke(procedure->begin Self.Element.sz := value; Self.Element.RecreateFormText; Width := Self.Element.Width; Height := Self.Element.Height; ob.InvalidateVisual; end); override;
    /// Имя шрифта
    property FontName: string read Element.name 
      write Invoke(procedure->begin Self.Element.name := value; Self.Element.RecreateFormText; Width := Self.Element.Width; Height := Self.Element.Height; ob.InvalidateVisual end);
    /// Цвет шрифта
    property Color: GColor read Invoke&<GColor>(()->Element.C) 
      write Invoke(()->begin Self.Element.C := value; Self.Element.RecreateFormText; ob.InvalidateVisual end); override;
    /// Цвет фона 
    property BackgroundColor: GColor read Invoke&<GColor>(()->Element.bc) 
      write Invoke(()->begin Self.Element.bc := value; Self.Element.RecreateFormText; ob.InvalidateVisual end);
    /// Текст графического объекта
    property Text: string read InvokeString(()->Element.Text) 
      write Invoke(procedure->begin Self.Element.Text := value; Self.Element.RecreateFormText; Width := Self.Element.Width; Height := Self.Element.Height; ob.InvalidateVisual end); override;
    /// Декоратор поворота объекта
    function SetRotate(da: real): TextWPF := inherited SetRotate(da) as TextWPF;
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
    procedure ES(value: GColor) := Element.Stroke := ColorBrush(Value);
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
    function SetText(txt: string; size: real; fontname: string; c: GColor): LineWPF
      := inherited SetText(txt,size,fontname,c) as LineWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real := 16; fontname: string := 'Arial'): LineWPF
      := SetText(txt, size, fontname, Colors.Black);
    /// Декоратор поворота объекта
    function SetRotate(da: real): LineWPF
      := inherited SetRotate(da) as LineWPF;
    /// Декоратор ширины линии отрезка
    function SetLineWidth(lw: real): LineWPF;
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
    function GetInternalGeometry: Geometry; override := (ob as Shape).RenderedGeometry;//new EllipseGeometry(Center,Width/2,Height/2);  
  public
    /// Создает правильный многоугольник заданного цвета с координатами центра (x,y) и радиусом описанной окружности r
    constructor (x,y,r: real; n: integer; c: GColor) := Invoke(InitOb2,x,y,r,n,c);
    /// Создает правильный многоугольник заданного цвета с координатами центра (x,y) и радиусом описанной окружности r, с границей ширины borderWidth и цвета borderColor
    constructor (x,y,r: real; n: integer; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,x,y,r,n,c); SetBorder(borderWidth,borderColor); end;
    /// Создает правильный многоугольник заданного цвета с координатами центра (x,y) и радиусом описанной окружности r, с границей ширины borderWidth
    constructor (x,y,r: real; n: integer; c: GColor; borderWidth: real) 
      := Create(x,y,r,n,c,borderWidth,Colors.Black);
    /// Создает правильный многоугольник заданного цвета с центром в заданной точке p и радиусом описанной окружности r
    constructor (p: Point; r: real; n: integer; c: GColor) := Create(p.X,p.Y,r,n,c);
    /// Создает правильный многоугольник заданного цвета с центром в заданной точке p и радиусом описанной окружности r, с границей ширины borderWidth и цвета borderColor
    constructor (p: Point; r: real; n: integer; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,p.x,p.y,r,n,c); SetBorder(borderWidth,borderColor); end;
    /// Создает правильный многоугольник заданного цвета с центром в заданной точке p и радиусом описанной окружности r, с границей ширины borderWidth
    constructor (p: Point; r: real; n: integer; c: GColor; borderWidth: real) 
      := Create(p,r,n,c,borderWidth,Colors.Black);
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
    function SetBorder(w: real; c: GColor): RegularPolygonWPF
      := inherited SetBorder(w,c) as RegularPolygonWPF;
    /// Декоратор включения границы объекта
    function SetBorder(w: real := 1): RegularPolygonWPF
      := SetBorder(w,Colors.Black);
    /// Декоратор выключения границы объекта
    function RemoveBorder: RegularPolygonWPF 
      := inherited RemoveBorder as RegularPolygonWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real; fontname: string; c: GColor): RegularPolygonWPF  
      := inherited SetText(txt,size,fontname,c) as RegularPolygonWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real := 16; fontname: string := 'Arial'): RegularPolygonWPF  
      := SetText(txt, size, fontname, Colors.Black);
    /// Декоратор поворота объекта
    function SetRotate(da: real): RegularPolygonWPF  
      := inherited SetRotate(da) as RegularPolygonWPF;
  end;
  
// -----------------------------------------------------
//>>     Класс StarWPF # Class StarWPF
// -----------------------------------------------------
  /// Класс графических объектов "Звезда"
  StarWPF = class(RegularPolygonWPF)
  private
    rint: real;
    function Element := ob as System.Windows.Shapes.Polygon;
    procedure InitOb2(x,y,r,rint: real; n: integer; c: GColor);
    begin 
      InitOb1(x-r,y-r,2*r,2*r,c,CreatePolygon(r,rint,n),false);
      (Self.Left,Self.Top,Self.rint,Self.n) := (x-r,y-r,rint,n);
    end;  
    function ChangePointCollection(r,rint: real; n: integer): PointCollection; 
    begin
      var pp1 := PartitionPoints(0+2*Pi/n,2*Pi,n-1).Select(phi->Pnt(r+r*cos(phi-Pi/2),r+r*sin(phi-Pi/2)));
      var pp2 := PartitionPoints(0+3*Pi/n,2*Pi+Pi/n,n-1).Select(phi->Pnt(r+rint*cos(phi-Pi/2),r+rint*sin(phi-Pi/2)));
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
    /// Создает звезду заданного цвета с координатами центра (x,y), радиусом описанной окружности r и внутренним радиусом rinternal, с границей ширины borderWidth и цвета borderColor
    constructor (x,y,r,rinternal: real; n: integer; c: GColor; borderWidth: real; borderColor: GColor);
    begin
      if rinternal<r then
        Invoke(InitOb2,x,y,r,rinternal,n,c)
      else Invoke(InitOb2,x,y,rinternal,r,n,c);
      SetBorder(borderWidth,borderColor);
    end; 
    /// Создает звезду заданного цвета с координатами центра (x,y), радиусом описанной окружности r и внутренним радиусом rinternal, с границей ширины borderWidth
    constructor (x,y,r,rinternal: real; n: integer; c: GColor; borderWidth: real)
      := Create(x,y,r,rinternal, n, c, borderWidth, Colors.Black);
    /// Создает звезду заданного цвета c центром в точке p, радиусом описанной окружности r и внутренним радиусом rinternal, с границей ширины borderWidth и цвета borderColor
    constructor (p: Point; r,rinternal: real; n: integer; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,p.X,p.Y,r,rinternal,n,c); SetBorder(borderWidth,borderColor); end;
    /// Создает звезду заданного цвета c центром в точке p, радиусом описанной окружности r и внутренним радиусом rinternal, с границей ширины borderWidth
    constructor (p: Point; r,rinternal: real; n: integer; c: GColor; borderWidth: real) 
      := Create(p,r,rinternal, n, c, borderWidth, Colors.Black);
    /// Радиус описанной окружности
    property Radius: real 
      read InvokeReal(()->gr.Height/2) 
      write Invoke(Rad,Value); override;
    /// Внутренний радиус
    property InternalRadius: real 
      read rint 
      write Invoke(IntRad,Value);
    /// Количество вершин
    property Count: integer
      read InvokeInteger(()->n) 
      write Invoke(Cnt,Value);
    /// Декоратор включения границы объекта
    function SetBorder(w: real; c: GColor): StarWPF 
      := inherited SetBorder(w,c) as StarWPF;
    /// Декоратор включения границы объекта
    function SetBorder(w: real := 1): StarWPF 
      := SetBorder(w,Colors.Black);
    /// Декоратор выключения границы объекта
    function RemoveBorder: StarWPF 
      := inherited RemoveBorder as StarWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real; fontname: string; c: GColor): StarWPF 
      := inherited SetText(txt,size,fontname,c) as StarWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real := 16; fontname: string := 'Arial'): StarWPF 
      := SetText(txt, size , fontname, Colors.Black);
    /// Декоратор поворота объекта
    function SetRotate(da: real): StarWPF 
      := inherited SetRotate(da) as StarWPF;
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
    function Element := ob as System.Windows.Shapes.Polygon;
    /// Создает многоугольник заданного цвета с координатами вершин, заданными массивом точек pp
    constructor (pp: array of Point; c: GColor) := Invoke(InitOb2,pp,c);
    /// Создает многоугольник заданного цвета с координатами вершин, заданными массивом точек pp, с границей ширины borderWidth и цвета borderColor
    constructor (pp: array of Point; c: GColor; borderWidth: real; borderColor: GColor) := begin Invoke(InitOb2,pp,c); SetBorder(borderWidth,borderColor); end;
    /// Создает многоугольник заданного цвета с координатами вершин, заданными массивом точек pp, с границей ширины borderWidth
    constructor (pp: array of Point; c: GColor; borderWidth: real) 
      := Create(pp,c,borderWidth,Colors.Black);
    /// Массив вершин
    property Points: array of Point
      read Invoke&<PointsArray>(GetPointsArrayP)
      write begin
        var ob1 := ob as System.Windows.Shapes.Polygon;
        var pp := value;
        // ширина и высота будут некорректно. Надо переопределить на чтение
        var x1 := pp.Min(p->p.x);
        //var x2 := pp.Max(p->p.x);
        var y1 := pp.Min(p->p.y);
        //var y2 := pp.Max(p->p.y);
        var a := pp.Select(p->Pnt(p.x-x1,p.y-y1)).ToArray;
        MoveTo(x1,y1);
        //(gr.Width,gr.Height) := (x2-x1,y2-y1);
        Invoke(procedure -> ob1.Points := new PointCollection(a));
      end;  
    /// Декоратор включения границы объекта
    function SetBorder(w: real; c: GColor): PolygonWPF
      := inherited SetBorder(w,c) as PolygonWPF;
    /// Декоратор включения границы объекта
    function SetBorder(w: real := 1): PolygonWPF
      := SetBorder(w,Colors.Black);
    /// Декоратор выключения границы объекта
    function RemoveBorder: PolygonWPF 
      := inherited RemoveBorder as PolygonWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real; fontname: string; c: GColor): PolygonWPF 
      := inherited SetText(txt,size,fontname,c) as PolygonWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real := 16; fontname: string := 'Arial'): PolygonWPF 
      := SetText(txt,size,fontname,Colors.Black);
    /// Декоратор поворота объекта
    function SetRotate(da: real): PolygonWPF
      := inherited SetRotate(da) as PolygonWPF;
  end;

// -----------------------------------------------------
//>>     Класс PictureWPF # Class PictureWPF
// -----------------------------------------------------
  /// Класс графических объектов "Рисунок"
  PictureWPF = class(ObjectWPF)
  protected
    function CreateBitmapImage(fname: string) := new BitmapImage(new System.Uri(fname,System.UriKind.Relative)); 
    procedure Rest(x,y,w,h: real; b: BitmapImage);
    begin
      var im := new System.Windows.Controls.Image();
      im.Source := b;
      im.Width := w;
      im.Height := h;

      InitOb(x,y,w,h,im);
    end;
  private
    procedure InitOb3(x,y,w,h: real; fname: string);
    begin
      var b := CreateBitmapImage(fname);
      Rest(x,y,w,h,b);
    end;

    procedure InitOb2(x,y: real; fname: string);
    begin
      var b := CreateBitmapImage(fname);
      Rest(x,y,b.PixelWidth,b.PixelHeight,b);
    end;

    procedure InitObHidden(x,y: real; fname: string);
    begin
      var b := CreateBitmapImage(fname);
      var im := new System.Windows.Controls.Image();
      im.Source := b;
      im.Width := b.PixelWidth;
      im.Height := b.PixelHeight;
      InitOb(x,y,b.PixelWidth,b.PixelHeight,im,True,True);
    end;
    function GetInternalGeometry: Geometry; override;
    begin
      var r := Rect(0,0,Width,Height);
      Result := new RectangleGeometry(r);
    end;  
    ///
    constructor Create(Hidden: boolean; x,y: real; fname: string) := Invoke(InitObHidden,x,y,fname);
  public
    function Element := ob as System.Windows.Controls.Image;
    static function CreateInvisible(x,y: real; fname: string): PictureWPF := new PictureWPF(False,x,y,fname);
    /// Создает рисунок из файла fname с координатами левого верхнего угла (x,y)
    constructor (x,y: real; fname: string) := Invoke(InitOb2,x,y,fname);
    /// Создает рисунок из файла fname с координатами левого верхнего угла (x,y) и размерами (w,h)
    constructor (x,y,w,h: real; fname: string) := Invoke(InitOb3,x,y,w,h,fname);
    /// Создает рисунок из файла fname с координатой левого верхнего угла, заданной точкой p
    constructor (p: Point; fname: string) := Invoke(InitOb2,p.x,p.y,fname);
    /// Создает рисунок из файла fname  с координатой левого верхнего угла, заданной точкой p, и размерами (w,h)
    constructor (p: Point; w,h: real; fname: string) := Invoke(InitOb3,p.x,p.y,w,h,fname);
    /// Декоратор текста объекта
    function SetText(txt: string; size: real; fontname: string; c: GColor): PictureWPF  
      := inherited SetText(txt,size,fontname,c) as PictureWPF;
    /// Декоратор текста объекта
    function SetText(txt: string; size: real := 16; fontname: string := 'Arial'): PictureWPF  
      := SetText(txt, size, fontname, Colors.Black);
    /// Декоратор поворота объекта
    function SetRotate(da: real): PictureWPF := inherited SetRotate(da) as PictureWPF;
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
  /// Событие перерисовки графического окна. Параметр dt обозначает количество миллисекунд с момента последнего вызова OnDrawFrame
  OnDrawFrame: procedure(dt: real);
  /// Событие, происходящее при закрытии основного окна
  OnClose: procedure;

// -----------------------------------------------------
//>>     Функции пересечения# Intersection functions
// -----------------------------------------------------
/// Возвращает графический объект под точкой с координатами (x,y) или nil
function ObjectUnderPoint(x,y: real): ObjectWPF;
/// Возвращает True если графические объекты пересекаются
function ObjectsIntersect(o1,o2: ObjectWPF): boolean;

// -----------------------------------------------------
//>>     Другие подпрограммы# Other functions
// -----------------------------------------------------
/// Перемещает объект на передний план 
procedure ToFront(o: ObjectWPF);
/// Перемещает объект на задний план
procedure ToBack(o: ObjectWPF);

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;
//{{{--doc: Конец секции 2 }}} 

//procedure BeginFrameBasedAnimation(Draw: procedure; frate: integer := 61);

//procedure BeginFrameBasedAnimationTime(DrawT: procedure(dt: real));

/// Класс, содержащий константы направления
type Direction = class
  /// Направление "Влево"
  static property Left: (real,real) read (-1.0,0.0);
  /// Направление "Вправо"
  static property Right: (real,real) read (1.0,0.0);
  /// Направление "Вверх"
  static property Up: (real,real) read (0.0,-1.0);
  /// Направление "Вниз"
  static property Down: (real,real) read (0.0,1.0);
  /// Направление "Влево вверх"
  static property LeftUp: (real,real) read (-1.0,-1.0);
  /// Направление "Влево вниз"
  static property LeftDown: (real,real) read (-1.0,1.0);
  /// Направление "Вправо вверх"
  static property RightUp: (real,real) read (1.0,-1.0);
  /// Направление "Вправо вниз"
  static property RightDown: (real,real) read (1.0,1.0);
  /// Нулевое направление (объект неподвижен)
  static property Zero: (real,real) read (0.0,0.0);
  /// Направление "Вперёд" для объекта
  static function &Forward(obj: ObjectWPF): (real,real) 
    := (sin(Pi/180*obj.RotateAngle),-cos(Pi/180*obj.RotateAngle));
  /// Направление "Назад" для объекта
  static function &Backward(obj: ObjectWPF): (real,real) 
    := (-sin(Pi/180*obj.RotateAngle),cos(Pi/180*obj.RotateAngle));
  /// Направление "Влево" для объекта
  static function LeftSide(obj: ObjectWPF): (real,real) 
    := (-cos(Pi/180*obj.RotateAngle),-sin(Pi/180*obj.RotateAngle));
  /// Направление "Вправо" для объекта
  static function RightSide(obj: ObjectWPF): (real,real) 
    := (cos(Pi/180*obj.RotateAngle),sin(Pi/180*obj.RotateAngle));
end;

/// Не отображать слой графических объектов (обычно вызывается в начале до создания графических объектов)
procedure HideObjects;

/// Отображать слой графических объектов (вызывается после HideObjects и создания начальной сцены графических объектов)
procedure ShowObjects;

implementation

procedure HideObjects;
begin
  Invoke(()->begin host.Visibility := Visibility.Hidden end);
end;

procedure ShowObjects;
begin
  Invoke(()->begin host.Visibility := Visibility.Visible end);
end;

//procedure BeginFrameBasedAnimation(Draw: procedure; frate: integer) := GraphWPF.BeginFrameBasedAnimation(Draw,frate);

//procedure BeginFrameBasedAnimationTime(DrawT: procedure(dt: real)) := GraphWPF.BeginFrameBasedAnimationTime(DrawT);

function RGB(r,g,b: byte) := Color.Fromrgb(r, g, b);
function ARGB(a,r,g,b: byte) := Color.FromArgb(a, r, g, b);
function RandomColor := RGB(PABCSystem.Random(256), PABCSystem.Random(256), PABCSystem.Random(256));
function GrayColor(b: byte): Color := RGB(b,b,b);
function EmptyColor := ARGB(0,0,0,0);
function clRandom := RandomColor();
function Pnt(x,y: real) := new Point(x,y);
function Rect(x,y,w,h: real) := new System.Windows.Rect(x,y,w,h);
function RandomPoint(z: real): GPoint := Pnt(Random(z,GraphWindow.Width-z),Random(z,GraphWindow.Height-z));

var ColorsDict := new Dictionary<GColor,SolidColorBrush>;

function ColorBrush(c: Color): SolidColorBrush;
begin
  {if ColorsDict.ContainsKey(c) then
    Result := ColorsDict[c]
  else
  begin
    var scb := new SolidColorBrush(c);
    ColorsDict[c] := scb;
    Result := scb
  end;}
  Result := new SolidColorBrush(c);
end;  

procedure Redraw(p: ()->()) := GraphWPFBase.Invoke(p);

procedure SetLeft(Self: UIElement; l: integer) := Self.SetLeft(l);
procedure SetTop(Self: UIElement; t: integer) := Self.SetTop(t);

function MoveBy(Self: Point; vx,vy: real): Point; extensionmethod;
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

procedure ObjectWPF.AddChild(ch: ObjectWPF; al: Alignment);
begin
  if not Objects.l.Contains(ch) then
    raise new Exception('Добавляемый объект уже является дочерним');
  Invoke(AddChildP,ch,al);
end;  

function ObjectWPF.Intersects(ob: ObjectWPF): boolean;
begin
  Result := ObjectsIntersect(Self,ob);
end;

procedure ObjectWPF.InitOb(x,y,w,h: real; o: FrameworkElement; SetWH: boolean; Hidden: boolean);
begin
  can := new Canvas;
  gr := new Grid;
  rot := new RotateTransform(0);
  sca := new ScaleTransform;
  transl := new TranslateTransform;
  transfgroup := new TransformGroup;
  transfgroup.Children.Add(rot);
  transfgroup.Children.Add(sca);
  transfgroup.Children.Add(transl);
  rot.CenterX := w / 2;
  rot.CenterY := h / 2;
  can.RenderTransform := transfgroup;
  ob := o;
  if SetWH then 
    (ob.Width,ob.Height) := (w,h);
  MoveTo(x,y);
  //gr.Children.Add(ob);
  can.Children.Add(ob);

  (gr.Width,gr.Height) := (w,h);
  t := new TextBlock();
  t.FontFamily := new FontFamily('Arial');
  t.VerticalAlignment := VerticalAlignment.Center;
  t.HorizontalAlignment := HorizontalAlignment.Center;

  gr.Children.Add(t);
  can.Children.Add(gr);

  FontSize := 16;      

  if Hidden then
    Visible := False;
  Objects.Add(Self);  
end;

procedure ObjectWPF.AddChildP(ch: ObjectWPF; al: Alignment);
begin
  if (al=Alignment.RightTop) or (al=Alignment.RightCenter) or (al=Alignment.RightBottom) then
    ch.Left := Width-ch.Width
  else if (al=Alignment.CenterTop) or (al=Alignment.Center) or (al=Alignment.CenterBottom) then 
    ch.Left := (Width-ch.Width)/2
  else if (al=Alignment.LeftTop) or (al=Alignment.LeftCenter) or (al=Alignment.LeftBottom) then  
    ch.Left := 0;
    
  if (al=Alignment.RightBottom) or (al=Alignment.CenterBottom) or (al=Alignment.LeftBottom) then
    ch.Top := Height-ch.Height
  else if (al=Alignment.RightCenter) or (al=Alignment.Center) or (al=Alignment.LeftCenter) then
    ch.Top := (Height-ch.Height)/2
  else if (al=Alignment.LeftTop) or (al=Alignment.CenterTop) or (al=Alignment.RightTop) then  
    ch.Top := 0;

  ChildrenWPF.Add(ch);
  Objects.Destroy(ch);
  can.Children.Add(ch.can);
end;

procedure ObjectWPF.DeleteChildP(ch: ObjectWPF);
begin
  ChildrenWPF.Remove(ch);
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
  function IntersectP: boolean := ObjectsIntersectP(o1,o2);
end;

type OLHelper = auto class
  o: ObjectWPF;
  function f: List<ObjectWPF> := ObjectsIntersectPL(o);
end;


function ObjectsIntersect(o1,o2: ObjectWPF) 
  := Invoke&<boolean>(ObHelper.Create(o1,o2).IntersectP);
  
/// Возвращает список объектов, пересекающихся с данным
function IntersectionList(Self: ObjectWPF): List<ObjectWPF>; extensionmethod
  := Invoke&<List<ObjectWPF>>(OLHelper.Create(Self).f);

procedure ToFront(o: ObjectWPF) := Objects.ToFront(o);

procedure ToBack(o: ObjectWPF) := Objects.ToBack(o);

/// Перемещает объект на передний план
procedure ToFront(Self: ObjectWPF); extensionmethod := Objects.ToFront(Self);

/// Перемещает объект на задний план
procedure ToBack(Self: ObjectWPF); extensionmethod := Objects.ToBack(Self);

function operator implicit (t: (integer,integer)): (real,real); extensionmethod := (real(t[0]),real(t[1])); 

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
procedure SystemOnKeyDown(sender: Object; e: KeyEventArgs);
begin
  if OnKeyDown<>nil then
    OnKeyDown(e.Key);
end;

procedure SystemOnKeyUp(sender: Object; e: KeyEventArgs) := 
begin
  if OnKeyUp<>nil then
    OnKeyUp(e.Key);
end;
    
procedure SystemOnKeyPress(sender: Object; e: TextCompositionEventArgs) := 
begin
  if (OnKeyPress<>nil) and (e.Text<>nil) and (e.Text.Length>0) then
    OnKeyPress(e.Text[1]);
end;
    
procedure SystemOnResize(sender: Object; e: SizeChangedEventArgs) := 
begin
  if OnResize<>nil then
    OnResize();
end;

var LastUpdatedTimeWPF := new System.TimeSpan(integer.MinValue); 

procedure RenderFrameWPF(o: Object; e: System.EventArgs);
begin
  if OnDrawFrame<>nil then
  begin
    var e1 := RenderingEventArgs(e).RenderingTime;
    if LastUpdatedTimeWPF.Ticks = integer.MinValue then // первый раз
      LastUpdatedTimeWPF := e1;
    var dt := e1 - LastUpdatedTimeWPF;
    LastUpdatedTimeWPF := e1;  
    OnDrawFrame(dt.Milliseconds/1000);
  end;  
end;

procedure DrawWPFObjects(dc: DrawingContext);
begin
  var bmp := CreateRenderTargetBitmap;

  var rr := Rect(0,0,bmp.Width,bmp.Height);
  bmp.Render(host);
  dc.DrawImage(bmp,rr);
end;

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
    MainWindow.Closing += (sender,e) -> begin 
      if OnClose<>nil then
        OnClose;
    end;
    
    CompositionTarget.Rendering += RenderFrameWPF;
    
    Objects := new ObjectsType;
    Window := GraphWPF.Window;
    GraphWindow := GraphWPF.GraphWindow;

    host := new Canvas();
    host.ClipToBounds := True;
    {host.SizeChanged += (s,e) ->
    begin
      var sz := e.NewSize;
      host.DataContext := sz;
    end;}
    var g := MainWindow.Content as DockPanel;
    (g.children[0] as Canvas).ClipToBounds := False; // SSM 04/08/20 - возвращаем в False иначе графику GraphWPF становится не видно
    g.children.Add(host); // Слой графики WPF - последний
    AdditionalDrawOnDC += DrawWPFObjects;
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