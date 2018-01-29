uses GraphWPF;

begin
  Window.Title := 'Рисование эллипсов';
  Pen.Width := 1;
  var n := 20000;
  for var i:=1 to n do
  begin
    if i mod 10000 = 0 then
      Println(i,MillisecondsDelta); 
    Brush.Color := RandomColor;
    Ellipse(Random(800),Random(600),Random(20),Random(20));
  end;
end.