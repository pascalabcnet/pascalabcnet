uses Graph3D;

begin  
  Window.Caption := 'Развевающийся флаг';
  View3D.HideAll;
  Cylinder(0,4,0,6,0.07,Colors.Brown);
  var c := Cloth(0,4,0,3,4,6,'FlagOfRussia.png');
  c.WindSpeed := 6;
  c.WindDirection := 180;
  c.Gravity := 4;
  c.Damping := 0.999;
  c.Mass := 0.8;
  c.Iterations := 20;
  Sleep(1000);
  c.Stop;
  Sleep(1000);
  c.Start;
end.