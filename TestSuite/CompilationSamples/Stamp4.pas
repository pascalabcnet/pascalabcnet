// Класс штампа прямоугольника с методами увеличения-уменьшения от центра
uses GraphABC;

type 
  RectangleStamp = class
    x,y,w,h: integer;
    constructor (xx,yy,ww,hh: integer);
    begin
      x := xx; y := yy;
      w := ww; h := hh;
    end;
    procedure Stamp;
    begin
      Rectangle(x,y,x+w,y+h);
    end;
    procedure IncreaseFromCenter(dw: integer);
    begin
      w += dw*2; h += dw*2;
      x -= dw; y -= dw;
    end;
    procedure DecreaseFromCenter(dw: integer);
    begin
      IncreaseFromCenter(-dw);      
    end;
    procedure MoveOn(dx,dy: integer);
    begin
      x += dx; y += dy;
    end;
  end;
  
begin
  var r := new RectangleStamp(100,100,300,300);
  r.Stamp;
  while r.w>2 do
  begin
    r.DecreaseFromCenter(4);
    r.Stamp;
  end;
end. 