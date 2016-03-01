// Имитация кругов на воде от капель дождя
uses GraphABC;

const speed = 2;

procedure Kaplia(x0,y0: integer);
begin
  var r := 1;
  for var i:=0 to 63 do
  begin
    Pen.Color := RGB(i*4,i*4,i*4);
    Circle(x0,y0,r);
    if i mod speed = 0 then Sleep(10);
    Pen.Color := clWhite;
    Circle(x0,y0,r);
    r += 2;    
  end;
end;

const z=50;

begin
  Window.Title := 'Капли дождя';
  SetWindowSize(800,600);
  while True do
    Kaplia(Random(z,WindowWidth-z),Random(z,WindowHeight-z));
end.
