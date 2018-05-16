uses GraphWPF;

begin
  var x1,y1: real;
  var c: Color;
  OnMouseDown := procedure(x,y,mb) -> begin
    (x1,y1) := (x,y);
    c := RandomColor;
  end;
  OnMouseMove := procedure(x,y,mb) -> if mb=1 then
  begin
    var r := Sqrt(Sqr(x1-x)+Sqr(y1-y));
    Window.Clear;
    Circle(x1,y1,r,c);
  end;
end.