uses GraphWPF;

begin
  Parameters.ArrowSizeAcross := 4;
  Parameters.ArrowSizeAlong := 10;
  SetMathematicCoords;
  var (p1,p2,p3) := (Pnt(0,0),Pnt(2,3),Pnt(4,-1));
  Arrow(p1,p2);
  Arrow(p2,p3);
  Arrow(p3,p1);
end.