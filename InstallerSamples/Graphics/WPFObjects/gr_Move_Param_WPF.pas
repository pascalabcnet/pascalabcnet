// Движение по траектории
uses WPFObjects,GraphWPF;

const
/// Шаг по параметру кривой
  step = 0.02;
/// Задержка по времени, мс
  delay = 5;

type
  PointR = record
    x,y: real;
  end;

function Position(t: real): PointR; // астроида
begin
  var si := sin(1.7 * t);
  var co := cos(1.5 * t);
  Result.x := si*si*si;
  Result.y := co*co*co;
end;

function Position1(t: real): PointR; // фигура Лиссажу
begin
  Result.x := cos(4*t);
  Result.y := cos(2.97221*t + 2*Pi/3);
end;

function LogicalToScreen(p: PointR): GPoint;
begin
  var ww := Window.Width / 2;
  var hh := Window.Height / 2;
  Result.x := Round((ww - 50) * p.x + ww);
  Result.y := Round((hh - 50) * p.y + hh);
end;

procedure InitScreen;
begin
  Brush.Color := Colors.LightGreen;
  Rectangle(10,10,Window.Width-20,Window.Height-20);
  var p := LogicalToScreen(Position1(0));
  MoveTo(p.x,p.y);
end;

begin
  //Window.IsFixedSize := True;
  Window.Title := 'Движение по траектории';
  Window.SetSize(640,480);
  Window.CenterOnScreen;

  InitScreen;
  var c := new CircleWPF(200,200,25,Colors.Green);
  var d := new StarWPF(200,200,40,20,5,Colors.Yellow);

  var t: real := 0;
  while True do
  begin
    c.Center := LogicalToScreen(Position1(t));
    d.Center := LogicalToScreen(Position(t));
    if t<20*Pi then
      LineTo(c.Center.x,c.Center.y)
    else
    begin
      t := 0;
      InitScreen;
    end;
    t += step;
    Sleep(delay);
  end;
end.
