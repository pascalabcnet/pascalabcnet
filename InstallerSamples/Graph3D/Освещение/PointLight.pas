uses Graph3D;

begin
  //Lights.AddDirectionalLight(GrayColor(100),V3D(-1,-1,-1));
  var p := P3D(-3,3,3);
  Sphere(p,0.3,Colors.White);
  View3D.ShowGridLines := false;
  Cylinder(0,0,0,5,2,True,Colors.Yellow);
  Rectangle3D(0,0,-0.01,15,15,OrtZ,OrtX,ImageMaterial('трава.jpg',0.2,0.2));
  Sleep(2000);
  Lights.AddPointLight(GrayColor(64),p);
  Lights.Proba;
  //Rectangle3D(0,0,-0.01,15,15,OrtZ,OrtX,Colors.Green);
end.