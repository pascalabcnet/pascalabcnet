// Класс штампа прямоугольника с методами увеличения-уменьшения
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
    procedure Increase(dw,dh: integer);
    begin
      w += dw; h += dh;    
    end;
    procedure Decrease(dw,dh: integer);
    begin
      Increase(-dw,-dh);      
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
    r.Decrease(8,8);
    r.MoveOn(4,4);
    r.Stamp;
  end;
end. 