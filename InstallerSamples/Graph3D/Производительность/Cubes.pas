uses Graph3D;

begin
  View3D.ShowGridLines := False;
  View3D.ShowViewCube := False;
  View3D.ShowCoordinateSystem := False;
  var w := 1;
  for var x := -5 to 5 do
  for var y := -5 to 5 do
  for var z := -4 to 6 do
    Cube(x*w,y*w,z*w,w*0.96,RandomColor)
end.