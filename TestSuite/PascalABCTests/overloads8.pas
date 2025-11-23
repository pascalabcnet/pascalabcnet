type Point = auto class
  x,y: integer;
end;

var i := 1;

function RandomPoint: Point;
begin
  Result := new Point(i, i);
  Inc(i);
end;

var a: array of Point;

begin
  a := ArrGen(10,i->RandomPoint);
  assert(a.Min(p->p.x) = 1);
  assert(a.Max(p->p.x) = 10);
end.