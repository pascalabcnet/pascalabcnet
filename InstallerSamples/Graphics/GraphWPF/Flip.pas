uses GraphWPF,Timers;

begin
  var b := new Bitmap('m1.png');
  b.ScaleX := 0.5;
  b.ScaleY := 0.5;
  var b1 := b.Clone;
  b1.FlipVertical;
  
  OnDrawFrame := dt -> 
  begin
    DrawImage(100,100,b);
    DrawImage(300,100,b1);
  end;  
  
  var t := new Timer(500);
  t.TimerProc := procedure -> b.FlipHorizontal;
  t.Start;
end.