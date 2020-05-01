uses GraphWPF;

begin
  var x0 := 100.0;
  var v := 100;
  OnDrawFrame := dt -> 
  begin
    x0 += v * dt;
    Circle(x0,100,30,Colors.Yellow);
  end; 
end.