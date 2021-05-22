uses Graph3D;

function MultipleClones(c: Object3D; N: integer): Group3D;
begin
  var g := Group(c);
  loop N-1 do
  begin
    c := c.Clone;
    c.MoveByX(-1.5);
    if c is ObjectWithMaterial3D then 
      (c as ObjectWithMaterial3D).Color := RandomColor;
    g.AddChild(c);
  end;
  Result := g;
end;

begin
  HideObjects;
  var b := Box(0,0,2,14.5,1,4,Colors.Orange);
  var c := Cube(6.75,0,4.5,1,RandomColor);
  var c1 := Cone(6.75,0,5,1.5,0.4,RandomColor);
  var g := MultipleClones(c,10);
  var gp := MultipleClones(c1,10);
  var gg := Group(g,b,gp);
  var g1 := gg.Clone.MoveByY(6);
  var g2 := gg.Clone.MoveByY(-6);
  gg.Rotate(OrtZ,90);
  var g3 := gg.Clone.MoveByX(-6);
  gg.MoveByX(6);
  var ggg := Group(gg,g1,g2,g3);
  ShowObjects;

  ggg.Save('Замок.xaml');
  //ggg.AnimRotate(OrtZ,360,10).Forever.begin;
end.