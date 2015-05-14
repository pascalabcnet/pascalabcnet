// Демонстрация возможносткй векторной графики. Ромашка
uses Turtle,GraphABC;

const 
  N=14;
  R=300;
  Rmin=50;
  d=50;
procedure Lepestok (Rmin,R,p: real);
 procedure Vspom(sign: integer);
 var n,i: integer;
 begin
  Center;
  SetTurtlePhi(p);
  Turn(90);
  Forw(RMin);
  Turn(-sign*30);
  PenDown;
  n:=30;
  for i:=1 to 6 do
  begin
    forw(R*sin(Pi/n));
    Turn(sign*360/n);
  end;
  PenUp;
 end;
 
begin
  Vspom(1);
  Vspom(-1);
end;


var i: integer;

begin
  SetWindowCaption('Черепашья графика: ромашка');
  SetWindowSize(500,500);
  Init;
  for i:=0 to N-1 do
    Lepestok(Rmin,R,i*360/N);
  SetBrushColor(clYellow);
  Ellipse(WindowWidth div 2 - d,WindowHeight div 2 - d,
          WindowWidth div 2 + d,WindowHeight div 2 + d);
end.
