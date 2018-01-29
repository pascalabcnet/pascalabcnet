uses GraphWPF;

begin
  Window.Title := 'Дуги и секторы';
  var (x,y) := (200,Window.Height/2);
  Circle(x,y,5);
  for var i:=1 to 18*2 do
    Arc(x,y,5*i,0,10*i);
  (x,y) := (600,Window.Height/2);  
  for var i:=1 to 12 do
  begin
    Brush.Color := RandomColor;
    Sector(x,y,180,30*(i-1),30*i);
  end;
end.