uses Graph3D;

begin
  var jmp := 0.3;
  var time := 0.5;
  var c := Box(0,0,2,1,2,4,Colors.Green);
  var a := c.AnimMoveOnZ(-0.4,time).AutoReverse.Forever * c.AnimScaleZ(0.95,time).AutoReverse.Forever * c.AnimMoveOnX(1*10,time*10);
  a.Begin;
end.