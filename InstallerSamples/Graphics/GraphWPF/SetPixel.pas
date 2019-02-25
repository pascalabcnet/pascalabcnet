uses GraphWPF;

begin
  Window.SetSize(640, 480);
  for var x := 0 to Window.Width.Round do
  for var y := 0 to Window.Height.Round do
    SetPixel(x, y, RGB(2 * x - y, x - 3 * y, x + y));
  Milliseconds.Println;  
end.