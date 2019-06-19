uses GraphWPF;

begin
  Window.SetSize(640, 480);
  var m := MatrGen(640,480,(x,y)->RGB(2 * x - y, x - 3 * y, x + y));
  MillisecondsDelta.Println;  
  DrawPixels(0,0,m);
  MillisecondsDelta.Println;  
end.