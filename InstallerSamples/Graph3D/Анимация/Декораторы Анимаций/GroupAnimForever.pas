uses Graph3D;

begin
  var c := Cube(-2,0,0,2,Colors.Beige);
  var c1 := Cube(2,0,0,2,Colors.Green);
  var a := c.AnimRotate(OrtZ,360,4).Forever * c1.AnimRotate(OrtZ,360,1).Forever;
  a.Begin;
end.