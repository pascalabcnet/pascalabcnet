// Вращение многоугольника
uses Turtle,GraphABC;

procedure MnogoUg(r: real; n: integer);
var i: integer;
begin
  SetPixel(round(TurtleX),round(TurtleY),clBlack);
  Forw(r);
  Turn(90+180/n);
  PenDown;
  for i:=1 to n do
  begin
    Forw(2*r*sin(Pi/n));
    Turn(360/n);
  end;
  Turn(90-180/n);
  PenUp;
  Forw(r);
  Turn(180);
end;

const
  n=4;

var
  i,di: integer;

begin
  SetWindowCaption('Черепашья графика: вращение');
  SetWindowSize(320,165);
  Init;
  i:=5;
  while True do
  begin
    if i=50 then di:=-1;
    if i=5 then di:=1;
    i:=i+di;
    SetPenColor(clBlack);
    MnogoUg(i,n);
    Sleep(15);
    SetPenColor(clWhite);
    MnogoUg(i,n);
    Turn(8);
  end;
end.
