uses GraphWPF;

begin
  Window.Title := 'Простая анимация';
  var x := 30;
  Brush.Color := Colors.Beige;
  Circle(x,50,20);
  loop 600 do
  begin
    Sleep(10);
    Window.Clear;
    x += 1;
    Circle(x,50,20);
    Window.Title := '' + (Milliseconds div 100)/10;
  end;
end.