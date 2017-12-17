uses Graph3D;

begin
  View3D.ShowGridLines := False;
  var disc := Cylinder(0,0,0,0.2,6,Colors.DeepPink);

  var r := 5;  
  foreach var x in Partition(0,2*Pi,12) do 
    disc.AddChild(Sphere(r*cos(x),r*sin(x),0.2,0.1,Colors.White));
  disc.AnimRotate(OrtZ,360,2).AccelerationRatio(1,1).AutoReverse.Forever.Begin;
end.