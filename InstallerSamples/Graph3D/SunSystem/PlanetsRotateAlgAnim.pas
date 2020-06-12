uses Graph3D;

begin
  Window.Title := 'Вращение планет';
  View3D.HideAll;
  View3D.BackgroundColor := Colors.Black;
  
  var s := Sphere(0,0,0,30);
  s.BackMaterial := ImageMaterial('skymap.jpg');

  var Sun := Sphere(0,0,0,2,ImageMaterial('sunmap.jpg'));
  var Earth := Sphere(-7,0,0,1,ImageMaterial('earthmap.jpg'));
  var Moon := Sphere(2,0,0,0.5,ImageMaterial('moonmap.jpg'));
  Earth.AddChild(Moon);
  
  var tr := ParametricTrajectory(0,2*Pi,100,t->P3D(7*cos(t),7*sin(t),0));
  Polyline3D(tr,1.2,GrayColor(70));
  
  Moon.AnimRotateAt(OrtZ,360,P3D(-2,0,0),2.sec).Forever.Begin;
  Earth.AnimRotateAtAbsolute(OrtZ,360,Origin,20.sec).Forever.Begin;
  Earth.AnimRotate(OrtZ,-360,5.sec).Forever.Begin;
  Sun.AnimRotate(OrtZ,-360,20.sec).Forever.Begin;
end.