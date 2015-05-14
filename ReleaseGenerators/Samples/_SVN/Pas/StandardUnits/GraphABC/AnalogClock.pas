uses GraphABC,System;

begin
  Font.Size := 80;
  var x0 := (Window.Width - TextWidth('00:00:00')) div 2;
  var y0 := (Window.Height - TextHeight('00:00:00')) div 2;
  while True do
  begin
    var t := DateTime.Now;
    var s := string.Format('{0:d2}:{1:d2}:{2:d2}',t.Hour,t.Minute,t.Second);
    TextOut(x0,y0,s);
    Sleep(1000);
  end;
end.