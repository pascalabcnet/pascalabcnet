// Демонстрация преобразований системы координат
uses GraphABC;

begin
  Window.Title := 'Преобразования системы координат';
  Window.IsFixedSize := True;

  Coordinate.Origin := Window.Center;
  Circle(0,0,200);
  for var i := 1 to 12 do
  begin
    Circle(0,180,10);
    Coordinate.Angle := Coordinate.Angle + 30; 
  end;  
end.
