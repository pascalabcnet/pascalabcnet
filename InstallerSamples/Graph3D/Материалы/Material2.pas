uses Graph3D;

begin
  for var i:=0 to 6 do
    Sphere(6-2*i,-3,0,1,
      Materials.Diffuse(Colors.Green) + 
      Materials.Specular(255-32*i,100));
  for var i:=0 to 6 do
    Sphere(6-2*i,0,0,1,
      Materials.Diffuse(Colors.Green) +
      Materials.Specular(255,100-15*i));
  for var i:=0 to 6 do
    Sphere(6-2*i,3,0,1,
      Materials.Diffuse(Colors.Green) + 
      Materials.Specular(128,100) + 
      Materials.Emissive(GrayColor(15*i)));
end.