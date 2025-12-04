// Графика. Draw и Fill - процедуры
uses GraphABC;

begin
  Window.Title := 'Draw и Fill - процедуры';
  for var i := 1 to 100 do
  begin
    Pen.Color := clRandom;
    var x := Random(Window.Width-100);
    var y := Random(Window.Height-100);
    DrawRectangle(x,y,x+Random(100),y+Random(100));
    Sleep(30);
  end;
  for var i := 1 to 100 do
  begin
    Brush.Color := clRandom;
    var x := Random(Window.Width-100);
    var y := Random(Window.Height-100);
    FillRectangle(x,y,x+Random(100),y+Random(100));
    Sleep(30);
  end;
end.