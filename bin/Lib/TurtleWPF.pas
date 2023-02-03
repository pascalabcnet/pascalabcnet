unit TurtleWPF;

{$resource 'turtle.png'}

uses GraphWPF,WPFObjects,GraphWPFBase;
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
      var ax := new DoubleAnimation(a + transl.X, System.TimeSpan.FromSeconds(sec));
      var ay := new DoubleAnimation(b + transl.Y, System.TimeSpan.FromSeconds(sec));
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
      TurtleX += a;
      TurtleY += b;
      GraphWPF.MoveTo(TurtleX,TurtleY);
    end;
    procedure AnimRotateE(a,sec: real);
    begin
      AnimRotate(a,sec);
      Sleep(Round(sec*1000 + 50));
    end;
  end;
  Colors = GraphWPF.Colors;
  
var t: TurtleHelper;

var 
  a: real := 0;
  dr := False;
  speed := 10;

procedure SetSpeed(sp: integer);
begin
  speed := sp;
end;

var sl := 30;

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

/// Продвигает Черепаху вперёд на расстояние r
procedure Forw(r: real);
begin
  var v := r * Vect(Cos(DegToRad(a)),Sin(DegToRad(a)));
  if (speed = 10) or (r < 20) then
  begin
    TurtleX += v.X;
    TurtleY += v.Y;
    if dr then 
      LineTo(TurtleX,TurtleY)
    else MoveTo(TurtleX,TurtleY);
    t.MoveBy(v.X,v.Y);
    Sleep(sl);
  end
  else
  begin
    if dr then 
      t.AnimMoveByE(v.X,v.Y,r/(50*speed)) // 450 в секунду при скорости 9  
    else 
    begin  
      t.AnimMoveBy(v.X,v.Y,r/(50*speed));
      Sleep(Round(r/(50*speed)*1000)+50);
    end;  
  end;
end;

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
 
begin
  Window.Title := 'Исполнитель Черепаха WPF';

  t := new TurtleHelper(TurtleX-w/2,TurtleY-w/2,w,w,'turtle.png');
  //Pen.RoundCap := True;
  MoveTo(TurtleX,TurtleY); 
  a -= 90;
end.