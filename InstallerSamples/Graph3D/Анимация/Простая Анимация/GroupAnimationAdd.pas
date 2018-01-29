uses Graph3D;

begin
  View3D.ShowGridLines := False;
  
  var a := new GroupAnimation;
  
  var p := P3D(9,-3,0);
  var sz := 1.3;
  var h := 4;
  for var n:=3 to 10 do
  begin
    var pr := Prism(p,n,sz,4,Colors.Beige);
    var prw := PrismWireFrame(p,n,sz+0.01,4);
    p := p.MoveX(-2*sz-0.3);
    a += pr.AnimRotate(OrtZ,360,10).Forever;
    a += prw.AnimRotate(OrtZ,360,10).Forever;
  end;

  p := P3D(9,3,0);
  for var n:=3 to 10 do
  begin
    var pr := Pyramid(p,n,sz,4,Colors.Beige);
    var prw := PyramidWireFrame(p,n,sz+0.01,4);
    p := p.MoveX(-2*sz-0.3);
    a += pr.AnimRotate(OrtZ,360,10).Forever;
    a += prw.AnimRotate(OrtZ,360,10).Forever;
  end;
  a.Begin;
end.