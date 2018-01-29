uses GraphWPF;

begin
  Window.Title := 'Рисование мышью';
  Pen.Color := Colors.Blue;
  Pen.Width := 3;
  OnMouseDown := (x,y,mb) -> MoveTo(x,y);
  OnMouseMove := (x,y,mb) -> if mb=1 then LineTo(x,y);
  OnKeyDown := k -> if k = Key.Space then Window.Save('a.png');
end.