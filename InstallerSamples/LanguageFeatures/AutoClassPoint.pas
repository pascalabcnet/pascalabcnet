type 
  Point = auto class
    x,y: integer;
    procedure MoveOn(dx,dy: integer) := (x,y) := (x+dx,y+dy);
    function Distance(p: Point) := sqrt(sqr(x-p.x)+sqr(y-p.y));
    class function operator implicit(t: (integer,integer)): Point := new Point(t[0],t[1]);
  end;
  
begin
  var p: Point;
  p := (2,3);
  Println(p);
end.