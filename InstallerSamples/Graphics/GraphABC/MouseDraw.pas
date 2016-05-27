uses GraphABC;

begin
  Window.Title := 'Рисование мышью';
  OnMouseDown := (x,y,mb) -> MoveTo(x,y);
  OnMouseMove := (x,y,mb) -> if mb=1 then LineTo(x,y);
end.