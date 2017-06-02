// Демонстрация преобразований системы координат
uses GraphABC;

begin
  Window.Title := 'Преобразования системы координат';
  Window.IsFixedSize := True;

  // Поворот
  Coordinate.Angle := 30; 
  // Масштаб
  Coordinate.Scale := 3;
  // Начало координат - в центре окна
  Coordinate.Origin := Window.Center;

  Brush.Color := clMoneyGreen;
  Ellipse(-50,-20,50,20);
  Line(-70,0,70,0);
  Line(0,-40,0,40);
end.
