uses GraphWPF;

begin
  OnMouseDown := (x,y,mb) -> if mb=1 then Circle(x,y,5);
  OnKeyDown := k -> Print(k);
end.