uses GraphABC;

begin
  Window.Title := '��������� �����';
  OnMouseDown := (x,y,mb) -> MoveTo(x,y);
  OnMouseMove := (x,y,mb) -> if mb=1 then LineTo(x,y);
end.