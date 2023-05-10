uses GraphWPF;

begin
  SetMathematicCoords;
  var (p1,p2,p3) := (Pnt(0,0),Pnt(2,3),Pnt(4,-1));
  Circle(p1,0.1);
  Circle(p2,0.1);
  Circle(p3,0.1);
  TextOut(p1,p1);
  TextOut(p2,p2);
  TextOut(p3,p3);
end.