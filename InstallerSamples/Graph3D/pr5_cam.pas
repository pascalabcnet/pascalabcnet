uses Graph3D;

begin
  Window.Title := 'Вращение камеры';
  var tp := Teapot(0,0,2,Colors.Green);
  tp.Scale(2);
  Camera.Position := P3D(8,16,20);
  Camera.LookDirection := Camera.Position.Multiply(-1).ToVector3D;
  var d := 26.0;
  {loop 200 do
  begin
    Sleep(20);
    Camera.SetDistanse(d);
    d -= 0.05;    
  end;}
  var t := 0.0;
  while True do
  begin
    Sleep(10);
    Camera.Position := P3D(15*cos(t),15*sin(t),10);
    Camera.UpDirection := V3D(0,0,1);
    //Camera.LookDirection := Camera.Position.Multiply(-1).ToVector3D;
    t += 2*Pi/360/2;
  end;
  
  Camera.LookDirection := Camera.Position.Multiply(-1).ToVector3D;
end.