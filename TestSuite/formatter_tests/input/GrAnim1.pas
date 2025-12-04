// Графика. Пример "наивной" анимации. Эффект мерцания
uses GraphABC;

begin
  Window.Title := 'Демонстрация простой анимации';
  var y := 200;
  Brush.Color := clGreen;
  for var x := 70 to 500 do
  begin
    Window.Clear(clWhite);    
    Circle(x,y,50);
    Sleep(3);
  end;
end.