// Движение по траектории
uses ABCObjects,GraphABC;

const
  step = 0.002;
  a = 200;

type
  PointR = record
    x,y: real;
  end;

function Position(t: real): PointR; // астроида
var si,co: real;
begin
  si := sin(1.5 * t);
  co := cos(1.5 * t);
  Result.x := si*si*si;
  Result.y := co*co*co;
end;

function Position1(t: real): PointR; // фигура Лиссажу
begin
  Result.x := cos(4*t);
  Result.y := cos(2.97221*t + 2*Pi/3);
end;

function LogicalToScreen(p: PointR): Point;
var ww,hh: real;
begin
  ww := WindowWidth div 2;
  hh := WindowHeight div 2;
  Result.x := round((ww - 50) * p.x + ww);
  Result.y := round((hh - 50) * p.y + hh);
end;

procedure InitScreen;
var p: Point;
begin
  SetBrushColor(clMoneyGreen);
  Rectangle(10,10,WindowWidth-10,WindowHeight-10);
  p:=LogicalToScreen(Position1(0));
  MoveTo(p.x,p.y);
end;

var
  c,d: ObjectABC;
  t: real;

begin
  SetWindowCaption('Движение по траектории');
  SetWindowSize(800,600);
  CenterWindow;

  InitScreen;
  c := CircleABC.Create(200,200,25,clGreen);
  d := StarABC.Create(200,200,40,20,5,clYellow);

  t := 0;
  while True do
  begin
    c.Center:=LogicalToScreen(Position1(t));
    d.Center:=LogicalToScreen(Position(t));
    if t<20*Pi then
      LineTo(c.Center.x,c.Center.y)
    else
    begin
      t:=0;
      InitScreen;
    end;
    t := t + step;
  end;
end.
