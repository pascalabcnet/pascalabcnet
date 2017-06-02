// Свойства ScaleX и ScaleY класса PictureABC
uses ABCObjects,GraphABC;

const delay = 2;

var p: PictureABC;

begin
  Window.Title := 'Движение и масштабирование рисунка по горизонтали';
  SetWindowSize(640,260);
  p := PictureABC.Create(100,100,'demo.bmp');
  while True do
  begin
    for var x:=100 to 450 do
    begin
      Sleep(delay);
      p.MoveOn(1,0);
    end;
    for var i:=100 downto -100 do
    begin
      Sleep(delay);
      var w := p.Width;
      p.ScaleX := i/100;
      p.MoveOn(w - p.Width,0);
    end;
    for var x:=450 downto 100 do
    begin
      Sleep(delay);
      p.MoveOn(-1,0);
    end;
    for var i:=-100 to 100 do
    begin
      Sleep(delay);
      p.ScaleX:=i/100;
    end;
  end;
end.

