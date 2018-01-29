uses Graph3D;

begin
  var p := PrismWireFrame(Origin,13,3,5);
  var pp := p.Points;
  foreach var x in pp do
    p.AddChild(Sphere(x,0.1,Colors.Gray));
  var m := 'A';  
  foreach var x in pp do
  begin
    var b := Text3D(x.Move(0.3,0.3,0.4),m,0.5);
    p.AddChild(b);
    Inc(m);
  end;
  var p1 := p.Clone;
  p1.MoveOnX(-5);
  p1.AnimRotate(OrtZ,360,10).Forever.Begin;
end.