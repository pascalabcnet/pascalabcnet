uses Graph3D;

begin
  View3D.BackgroundColor := Colors.Black;
  var ps := ParticleSystem(0,0,0,'smoke.png');
  // ps.TextureName := 'smoke.png';
  ps.LifeTime := 5;
  ps.FadeOutTime := 0;
  ps.VelocityDamping := 0.999;
  ps.AngularVelocity := 10;
  ps.SizeRate := 1;
  ps.AccelerationDirection := V3D(3,0,-1);
  ps.Acceleration := 4;
  ps.AccelerationSpreading := 10;
  ps.EmitRate := 100;
  ps.StartRadius := 0;
  ps.StartSize := 0.5;
  ps.StartDirection := V3D(0,0,1);
  ps.StartSpreading := 10;
  ps.StartVelocity := 4;
  ps.StartVelocityRandomness := 2;
  ps.Position := P3D(0, 0, 0);
end.