uses GraphWPF;

procedure Светофор(x,y,r: real);
begin
  Rectangle(x,y,4*r,10*r,Colors.LightGray);
  x += 2*r;
  y += 2*r;
  var dy := 3*r;

  Circle(x,y,r,Colors.Red);
  Circle(x,y + dy,r,Colors.Yellow);
  Circle(x,y + 2*dy,r,Colors.Green);
end;

begin
  Pen.Width := 2;
  Window.Title := 'Светофор';
  Светофор(150,40,50);
end.