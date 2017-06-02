// Класс штампа прямоугольника с методами увеличения-уменьшения
uses GraphABC;

type 
  RectangleStamp = auto class
    x,y,w,h: integer;
    procedure Stamp := Rectangle(x,y,x+w,y+h);
    procedure Increase(dw,dh: integer);
    begin
      w += dw; h += dh;    
    end;
    procedure Decrease(dw,dh: integer) := Increase(-dw,-dh);      
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