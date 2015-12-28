// Движение по траектории
uses ABCObjects,GraphABC;

const
/// Шаг по параметру кривой
  step = 0.03;
/// Задержка по времени, мс
  delay = 10;

type
  PointR = record
    x,y: real;
  end;

function Position(t: real): PointR; // астроида
begin
  var si := sin(1.5 * t);
  var co := cos(1.5 * t);
  Result.x := si*si*si;
  Result.y := co*co*co;
end;

function Position1(t: real): PointR; // фигура Лиссажу
begin
  Result.x := cos(4*t);
  Result.y := cos(2.97221*t + 2*Pi/3);
end;

function LogicalToScreen(p: PointR): Point;
begin
  var ww := WindowWidth div 2;
  var hh := WindowHeight div 2;
  Result.x := round((ww - 50) * p.x + ww);
  Result.y := round((hh - 50) * p.y + hh);
end;

procedure InitScreen;
begin
  SetBrushColor(clMoneyGreen);
  Rectangle(10,10,WindowWidth-10,WindowHeight-10);
  var p := LogicalToScreen(Position1(0));
  MoveTo(p.x,p.y);
end;

begin
  Window.IsFixedSize := True;
  Window.Title := 'Движение по траектории';
  SetWindowSize(640,480);
  CenterWindow;

  InitScreen;
  var c := new CircleABC(200,200,25,clGreen);
  var d := new StarABC(200,200,40,20,5,clYellow);

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
