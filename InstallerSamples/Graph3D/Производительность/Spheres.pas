uses Graph3D;

begin
  //view3d.ShowCameraInfo := True;
  View3D.ShowGridLines := False;
  View3D.ShowViewCube := False;
  View3D.ShowCoordinateSystem := False;
  
  var l := new List<SphereT>;
  
  //Invoke(()->begin
  var w := 1;
  for var x := -5 to 5 do
  for var y := -5 to 5 do
  for var z := -5 to 5 do
     l.Add(Sphere(x*w,y*w,z*w,w*0.5,RandomColor));
  //end);
  
  foreach var x in l do
    x.AnimMoveOn(Random,Random,Random).AutoReverse.Forever.Begin;
  
  //Print(Milliseconds);
end.