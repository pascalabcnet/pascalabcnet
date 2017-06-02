// Класс штампа креста
uses GraphABC;

type 
  CrossStamp = class
    x,y,w: integer;
    constructor (xx,yy,ww: integer);
    begin
      x := xx; y := yy;
      w := ww; 
    end;
    procedure Stamp;
    begin
      MoveTo(x,y);
      LineTo(x+w,y);
      LineTo(x+w,y+w);
      LineTo(x+2*w,y+w);
      LineTo(x+2*w,y);
      LineTo(x+3*w,y);
      LineTo(x+3*w,y-w);
      LineTo(x+2*w,y-w);
      LineTo(x+2*w,y-2*w);
      LineTo(x+w,y-2*w);
      LineTo(x+w,y-w);
      LineTo(x,y-w);
      LineTo(x,y);
    end;
    procedure MoveOn(dx,dy: integer);
    begin
      x += dx; y += dy;
    end;
    procedure MoveOnRel(a,b: integer);
    begin
      MoveOn(a*w,b*w);
    end;
    function Clone := new CrossStamp(x,y,w);
  end;
  
begin
  var r := new CrossStamp(100,150,20);
  for var k:=1 to 2 do
  begin
    var r1 := r.Clone;
    for var i:=1 to 8 do
    begin
      r1.Stamp;
      r1.MoveOnRel(2,1);
    end;
    r.MoveOnRel(-1,2);
  end;
end. 