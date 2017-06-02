// Иллюстрация прозрачности
uses GraphABC;
 
begin
  for var Transparency:=0 to 255 do
  begin
    Brush.Color := ARGB(Transparency,Random(256),Random(256),Random(256));
    FillCircle(Random(Window.Width),Random(Window.Height),Random(20,60));
    sleep(100);
  end;
end.