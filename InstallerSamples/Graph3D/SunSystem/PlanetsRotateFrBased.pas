uses Graph3D;

begin
  Window.Title := 'Вращение планет';
  View3D.HideAll;
  
  var s := Sphere(0,0,0,30);
  s.BackMaterial := ImageMaterial('skymap.jpg');

  var earth := Sphere(-7,0,0,1,ImageMaterial('earthmap.jpg'));
  var moon := Sphere(2,0,0,0.5,ImageMaterial('moonmap.jpg'));
  earth.AddChild(moon);
  var Sun := Sphere(0,0,0,2,ImageMaterial('sunmap.jpg'));
  Sun.AnimRotate(OrtZ,-360,20.sec).Forever.Begin;
  
  var tr := ParametricTrajectory(0,2*Pi,100,t->P3D(7*cos(t),7*sin(t),0));
  Polyline3D(tr,1.2,GrayColor(80));
  
  var tr1 := ParametricTrajectory(0,2*Pi,100,t->P3D(2*cos(t),2*sin(t),0));
  var pl1 := Polyline3D(tr1,1.2,GrayColor(80));
  earth.AddChild(pl1);

  OnDrawFrame := dt -> begin
    moon.RotateAt(OrtZ,360*dt/3,P3D(-2,0,0));
    earth.RotateAtAbsolute(OrtZ,360*dt/6,Origin);
  end;
end.