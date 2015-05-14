uses ABCObjects;

const delay=2;

var
  p: PictureABC;
  x,y: integer;
  i,w: integer;

begin
  x:=100;
  y:=100;
  p:=PictureABC.Create(x,y,'demo.bmp');
  while True do
  begin
  for x:=100 to 450 do
    begin
      Sleep(delay);
      p.MoveOn(1,0);
    end;
    for i:=100 downto -100 do
    begin
      Sleep(delay);
      w:=p.Width;
      p.ScaleX:=i/100;
      p.Left:=p.Left+w-p.Width;
    end;
    for x:=450 downto 100 do
    begin
      Sleep(delay);
      p.MoveOn(-1,0);
    end;
    for i:=-100 to 100 do
    begin
      Sleep(delay);
      p.ScaleX:=i/100;
    end;
  end;
end.

