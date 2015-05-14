type 
  Point=record
    x,y:integer;
  end;

var p:Point;

begin
  p.x:=1;
  p.y:=2;
  writeln(p.x);
  readln;
end.