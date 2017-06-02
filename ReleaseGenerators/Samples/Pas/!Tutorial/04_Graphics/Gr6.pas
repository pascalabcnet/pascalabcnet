// Графика. Прозрачность
uses GraphABC;

begin
  Window.Title := 'Демонстрация прозрачности';
  
  Brush.Color := clRed;
  FillRectangle(50,50,100,Window.Height - 50);
  Brush.Color := clGreen;
  FillRectangle(150,50,200,Window.Height - 50);
  Brush.Color := clBlue;
  FillRectangle(250,50,300,Window.Height - 50);
  Brush.Color := ARGB(127,255,0,0);
  FillRectangle(20,100,420,150);
  Brush.Color := ARGB(63,0,127,0);
  FillRectangle(20,200,420,250);
  Brush.Color := ARGB(31,0,0,255);
  FillRectangle(20,300,420,350);
end.