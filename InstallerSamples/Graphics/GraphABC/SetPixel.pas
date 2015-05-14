uses GraphABC;

begin
  for var x:=0 to Window.Width-1 do
  for var y:=0 to Window.Height-1 do
    SetPixel(x,y,RGB(2*x-y,x-3*y,x+y));
end.