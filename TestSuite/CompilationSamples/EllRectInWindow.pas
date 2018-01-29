uses GraphWPF;

begin
  Pen.Width := 1;
  Rectangle(0,0,Window.Width-1,Window.Height-1);
  Ellipse((Window.Width-1)/2,(Window.Height-1)/2,(Window.Width-1)/2,(Window.Height-1)/2);
end.