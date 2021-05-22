uses Graph3D;

begin
  Window.Title := 'Creation of objects... wait!';
  HideObjects;
  View3D.ShowGridLines := False;
  View3D.ShowViewCube := False;
  View3D.ShowCoordinateSystem := False;
  
  var l := new List<SphereT>;
  
  var w := 1;
  for var x := -5 to 5 do
  for var y := -5 to 5 do
  for var z := -5 to 5 do
     l.Add(Sphere(x*w,y*w,z*w,w*0.5,RandomColor));
  ShowObjects;

  Window.Title := 'Spheres animation';
  
  foreach var x in l do
    x.AnimMoveBy(Random,Random,Random).AutoReverse.Forever.Begin;
end.