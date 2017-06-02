uses GraphABC;

const 
  n = 17; // количество точек 
  n1 = 7; // через сколько точек соединять

begin
  var a := -Pi/2;
  var Center := Window.Center;
  var Radius := Window.Height/2.2;
  MoveTo(Round(Center.X+Radius*cos(a)),Round(Center.Y+Radius*sin(a)));
  for var i:=1 to n do
  begin
    a += n1*2*Pi/n;
    LineTo(Round(Center.X+Radius*cos(a)),Round(Center.Y+Radius*sin(a)));
  end;
end.