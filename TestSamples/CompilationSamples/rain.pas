// Имитация кругов на воде от капель дождя
uses GraphABC;

const
  speed=2;

procedure Kaplia(x0,y0: integer);
var
  i,r: integer;
begin
  r:=1;
  for i:=0 to 63 do
  begin
    SetPenColor(RGB(i*4,i*4,i*4));
    Circle(x0,y0,r);
    if i mod speed = 0 then Sleep(10);
    SetPenColor(clWhite);
    Circle(x0,y0,r);
    r:=r+2;    
  end;
end;

const z=50;

begin
  SetWindowCaption('Капли дождя');
  while True do
    Kaplia(Random(WindowWidth-2*z)+z,Random(WindowHeight-2*z)+z);
end.
