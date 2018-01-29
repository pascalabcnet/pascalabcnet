uses Graph3D;

begin
  var m := ImageMaterial('Cells1.png',0.2,0.2);
  var c := Cube(0,0,2,4,m);
  c.Rotate(OrtZ,35)
end.