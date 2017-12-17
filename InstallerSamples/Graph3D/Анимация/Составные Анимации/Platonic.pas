uses Graph3D;

begin
  Window.Title := 'Платоновы тела';
  var РисоватьОписанныеСферы := True;
  var p := P3D(8,0,0);
  var p1 := p;
  var h := -4;
  Icosahedron(p,2,Colors.Green);
  p.Offset(h,0,0);
  Dodecahedron(p,2,Colors.Blue);
  p.Offset(h,0,0);
  Tetrahedron(p,2,Colors.Red);
  p.Offset(h,0,0);
  Octahedron(p,2,Colors.Magenta);
  p.Offset(h,0,0);
  Cube(p,2*2/Sqrt(3),Colors.Brown);
  
  p := p1;
  if РисоватьОписанныеСферы then
    loop 5 do
    begin
      var s := Sphere(p,2.0,DiffuseMaterial(Colors.Gold.ChangeAlpha(64))+SpecularMaterial(64));
      s.BackMaterial := nil;
      p.Offset(h,0,0);
    end;
end.