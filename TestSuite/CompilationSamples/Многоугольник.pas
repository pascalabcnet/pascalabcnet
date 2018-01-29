uses GraphWPF;

procedure Многоугольник(x0,y0,r: real; n: integer);
begin
  var a := Pi / 2;
  MoveTo(x0 + r * Cos(a), y0 - r * Sin(a));
  loop n do 
  begin
    a += 2 * Pi / n;
    //FillCircle(x0 + r * Cos(a), y0 - r * Sin(a),3,Colors.Black);
    LineTo(x0 + r * Cos(a), y0 - r * Sin(a));
  end;
end;

begin
  var (x0,y0) := (400.0,300.0);
  var r := 30.0;
  for var n := 3 to 11 do
    Многоугольник(x0,y0,r+(n-3)*30,n)
end.
