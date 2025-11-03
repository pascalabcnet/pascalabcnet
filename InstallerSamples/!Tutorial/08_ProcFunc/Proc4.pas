// Упаковка параметров в класс
uses GraphWPF;

type 
  Point = auto class
    x,y: integer;
  end;

procedure Line(p1,p2: Point) := Line(p1.x,p1.y,p2.x,p2.y);

begin
  var p1 := new Point(10,10);
  var p2 := new Point(10,210);
  var p3 := new Point(210,10);
  Line(p1,p2);
  Line(p2,p3);
  Line(p3,p1);
end.  