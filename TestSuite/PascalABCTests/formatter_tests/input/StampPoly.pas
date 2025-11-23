// Класс штампа правильного многоугольника
uses GraphABC;

type 
  RegularPolygonStamp = class
    x,y,r: real;
    n: integer;
    constructor (xx,yy,rr: real; nn: integer);
    begin
      x := xx; y := yy;
      r := rr; n := nn;
    end;
    procedure Stamp;
    begin
      var t := 0.0;
      var xr := r*cos(t);
      var yr := r*sin(t);
      MoveTo(Round(x + xr),Round(y + yr));
      for var i:=1 to n do
      begin
        t += 2*Pi/n;
        xr := Round(r*cos(t));
        yr := Round(r*sin(t));
        LineTo(Round(x + xr),Round(y + yr));
      end;  
    end;
    procedure MoveOn(dx,dy: real);
    begin
      x += dx; y += dy;
    end;
    function Clone: RegularPolygonStamp;
    begin
      Result := new RegularPolygonStamp(x,y,r,n);
    end;
  end;
  
begin
  var r := new RegularPolygonStamp(Window.Center.X,Window.Center.Y,50,6);
  r.Stamp;
  var t := 2*Pi/12;
  var rr := r.r*sqrt(3)+10;
  for var i:=1 to 6 do
  begin
    var r1 := r.Clone;
    r1.MoveOn(rr*cos(t),rr*sin(t));
    r1.Stamp;
    t += 2*Pi/6;
  end;
end. 