uses GraphWPF;

begin
  Window.SetSize(640, 480);
  SetPixels(0,0,640,480,(x, y)->RGB(2 * x - y, x - 3 * y, x + y));
  Milliseconds.Println;  
end.