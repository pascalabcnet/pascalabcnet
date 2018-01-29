uses GraphWPF;

begin
  Window.Title := 'Таблица умножения';
  Font.Size := 16;
  var n := 9;
  var w := 40;
  var (x0,y0) := (50,50);
  for var i:=0 to n-1 do
  for var j:=0 to n-1 do
  begin
    var (xx,yy) := (x0+i*w,y0+j*w);
    Rectangle(xx,yy,w,w);
    DrawText(xx,yy,w,w,(i+1)*(j+1));
  end;  
end.
