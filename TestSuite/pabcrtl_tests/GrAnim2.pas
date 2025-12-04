// Графика. Использование LockDrawing-Redraw. Мерцание отсутствует
uses GraphABC;

begin
  Window.Title := 'Анимация без мерцания';
  var y := 200;
  Brush.Color := clGreen;
  // Отключаем рисование в окне
  LockDrawing;
  for var x := 70 to 500 do
  begin
    Window.Clear(clWhite);    
    Circle(x,y,50);
    // Перерисовываем окно из внеэранного буфера
    Redraw;
    Sleep(3);
  end;
end.