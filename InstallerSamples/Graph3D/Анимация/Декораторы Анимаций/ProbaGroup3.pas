uses Graph3D;

begin
  var bb := ArrGen(15,i -> Object3D(Box(0.5*i,0,0,0.1,6,2,RandomColor)));
  var g := Group(bb).MoveOnX(-5);
  g.AnimRotateAt(ortZ,360,P3D(-1,0,0),2).Forever.Begin;  
end.