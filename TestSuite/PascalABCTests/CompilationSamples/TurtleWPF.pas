unit TurtleWPF;

interface

uses GraphWPF;

/// Передвигает Черепаху вперёд на расстояние r
procedure Forw(r: real);

/// Передвигает Черепаху назад на расстояние r
procedure Back(r: real);

/// Поворачивает Черепаху на угол da по часовой стрелке
procedure Turn(da: real);

/// Поворачивает Черепаху на угол da влево
procedure TurnLeft(da: real);

/// Поворачивает Черепаху на угол da вправо
procedure TurnRight(da: real);

/// Возвращает случайный цвет
function RandomColor: Color;

/// Возвращает цвет по красной, зеленой и синей составляющей (в диапазоне 0..255)
function RGB(r,g,b: byte): Color;

/// Устанавливает скорость черепашки (от 1 до 10)
procedure SetSpeed(sp: integer);

/// Прячет Черепаху
procedure Hide;

/// Показывает Черепаху
procedure Show;

/// Опускает перо Черепахи
procedure Down;

/// Поднимает перо Черепахи
procedure Up;

/// Устанавливает ширину линии 
procedure SetWidth(w: real);

/// Устанавливает цвет линии 
procedure SetColor(c: GColor);

/// Перемещает Черепаху в точку (x,y)
procedure MoveTo(x,y: real);

/// Сохраняет изображение, нарисованное Черепахой, в файл fname (например, 'a.png')
procedure Save(fname: string);

type Colors = GraphWPF.Colors;

{type turtle = static class
  /// Передвигает Черепаху вперёд на расстояние r
  static procedure Forw(r: real) := TurtleWPF.Forw(r);
  /// Поворачивает Черепаху на угол da по часовой стрелке
  static procedure Turn(da: real) := TurtleWPF.Turn(da);
  /// Возвращает случайный цвет
  static function RandomColor: Color := TurtleWPF.RandomColor;
  /// Возвращает цвет по красной, зеленой и синей составляющей (в диапазоне 0..255)
  static function RGB(r,g,b: byte): Color := TurtleWPF.RGB(r,g,b);
  /// Устанавливает скорость черепашки (от 1 до 10)
  static procedure SetSpeed(sp: integer) := TurtleWPF.SetSpeed(sp);
  /// Прячет Черепаху
  static procedure Hide := TurtleWPF.Hide;
  /// Показывает Черепаху
  static procedure Show := TurtleWPF.Show;
  /// Опускает перо Черепахи
  static procedure Down := TurtleWPF.Down;
  /// Поднимает перо Черепахи
  static procedure Up := TurtleWPF.Up;
  /// Устанавливает ширину линии 
  static procedure SetWidth(w: real) := TurtleWPF.SetWidth(w);
  /// Устанавливает цвет линии 
  static procedure SetColor(c: GColor) := TurtleWPF.SetColor(c);
  /// Перемещает Черепаху в точку (x,y)
  static procedure MoveTo(x,y: real) := TurtleWPF.MoveTo(x,y);
  /// Сохраняет изображение, нарисованное Черепахой, в файл fname (например, 'a.png')
  static procedure Save(fname: string) := TurtleWPF.Save(fname);
end;}

implementation

{$resource 'turtle.png'}

uses WPFObjects,GraphWPFBase;
uses System.Windows.Media.Animation;
uses System.Windows.Media;
uses System.Windows.Media.Imaging;

var 
  TurtleX: real := 400;
  TurtleY: real := 300;
  w := 24;  // размер изображения Черепахи
  
type
  TurtleHelper = class(PictureWPF)
  private
    function CreateBitmapImageFromResource(resourcename: string): BitmapImage;
    begin
      var bi := new BitmapImage();
      bi.BeginInit();
      bi.StreamSource := GetResourceStream(resourcename);
      bi.EndInit();
      Result := bi;
    end;
    procedure InitObFromResource(x,y,w,h: real; fname: string);
    begin
      var b := CreateBitmapImageFromResource(fname);
      Rest(x,y,w,h,b);
    end;
    procedure AnimMoveByPE(a,b,sec: real);
    begin
      var v := CreateVisual;
      var p := Center;
      var (x,y) := (transl.X,transl.Y);
      var ax := new DoubleAnimation(a + x, System.TimeSpan.FromSeconds(sec));
      var ay := new DoubleAnimation(b + y, System.TimeSpan.FromSeconds(sec));
      ax.FillBehavior := FillBehavior.Stop;
      ay.FillBehavior := FillBehavior.Stop;
      var transl1 := transl;
      ax.Completed += (o,e) -> begin
        DrawOnVisual(v,dc->dc.DrawLine(GraphWPF.Pen.PenConstruct,p,Pnt(p.X + a,p.Y + b)));
        transl1.X := x + a; // transl - ошибка с лямбдами!
      end;  
      ay.Completed += (o,e) -> begin
        transl1.Y := y + b;
      end;  
      ax.CurrentTimeInvalidated  += (o,e) -> 
        DrawOnVisual(v,dc->dc.DrawLine(GraphWPF.Pen.PenConstruct,p,Pnt(Center.X,Center.Y)));
      transl.BeginAnimation(TranslateTransform.XProperty, ax, HandoffBehavior.Compose);
      transl.BeginAnimation(TranslateTransform.YProperty, ay, HandoffBehavior.Compose);
    end;
  public
    constructor (x,y,w,h: real; fname: string) := Invoke(InitObFromResource,x,y,w,h,fname);
    procedure AnimMoveByE(a,b,sec: real);
    begin
      Invoke(AnimMoveByPE,a,b,sec);
      Sleep(Round(sec*1000 + 50)); // 50 мс - За это время анимация должна закончиться
    end;
    procedure AnimRotateE(a,sec: real);
    begin
      AnimRotate(a,sec);
      Sleep(Round(sec*1000 + 50));
    end;
  end;
  
var t: TurtleHelper;

var 
  a: real := 0;
  dr := False;
  speed := 10;
  Window := GraphWPF.Window;
  
function RandomColor := GraphWPF.RandomColor;  
function RGB(r,g,b: byte) := GraphWPF.RGB(r, g, b);

var sl := 30;

procedure SetSpeed(sp: integer);
begin
  speed := sp.Clamp(1,10);
  if sp > 10 then
    sl := 0
  else sl := 30;
end;

procedure Hide;
begin
  t.Visible := False;
end;

procedure Show;
begin
  t.Visible := True;
end;

/// Поворачивает Черепаху на угол da по часовой стрелке
procedure Turn(da: real);
begin
  a += da;
  if speed = 10 then
  begin
    t.Rotate(da);
    Sleep(sl);
  end
  else
  begin
    if dr then 
      t.AnimRotateE(da,Abs(da/(60*speed))) 
    else
    begin
      t.AnimRotate(da,Abs(da/(60*speed)));
      Sleep(Round(Abs(da/(60*speed))*1000)+50);
    end;  
  end;
end;

procedure TurnLeft(da: real) := Turn(-da);

procedure TurnRight(da: real) := Turn(da);

/// Продвигает Черепаху вперёд на расстояние r
procedure Forw(r: real);
begin
  var v := r * Vect(Cos(DegToRad(a)),Sin(DegToRad(a)));
  var (a,b) := (v.X,v.Y);
  TurtleX += a;
  TurtleY += b;
  if (speed = 10) or (r < 20) then
  begin
    if dr then 
      GraphWPF.LineTo(TurtleX,TurtleY)
    else GraphWPF.MoveTo(TurtleX,TurtleY);
    t.MoveBy(a,b);
    Sleep(sl);
  end
  else
  begin
    if dr then 
    begin  
      t.AnimMoveByE(a,b,r/(50*speed)); // 450 в секунду при скорости 9  
      GraphWPF.MoveTo(TurtleX,TurtleY);
    end
    else 
    begin  
      t.AnimMoveBy(a,b,r/(50*speed));
      Sleep(Round(r/(50*speed)*1000)+50);
      GraphWPF.MoveTo(TurtleX,TurtleY);
    end;  
  end;
end;

procedure Back(r: real) := Forw(-r);

/// Опускает хвост Черепахи
procedure Down := dr := True;

/// Поднимает хвост Черепахи
procedure Up := dr := False;

/// Устанавливает ширину линии 
procedure SetWidth(w: real) := GraphWPF.Pen.Width := w;

/// Устанавливает цвет линии 
procedure SetColor(c: GColor) := GraphWPF.Pen.Color := c;

/// Перемещает Черепаху в точку (x,y)
procedure MoveTo(x,y: real);
begin
  TurtleX := x;
  TurtleY := y;
  GraphWPF.MoveTo(x,y);
  t.Center := Pnt(x,y);
end;

procedure Save(fname: string) := Window.Save(fname);

 
begin
  Window.Title := 'Исполнитель Черепаха WPF';

  t := new TurtleHelper(TurtleX-w/2,TurtleY-w/2,w,w,'turtle.png');
  //Pen.RoundCap := True;
  MoveTo(TurtleX,TurtleY); 
  a -= 90;
end.