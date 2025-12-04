// Графика. Сглаживание
uses GraphABC;

begin
  Window.Title := 'Демонстрация сглаживания';
  TextOut(40,20,'Со сглаживанием');
  Pen.Width := 2;
  
  Ellipse(40,60,300,200);
  Line(340,60,600,130);
  Line(600,130,340,200);
  
  SetSmoothingOff;
  TextOut(40,240,'Без сглаживания');
  Ellipse(40,280,300,420);
  Line(340,280,600,350);
  Line(600,350,340,420);
end.