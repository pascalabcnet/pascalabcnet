uses GraphWPF;

begin
  Window.Title := 'Отражение шарика. Вещественное направление движения';
  Brush.Color := Colors.Beige;
  var x := 400.0;
  var y := 300.0;
  var dx := 2.1;
  var dy := -1.2;
  Circle(x,y,20);
  while True do
  begin
    Sleep(10);
    Window.Clear;
    x += dx;
    y += dy;
    if not x.Between(0,Window.Width) then 
      dx := -dx;
    if not y.Between(0,Window.Height) then 
      dy := -dy;
    Circle(x,y,20);
    if Milliseconds>2000 then
      Window.Title := 'Секунды: ' + (Milliseconds div 100)/10;
  end;
end.