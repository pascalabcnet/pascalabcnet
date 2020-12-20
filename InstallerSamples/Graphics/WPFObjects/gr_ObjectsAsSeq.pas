uses WPFObjects;

function GenRandomWPF: ObjectWPF;
begin
  var x := Random(Window.Width);
  var y := Random(Window.Height);
  case Random(3) of
    0: Result := new CircleWPF(x,y,Random(15,20),RandomColor);
    1: Result := new EllipseWPF(x,y,Random(15,60),Random(15,60),RandomColor); 
    2: Result := new RegularPolygonWPF(x,y,Random(15,30),Random(3,8),RandomColor);
  end;
end;

begin
  loop 100 do
    GenRandomWPF;
  
  Sleep(1000);
  foreach var o in Objects.Seq do
    match o with
      CircleWPF(c): c.Radius += 10;
      EllipseWPF(e): e.AnimMoveBy(Random(-50,50),Random(-50,50),1);
      RegularPolygonWPF(r) when r.Count<6: r.SetBorder;  
    end
end.