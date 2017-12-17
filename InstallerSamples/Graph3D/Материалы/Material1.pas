uses Graph3D;

begin
  //Sphere(0,0,0,2,Materials.Diffuse(Colors.Green)+Materials.Specular(128));
  var m := DiffuseMaterial(Colors.Green) + SpecularMaterial(128,30) + EmissiveMaterial(RGB(0,64,0));
  Sphere(4,0,0,2,m);
  Cube(8,0,0,4,RainbowMaterial);
  Cube(0,0,0,4,DiffuseMaterial(Colors.Green) + ImageMaterial('dog.png',0.5,0.5));
end.