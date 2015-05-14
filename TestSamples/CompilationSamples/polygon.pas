// Демонстрация возможностей векторной графики. Правильные многоугольники
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
end;

var i: integer;

begin
  SetWindowCaption('Черепашья графика: правильные многоугольники');
  Init;
  SetWindowSize(730,300);
  ToPoint(60,90);
  for i:=3 to 8 do
  begin
    MnogoUg(50,i);
    OnVector(120,0);
  end;  
  ToPoint(60,220);
  for i:=9 to 14 do
  begin
    MnogoUg(50,i);
    OnVector(120,0);
  end;  
end.
