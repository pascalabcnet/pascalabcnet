uses ABCObjects,GraphABC;

const r=150;

var
  s: StarABC;
  c: CircleABC;
  i: integer;
  x0,y0: integer;

begin
  x0:=WindowWidth div 2;
  y0:=WindowHeight div 2;
  s:=new StarABC(x0+r,y0,50,25,5,clRed);
  while True do
  begin
    Inc(i);
    s.MoveTo(x0-s.Width div 2 + round(r*cos(i*Pi/180)),y0-s.Width div 2 + round(r*sin(i*Pi/180)));
    sleep(10);
  end;
end.
