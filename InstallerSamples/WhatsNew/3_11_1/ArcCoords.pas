// Использование DrawArc и ATan2 (3.11.1)
uses Coords;

begin
  SetMouseDown((x,y,mb) -> begin
    if mb <> 2 then exit;
    var xr := ScreenToRealX(x);
    var yr := ScreenToRealY(y);
    DrawCircle(xr,yr,0.1,Colors.Red);
    DrawLine(0,0,xr,yr);
    var angle := RadToDeg(ATan2(yr,xr));
    DrawArc(0,0,2,0,angle);
    Println(angle);
  end);
end.