// Класс штампа треугольника
uses GraphABC;

type 
  TriangleStamp = auto class
    x,y,w,orient: integer;
    procedure Stamp;
    begin
      MoveTo(x,y);
      var dx := w;
      var dy := w;
      case orient of
     2: dx := -dx;
     3: dy := -dy;
     4: begin dx := -dx; dy := -dy; end;
      end;
      LineTo(x+dx,y);
      LineTo(x,y+dy);
      LineTo(x,y);      
    end;
    procedure MoveOn(dx,dy: integer);
    begin
      x += dx; y += dy;
    end;
  end;
  
begin
  var r := new TriangleStamp(200,200,100,1);
  r.Stamp;
  r.orient := 2;
  r.Stamp;
  r.orient := 3;
  r.Stamp;
  r.orient := 4;
  r.Stamp;
end. 