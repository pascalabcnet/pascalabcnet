uses Graph3D;

begin
  View3D.Title := 'Вращение Земли';
  View3D.CameraMode := CameraMode.Inspect;
  View3D.ShowCoordinateSystem := False;
  View3D.ShowGridLines := False;
  View3D.ShowViewCube := False;
  View3D.ShowCameraInfo := False;
  View3D.BackgroundColor := Colors.Black;
  
  var c := Sphere(0,0,0,5,Colors.Wheat);
  c.Material := ImageMaterial('Earth.jpg');
  var gr := 0;
  var dgr := 1;
  while True do
  begin
    c.Rotate(V3D(0,0,1),1);
    Sleep(10);
    gr += dgr;
    if (gr<=0) or (gr>=255) then
      dgr := - dgr;
    View3D.BackgroundColor := RGB(gr,gr,gr);
  end;
end.