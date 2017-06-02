// Упаковка параметров в запись
uses GraphABC;

type 
  Point = record
    x,y: integer;
    constructor (xx,yy: integer);
    begin
      x := xx;
      y := yy;
    end;
  end;

procedure Line(p1,p2: Point);
begin
  GraphABC.Line(p1.x,p1.y,p2.x,p2.y);
end;

begin
  var p1 := new Point(10,10);
  var p2 := new Point(10,210);
  var p3 := new Point(210,10);
  Line(p1,p2);
  Line(p2,p3);
  Line(p3,p1);
end.  